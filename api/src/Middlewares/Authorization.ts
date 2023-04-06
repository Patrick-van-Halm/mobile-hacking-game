import { Request, Response, NextFunction } from 'express';
import pgPromise from 'pg-promise';

export function IsAuthorized(req: Request, res: Response, next: NextFunction) {
    if(req.headers.authorization == null) {
        res.status(401);
        res.send("Unauthorized");
        return;
    }
    next();
}

export function IsAdmin(req: Request, res: Response, next: NextFunction) {
    if(req.headers.authorization == null) {
        res.status(401);
        res.send("Unauthorized");
        return;
    }

    if(req.headers.authorization != `Bearer ${process.env.ADMIN_TOKEN}`) {
        res.status(403);
        res.send("Forbidden");
        return;
    }

    next();
}

export function IsUserOrAdmin(db: pgPromise.IDatabase<{}>) {
    return async (req: Request, res: Response, next: NextFunction) => {
        if(req.headers.authorization == null || req.headers.authorization.split(" ")[0] != "Bearer") {
            res.status(401);
            res.send("Unauthorized");
            return;
        }

        if(req.headers.authorization != `Bearer ${process.env.ADMIN_TOKEN}`) {
            let authKey = await db.oneOrNone("SELECT player_id, token, expires_at FROM auth_tokens WHERE token = $1", [req.headers.authorization.split(" ")[1]]);
            if(authKey == null) {
                res.status(403);
                res.send("Forbidden");
                return;
            }

            if(authKey.expires_at < new Date()) {
                res.status(403);
                res.send("Auth key expired");
                return;
            }

            // Update the expires_at date to 30 days from now
            authKey.expires_at = new Date(new Date().getTime() + 30 * 24 * 60 * 60 * 1000);
            await db.none("UPDATE auth_tokens SET expires_at = $1 WHERE token = $2", [authKey.expires_at, authKey.token]);

            // Get the player id from the auth key
            let player = await db.oneOrNone("SELECT id, public_id FROM players WHERE id = $1", [authKey.player_id]);
            res.locals.player = player;

            // Get the person if it exists
            let person = await db.oneOrNone("SELECT id, public_id FROM persons WHERE player_id = $1", [authKey.player_id]);
            res.locals.person = person;
        }
        else {
            res.locals.isAdmin = true
        }
        next();
    }
}

export function IsRequestedUserOrAdmin(db: pgPromise.IDatabase<{}>, idParam: string) {
    return async (req: Request, res: Response, next: NextFunction) => {
        if(req.headers.authorization == null || req.headers.authorization.split(" ")[0] != "Bearer") {
            res.status(401);
            res.send("Unauthorized");
            return;
        }

        if(req.headers.authorization != `Bearer ${process.env.ADMIN_TOKEN}`) {
            let authKey = await db.oneOrNone("SELECT player_id, token, expires_at FROM auth_tokens WHERE token = $1", [req.headers.authorization.split(" ")[1]]);
            if(authKey == null) {
                res.status(403);
                res.send("Forbidden");
                return;
            }

            let requestedPlayer = await db.oneOrNone("SELECT public_id FROM players WHERE id = $1", [authKey.player_id]);
            if(requestedPlayer.public_id != req.params[idParam]) {
                res.status(403);
                res.send("Forbidden");
                return;
            }
            res.locals.player = requestedPlayer;


            if(authKey.expires_at < new Date()) {
                res.status(403);
                res.send("Auth key expired");
                return;
            }

            // Update the expires_at date to 30 days from now
            authKey.expires_at = new Date(new Date().getTime() + 30 * 24 * 60 * 60 * 1000);
            await db.none("UPDATE auth_tokens SET expires_at = $1 WHERE token = $2", [authKey.expires_at, authKey.token]);

            // Get the person if it exists
            let person = await db.oneOrNone("SELECT id, public_id FROM persons WHERE player_id = $1", [authKey.player_id]);
            res.locals.person = person;
        }
        else {
            res.locals.isAdmin = true
        }
        next();
    }
}

export function IsPersonOrAdmin(req: Request, res: Response, next: NextFunction) {
    if(res.locals.person == null && res.locals.isAdmin == null) {
        res.status(403);
        res.send("Forbidden");
        return;
    }
    next();
}