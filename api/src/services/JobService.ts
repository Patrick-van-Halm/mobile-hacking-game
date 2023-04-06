import { Request, Response, RequestHandler } from "express"
import { ParamsDictionary } from "express-serve-static-core";
import { ParsedQs } from "qs";
import { nanoid } from 'nanoid';
import { body } from 'express-validator';
import { IDatabase } from "pg-promise";
import { Server } from "socket.io";
import { IsAdmin, IsUserOrAdmin, IsPersonOrAdmin } from "../Middlewares/Authorization";
import { BaseService } from "./@BaseService";
import { ValidatorShowErrors } from "../Middlewares/Validator";

export class JobService extends BaseService {
    constructor(db: IDatabase<{}>, io: Server) {
        super(db, io, "/jobs");
    }

    protected All(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        IsPersonOrAdmin,
        async (req: Request, res: Response) => {
            // Get all jobs from the database corrisponding to the player or everyone if the user is an admin
            if(res.locals.isAdmin){
                return res.send(await this.db.manyOrNone("SELECT public_id, contact_person_id, performing_player_id, job_type, payout_amount, min_level, min_faction_reputation, data, deadline, completed, created_at, updated_at FROM jobs"));
            }

            // Using res.locals.person.id is safe because IsPersonOrAdmin middleware is used
            // Get all uncompleted jobs that no one is performing, or the jobs that the player is performing
            const jobs = await this.db.manyOrNone("SELECT public_id, contact_person_id, performing_player_id, job_type, payout_amount, min_level, min_faction_reputation, data, deadline, completed, created_at, updated_at FROM jobs WHERE (deadline > now() AND performing_player_id IS NULL) OR performing_player_id = $1", [res.locals.person.id]);
            for(const job of jobs){
                // Get the contact person
                job.contact_person = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [job.contact_person_id]);
                delete job.contact_person_id;
                
                // Delete the performing_player_id it is not needed since the player is already known
                if(job.performing_player_id == res.locals.person.id) job.performing_player_id = res.locals.player.public_id;
                else delete job.performing_player_id;
            }

            res.send(jobs);
        }
    ]};

    protected Get(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        IsPersonOrAdmin,
        async (req: Request, res: Response) => {
            // Get the job from the database
            const job = await this.db.oneOrNone("SELECT public_id, contact_person_id, performing_player_id, job_type, payout_amount, min_level, min_faction_reputation, data, deadline, completed, created_at, updated_at FROM jobs WHERE public_id = $1", [req.params.id]);
            if(!job) return res.status(404).json({ errors: [{msg: "Job not found"}] });

            // Get the contact person
            job.contact_person = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [job.contact_person_id]);
            delete job.contact_person_id;

            // Get the performing player
            if(job.performing_player_id){
                job.performing_player = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [job.performing_player_id]);
                delete job.performing_player_id;
            }

            res.send(job);
        }
    ]};
    
    protected Post(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        body("contact_person_id")
            .isLength({min: 21, max: 21})
            .custom(async (value: string) => (await this.db.oneOrNone("SELECT public_id FROM persons WHERE public_id = $1", [value])) != null ? Promise.resolve() : Promise.reject("Person does not exist")),
        body("job_type")
            .isIn(['UserCredentials', 'AnyCredentials']),
        body("payout_amount")
            .isInt({min: 1}),
        body("min_level")
            .isInt({min: 0}),
        body("min_faction_reputation")
            .isInt({min: 0}),
        body("data")
            .isJSON(),
        body("deadline")
            .isISO8601()
            .toDate(),
        IsAdmin,
        ValidatorShowErrors,
        async (req: Request, res: Response) => {
            // Get the contact person id from the database using the public_id
            req.body.contact_person_id = (await this.db.oneOrNone("SELECT id FROM persons WHERE public_id = $1", [req.body.contact_person_id])).id;

            // Create the job
            const job = await this.db.one("INSERT INTO jobs (public_id, contact_person_id, job_type, payout_amount, min_level, min_faction_reputation, data, deadline) VALUES ($1, $2, $3, $4, $5, $6, $7, $8) RETURNING *", [
                nanoid(21),
                req.body.contact_person_id,
                req.body.job_type,
                req.body.payout_amount,
                req.body.min_level,
                req.body.min_faction_reputation,
                req.body.data,
                req.body.deadline
            ]);

            // Get the contact person
            job.contact_person = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [job.contact_person_id]);
            delete job.contact_person_id;

            // Delete the performing_player_id it is not needed since the player is already known
            delete job.performing_player_id;

            // Todo: Send websocket event to all connected users that a new job has been created
            res.send(job);
        }
    ]};

    protected Patch(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        body("performing_player_id")
            .custom(async (value: string) => (await this.db.oneOrNone("SELECT public_id FROM players WHERE public_id = $1", [value])) != null ? Promise.resolve() : Promise.reject("Player does not exist"))
            .optional({nullable: true}),
        body("completed")
            .isBoolean()
            .optional({nullable: true}),
        body("deadline")
            .isISO8601()
            .toDate()
            .optional({nullable: true}),
        body("data")
            .isJSON()
            .optional({nullable: true}),
        body("payout_amount")
            .isInt({min: 1})
            .optional({nullable: true}),
        body("min_level")
            .isInt({min: 0})
            .optional({nullable: true}),
        body("min_faction_reputation")
            .isInt({min: 0})
            .optional({nullable: true}),
        IsUserOrAdmin(this.db),
        IsPersonOrAdmin,
        async (req: Request, res: Response) => {
            if(!res.locals.isAdmin){
                if(req.body.completed) {
                    // Check if the player is performing the job
                    const job = await this.db.oneOrNone("SELECT * FROM jobs WHERE public_id = $1", [req.params.id]);
                    if(!job) return res.status(404).json({ errors: [{msg: "Job not found"}] });

                    // Check if the player is allowed to complete the job
                    if(job.performing_player_id != res.locals.person.id) return res.status(403).json({ errors: [{msg: "You are not allowed to complete this job"}] });

                    // Update the job
                    await this.db.none("UPDATE jobs SET completed = $1 WHERE public_id = $2", [req.body.completed, req.params.id]);

                    
                    // Create transaction for the player that completed the job
                    const transaction = await this.db.one("INSERT INTO nexcoin_transactions (public_id, sender_person_id, receiver_person_id, job_id, amount) VALUES ($1, $2, $3, $4, $5) RETURNING *", [nanoid(), job.contact_person_id, res.locals.person.id, job.id, job.payout_amount]);

                    // Delete id
                    delete transaction.id;

                    // Get the sender person
                    transaction.sender_person = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [transaction.sender_person_id]);
                    delete transaction.sender_person_id;

                    // Get the receiver person
                    transaction.receiver_person = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [transaction.receiver_person_id]);
                    delete transaction.receiver_person_id;

                    // Get the job
                    transaction.job = await this.db.oneOrNone("SELECT public_id, job_type, payout_amount, min_level, min_faction_reputation, data, deadline, completed, created_at, updated_at FROM jobs WHERE id = $1", [transaction.job_id]);
                    delete transaction.job_id;
                    
                    // Send websocket event to the player that completed the job
                    this.io.to(res.locals.player.id).emit("MoneyReceived", transaction);

                    return res.send();
                }

                if(req.body.performing_player_id) {
                    // Check if the job is not completed and the performing player is not already set
                    const job = await this.db.oneOrNone("SELECT * FROM jobs WHERE public_id = $1", [req.params.id]);
                    if(!job) return res.status(404).json({ errors: [{msg: "Job not found"}] });

                    // Check if the player is allowed to perform the job
                    if(job.completed || job.performing_player_id) return res.status(403).json({ errors: [{msg: "You are not allowed to perform this job"}] });

                    // Get performing player
                    const performingPlayer = await this.db.oneOrNone("SELECT * FROM players WHERE public_id = $1", [req.body.performing_player_id]);

                    // Update the job
                    await this.db.none("UPDATE jobs SET performing_player_id = $1 WHERE public_id = $2", [performingPlayer.id, req.params.id]);

                    // Get person from player id
                    const playerPerson = await this.db.oneOrNone("SELECT * FROM persons WHERE player_id = $1", [performingPlayer.id]);

                    // Get email of the contact person
                    const contactPerson = await this.db.oneOrNone("SELECT * FROM persons WHERE id = $1", [job.contact_person_id]);

                    // Create email
                    let email = await this.db.one("INSERT INTO emails (sender_person_id, receiver_person_id, job_id, public_id, subject, body) VALUES ($1, $2, $3, $4, $5, $6) RETURNING public_id, sender_person_id, job_id, subject, body", [
                        job.contact_person_id,
                        playerPerson.id,
                        job.id,
                        nanoid(21),
                        "Job details",
                        `Hello {{PlayerName}},\n\nThis is an automated message from Hack4Hire, You accepted to do a job for {{ContactPersonName}}. The information about the job is stated below. Please complete the job before the deadline.\n\nReply to this email when you finished the job.\nGood Luck!`,
                    ]);

                    email.sender_person = contactPerson;
                    email.receiver_person = playerPerson;
                    email.job_id = job.public_id;

                    delete email.sender_person_id;
                    delete email.receiver_person_id;

                    // Send websocket event to the player that he accepted the job
                    this.io.to(performingPlayer.id).emit("EmailReceived", email);

                    return res.send();
                }

                return res.status(403).json({ errors: [{msg: "You are not allowed to update this job"}] });
            }

            // Update the job
            await this.db.none("UPDATE jobs SET performing_player_id = $1, completed = $2, deadline = $3, data = $4, payout_amount = $5, min_level = $6, min_faction_reputation = $7 WHERE public_id = $8", [req.body.performing_player_id, req.body.completed, req.body.deadline, req.body.data, req.body.payout_amount, req.body.min_level, req.body.min_faction_reputation, req.params.id]);

            res.send();
        }
    ]};

    protected Delete(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsAdmin,
        async (req: Request, res: Response) => {
            // Delete the job
            await this.db.none("DELETE FROM jobs WHERE public_id = $1", [req.params.id]);
            res.send();
        }
    ]};
}