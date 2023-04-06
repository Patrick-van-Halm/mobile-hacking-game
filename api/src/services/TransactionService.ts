import { Request, Response, RequestHandler } from "express"
import { IDatabase } from "pg-promise";
import { Server } from "socket.io";
import { IsPersonOrAdmin, IsUserOrAdmin } from "../Middlewares/Authorization";
import { BaseService } from "./@BaseService";
import { ParamsDictionary } from "express-serve-static-core";
import { ParsedQs } from "qs";
import { body } from "express-validator";
import { ValidatorShowErrors } from "../Middlewares/Validator";
import { nanoid } from "nanoid";

export class TransactionService extends BaseService {
    constructor(db: IDatabase<{}>, io: Server) {
        super(db, io, "/transactions");
    }

    // Gets all the Nexcoin transactions for the current user or all transactions if the user is an admin
    protected All(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        IsPersonOrAdmin,
        async (req: Request, res: Response) => {
            if(res.locals.isAdmin) return res.send(await this.db.manyOrNone("SELECT * FROM nexcoin_transactions"));

            let incomingTransactions = await this.db.manyOrNone("SELECT * FROM nexcoin_transactions WHERE receiver_person_id = $1", [res.locals.person.id]);
            for(const transaction of incomingTransactions){
                // Remove id
                delete transaction.id;
                
                // Get sender
                transaction.sender = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [transaction.sender_person_id]);
                delete transaction.sender_person_id;

                // Get receiver
                transaction.receiver = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [transaction.receiver_person_id]);
                delete transaction.receiver_person_id;

                // Get job if it is set
                if(transaction.job_id != null){
                    transaction.job = await this.db.oneOrNone("SELECT * FROM jobs WHERE id = $1", [transaction.job_id]);

                    // Remove contact person id and performing player id since sender is contact person and receiver is performing player
                    transaction.job.contact_person = transaction.sender;
                    delete transaction.job.contact_person_id;
                    transaction.job.performing_player = transaction.receiver;
                    delete transaction.job.performing_player_id;
                    
                    // Delete id
                    delete transaction.job.id;
                }
                delete transaction.job_id;
            }

            let outgoingTransactions = await this.db.manyOrNone("SELECT * FROM nexcoin_transactions WHERE sender_person_id = $1", [res.locals.person.id]);
            for(const transaction of outgoingTransactions){
                // Remove id
                delete transaction.id;

                // Get sender
                transaction.sender = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [transaction.sender_person_id]);
                delete transaction.sender_person_id;

                // Get receiver
                transaction.receiver = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [transaction.receiver_person_id]);
                delete transaction.receiver_person_id;

                // Get job if it is set
                if(transaction.job_id != null){
                    transaction.job = await this.db.oneOrNone("SELECT * FROM jobs WHERE id = $1", [transaction.job_id]);

                    // Remove contact person id and performing player id since sender is contact person and receiver is performing player
                    delete transaction.job.contact_person_id;
                    transaction.job.contact_person = transaction.receiver;
                    delete transaction.job.performing_player_id;
                    transaction.job.performing_player = transaction.sender;
                    
                    // Delete id
                    delete transaction.job.id;
                }
                delete transaction.job_id;
            }

            res.send({
                incoming: incomingTransactions,
                outgoing: outgoingTransactions
            });
        }
    ]};

    protected Get(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        async (req: Request, res: Response) => {
            res.status(404);
        }
    ]};
    
    // Creates a new transaction between two persons
    protected Post(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        body("receiver_id")
            .isString()
            .isLength({min: 21, max: 21})
            .custom(async (value: string) => (await this.db.oneOrNone("SELECT public_id FROM persons WHERE public_id = $1", [value])) != null ? Promise.resolve() : Promise.reject("Person does not exist")),
        body("sender_id")
            .isString()
            .isLength({min: 21, max: 21})
            .custom(async (value: string) => (await this.db.oneOrNone("SELECT public_id FROM persons WHERE public_id = $1", [value])) != null ? Promise.resolve() : Promise.reject("Person does not exist"))
            .optional({nullable: true}),
        body("job_id")
            .isString()
            .isLength({min: 21, max: 21})
            .custom(async (value: string) => (await this.db.oneOrNone("SELECT public_id FROM jobs WHERE public_id = $1", [value])) != null ? Promise.resolve() : Promise.reject("Job does not exist"))
            .optional({nullable: true}),
        body("amount")
            .isInt({min: 1}),
        IsUserOrAdmin(this.db),
        IsPersonOrAdmin,
        ValidatorShowErrors,
        async (req: Request, res: Response) => {
            // Check if sender is set and if the sender is the current user or an admin
            if(!res.locals.isAdmin && req.body.sender_id != null && req.body.sender_id != res.locals.person.public_id) return res.status(403).send("You are not allowed to send Nexcoin on behalf of another person");

            // Check if sender is not sending Nexcoin to himself
            if(req.body.sender_id == req.body.receiver_id) return res.status(400).send("You cannot send Nexcoin to yourself");

            // Get the sender and receiver
            const sender = await this.db.oneOrNone("SELECT id, player_id FROM persons WHERE public_id = $1", [req.body.sender_id ?? res.locals.person.public_id]);
            const receiver = await this.db.oneOrNone("SELECT id, player_id FROM persons WHERE public_id = $1", [req.body.receiver_id]);

            // Check if the sender has enough Nexcoin
            if(sender.player_id != null){
                let sum = 0;
                const incomingTransactions = await this.db.manyOrNone("SELECT * FROM nexcoin_transactions WHERE receiver_person_id = $1", [sender.id]);
                for(const transaction of incomingTransactions){
                    sum += transaction.amount;
                }
                const outgoingTransactions = await this.db.manyOrNone("SELECT * FROM nexcoin_transactions WHERE sender_person_id = $1", [sender.id]);
                for(const transaction of outgoingTransactions){
                    sum -= transaction.amount;
                }
    
                if(sum < req.body.amount) return res.status(403).send("You do not have enough Nexcoin to send this amount");
            }

            // Create transaction
            const transaction = await this.db.one("INSERT INTO nexcoin_transactions (public_id, sender_person_id, receiver_person_id, job_id, amount) VALUES ($1, $2, $3, $4, $5) RETURNING *", [nanoid(), sender.id, receiver.id, req.body.job_id, req.body.amount]);

            // Add sender and receiver to the response
            transaction.sender = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [transaction.sender_person_id]);
            transaction.receiver = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [transaction.receiver_person_id]);

            // Add job to the response
            if(transaction.job_id != null){
                transaction.job = await this.db.oneOrNone("SELECT public_id, name, description, created_at, updated_at FROM jobs WHERE id = $1", [transaction.job_id]);
            }

            // Dont include database ids in the response
            delete transaction.sender_person_id;
            delete transaction.receiver_person_id;
            delete transaction.job_id;
            delete transaction.id;

            // Send websocket event to the receiver
            if(receiver.player_id != null){
                this.io.to(receiver.player_id).emit("MoneyReceived", transaction);
            }

            res.send(transaction);
        }
    ]};

    // No one should be able to edit a transaction
    protected Patch(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        IsPersonOrAdmin,
        async (req: Request, res: Response) => {
            res.status(404);
        }
    ]};

    // No one should be able to delete a transaction
    protected Delete(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        async (req: Request, res: Response) => {
            res.status(404);
        }
    ]};
}