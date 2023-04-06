import { Request, Response, RequestHandler } from "express"
import { IDatabase } from "pg-promise";
import { Server } from "socket.io";
import { IsPersonOrAdmin, IsUserOrAdmin } from "../Middlewares/Authorization";
import { BaseService } from "./@BaseService";
import { ParamsDictionary } from "express-serve-static-core";
import { ParsedQs } from "qs";

export class EmailService extends BaseService {
    constructor(db: IDatabase<{}>, io: Server) {
        super(db, io, "/emails");
    }

    // Gets all emails for the current user or all emails if the user is an admin
    protected All(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        IsPersonOrAdmin,
        async (req: Request, res: Response) => {
            if(res.locals.isAdmin) return res.send(await this.db.manyOrNone("SELECT * FROM emails"));

            let emails = await this.db.manyOrNone("SELECT * FROM emails WHERE receiver_person_id = $1", [res.locals.person.id]);
            for(const email of emails){
                delete email.id;

                email.sender = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [email.sender_person_id]);
                delete email.sender_person_id;

                email.receiver = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [email.receiver_person_id]);
                delete email.receiver_person_id;

                if(email.job_id != null) {
                    email.job = await this.db.oneOrNone("SELECT public_id, job_type, payout_amount, min_level, min_faction_reputation, data, deadline, completed, created_at, updated_at FROM jobs WHERE id = $1", [email.job_id]);
                }
                delete email.job_id;

                if(email.prev_email_id != null) {
                    email.prev_email = await this.db.oneOrNone("SELECT public_id, subject, body, created_at, updated_at FROM emails WHERE id = $1", [email.prev_email_id]);
                }
                delete email.prev_email_id;
            }
            res.send(emails);
        }
    ]};

    protected Get(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        async (req: Request, res: Response) => {
            res.status(404);
        }
    ]};
    
    // Sends an email to a person
    protected Post(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        IsPersonOrAdmin,
        async (req: Request, res: Response) => {
            res.status(404);
            // Todo: get receiver person id either by email or by public id
            // Todo: store email in database
            // Todo: send websocket event to receiver
        }
    ]};

    // Marks an email as read or deleted
    protected Patch(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        IsPersonOrAdmin,
        async (req: Request, res: Response) => {
            res.status(404);
            // Todo: handle email been read
            // Todo: handle email been deleted
        }
    ]};

    protected Delete(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        async (req: Request, res: Response) => {
            res.status(404);
        }
    ]};
}