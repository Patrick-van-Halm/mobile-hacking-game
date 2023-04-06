import { Request, Response, RequestHandler } from "express"
import { ParamsDictionary } from "express-serve-static-core";
import { ParsedQs } from "qs";
import { nanoid } from 'nanoid';
import { body, validationResult } from 'express-validator';
import { IDatabase } from "pg-promise";
import { Server } from "socket.io";
import { IsAdmin, IsUserOrAdmin } from "../Middlewares/Authorization";
import { BaseService } from "./@BaseService";
import { ValidatorShowErrors } from "../Middlewares/Validator";

export class PersonService extends BaseService {
    constructor(db: IDatabase<{}>, io: Server) {
        super(db, io, "/persons");
    }

    protected All(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsAdmin,
        async (req: Request, res: Response) => {
            let persons = await this.db.manyOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons");
            res.send(persons);
        }
    ]};

    protected Get(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        async (req: Request, res: Response) => {
            res.status(404);
        }
    ]};
    
    protected Post(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        body("name")
            .isLength({min: 1, max: 255})
            .isAlpha("en-US", {ignore: [" ", "-", "'"]}),
        body("player_id")
            .isLength({min: 21, max: 21})
            .custom(async (value: string) => (await this.db.oneOrNone("SELECT public_id FROM players WHERE public_id = $1", [value])) != null ? Promise.resolve() : Promise.reject("Player does not exist"))
            .optional({nullable: true}),
        body("faction")
            .isIn(['Police', 'Hacker', 'Civilian'])
            .optional({nullable: true}),
        IsUserOrAdmin(this.db),
        ValidatorShowErrors,
        async (req: Request, res: Response) => {
            if(req.body.player_id && req.body.faction) return res.status(400).json({ errors: [{msg: "Player and faction cannot be set at the same time"}] });
            if(req.body.player_id) {
                // Validate authorization to create a person for this player
                if(!res.locals.isAdmin && res.locals.player.public_id != req.body.player_id){
                    return res.status(401).json({ errors: [{msg: "Unauthorized"}] });
                }

                // Check if the player already has a person
                if(res.locals.person != null){
                    return res.status(400).json({ errors: [{msg: "Player already has a person"}] });
                }

                // Generate email address
                let email_address = req.body.name.replace(" ", ".").replace("'", "").toLowerCase() + "@fluxmail.com";

                // Check if email address already exists
                if((await this.db.oneOrNone("SELECT email_address FROM persons WHERE email_address = $1", [email_address])) != null){
                    return res.status(400).json({ errors: [{msg: "Email address already exists"}] });
                }

                // Create person
                let person = await this.db.oneOrNone("INSERT INTO persons (public_id, player_id, name, email_address) VALUES ($1, $2, $3, $4) RETURNING public_id, name, email_address, created_at, updated_at", [nanoid(), res.locals.player.id, req.body.name, email_address]);
                res.send(person);
            }
            else {
                // When creating a person without a player the faction is required
                if(req.body.faction == null){
                    return res.status(400).json({ errors: [{msg: "Faction is required"}] });
                }

                // Validate if the person is an admin since only admins can create persons without a player
                if(!res.locals.isAdmin){
                    return res.status(401).json({ errors: [{msg: "Unauthorized"}] });
                }

                // Generate email address
                let email_address = req.body.name.replace(" ", ".").replace("'", "").toLowerCase() + "@fluxmail.com";

                // Check if email address already exists
                if((await this.db.oneOrNone("SELECT email_address FROM persons WHERE email_address = $1", [email_address])) != null){
                    return res.status(400).json({ errors: [{msg: "Email address already exists"}] });
                }

                // Create person
                let person = await this.db.oneOrNone("INSERT INTO persons (public_id, faction, name, email_address) VALUES ($1, $2, $3, $4) RETURNING public_id, faction, name, email_address, created_at, updated_at", [nanoid(), req.body.faction, req.body.name, email_address]);
                res.send(person);
            }
        }
    ]};

    protected Patch(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        async (req: Request, res: Response) => {
            res.status(404);
        }
    ]};

    protected Delete(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        async (req: Request, res: Response) => {
            res.status(404);
        }
    ]};
}