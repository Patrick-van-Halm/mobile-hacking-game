import { Request, Response, RequestHandler } from "express"
import { nanoid } from 'nanoid';
import argon2 from 'argon2';
import { body } from 'express-validator';
import { IDatabase } from "pg-promise";
import { Server } from "socket.io";
import { IsAdmin, IsRequestedUserOrAdmin } from "../Middlewares/Authorization";
import { ValidatorShowErrors } from "../Middlewares/Validator";
import { BaseService } from "./@BaseService";
import { ParamsDictionary } from "express-serve-static-core";
import { ParsedQs } from "qs";

export class PlayerService extends BaseService {
    constructor(db: IDatabase<{}>, io: Server) {
        super(db, io, "/players");
        this.router.get("/players/:id/reputation", ...this.GetFactionReputation());
    }

    protected All(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsAdmin,
        async (req: Request, res: Response) => {
            res.send(await this.db.manyOrNone("SELECT id, public_id, email, level, created_at, updated_at FROM players"));
        }
    ]};

    protected Get(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsRequestedUserOrAdmin(this.db, "id"),
        async (req: Request, res: Response) => {
            const player = await this.db.oneOrNone("SELECT public_id, email, level, created_at, updated_at FROM players WHERE public_id = $1", [req.params.id]);
            if(player == null) {
                res.status(404);
                res.send("Player not found");
                return;
            }
            res.send(player);
        }
    ]};
    
    protected Post(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        body('email')
        .isEmail().withMessage("Email is not valid")
        .custom(async (email: string) => {
            const player = await this.db.oneOrNone("SELECT * FROM players WHERE email = $1", [email]);
            if(player != null){
                return Promise.reject("Email already in use");
            }
        }),
        body('password')
        .isStrongPassword()
        .withMessage("Password is not strong enough"),
        ValidatorShowErrors,
        async (req: Request, res: Response) => {
            // Create player
            const player = await this.db.one("INSERT INTO players (public_id, email, password) VALUES ($1, $2, $3) RETURNING id, public_id, email, level, created_at, updated_at", [nanoid(), req.body.email, await argon2.hash(req.body.password)]);

            // Create auth token
            const authToken = await this.db.one("INSERT INTO auth_tokens (player_id, token, expires_at) VALUES ($1, $2, $3) RETURNING token", [player.id, nanoid(128), new Date(Date.now() + 1000 * 60 * 60 * 24 * 30)]);
            
            // Create faction reputation for each faction
            await this.db.none("INSERT INTO player_faction_reputation (player_id, faction, reputation) VALUES ($1, $2, $3), ($1, $4, $5), ($1, $6, $7)", [player.id, "Police", 0, "Hacker", 0, "Civilian", 0]);

            // Remove player id from response
            delete player.id;
            res.send({...player, token: authToken.token});
        }
    ]};

    protected Patch(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsRequestedUserOrAdmin(this.db, "id"),
        async (req: Request, res: Response) => {
            res.status(404);
            // Todo: Only allow changing password and email
        }
    ]};

    protected Delete(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsRequestedUserOrAdmin(this.db, "id"),
        async (req: Request, res: Response) => {
            res.status(404);
            // Todo: Allow deleting player data when the player wants it (GDPR)
        }
    ]};

    private GetFactionReputation(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsRequestedUserOrAdmin(this.db, "id"),
        async (req: Request, res: Response) => {
            // Get player by public id
            const player = await this.db.oneOrNone("SELECT id FROM players WHERE public_id = $1", [req.params.id]);
            if(player == null) return res.status(404).send("Player not found");

            // Get faction reputation
            res.send(await this.db.manyOrNone("SELECT faction, reputation FROM player_faction_reputation WHERE player_id = $1", [player.id]));
        }
    ]};
}