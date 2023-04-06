import { Request, Response, RequestHandler } from "express"
import { ParamsDictionary } from "express-serve-static-core";
import { ParsedQs } from "qs";
import { nanoid } from 'nanoid';
import { body, validationResult } from 'express-validator';
import { IDatabase } from "pg-promise";
import { Server } from "socket.io";
import { IsAdmin, IsPersonOrAdmin, IsUserOrAdmin } from "../Middlewares/Authorization";
import { BaseService } from "./@BaseService";
import { ValidatorShowErrors } from "../Middlewares/Validator";

export class DeviceService extends BaseService {
    constructor(db: IDatabase<{}>, io: Server) {
        super(db, io, "/devices");
    }

    protected All(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsAdmin,
        async (req: Request, res: Response) => {
            res.send(await this.db.manyOrNone("SELECT public_id, name, ip, data, created_at, updated_at FROM devices"));
        }
    ]};

    protected Get(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        IsPersonOrAdmin,
        async (req: Request, res: Response) => {
            // Get the device from the database
            const device = await this.db.oneOrNone("SELECT public_id, person_id, name, ip, data, created_at, updated_at FROM devices WHERE public_id = $1", [req.params.id]);
            if(!device) return res.status(404).json({ errors: [{msg: "Device not found"}] });

            // Get the person
            device.person = await this.db.oneOrNone("SELECT public_id, name, faction, email_address, created_at, updated_at FROM persons WHERE id = $1", [device.person_id]);
            delete device.person_id;

            res.send(device);
        }
    ]};
    
    protected Post(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        body("person_id")
            .isLength({min: 21, max: 21})
            .custom(async (value) => (await this.db.oneOrNone("SELECT public_id FROM persons WHERE public_id = $1", [value])) != null ? Promise.resolve() : Promise.reject("Person does not exist")),
        body("name")
            .isLength({min: 1, max: 255})
            .isAscii(),
        body("ip")
            .isIP(),
        body("data")
            .isJSON(),
        IsUserOrAdmin(this.db),
        ValidatorShowErrors,
        async (req: Request, res: Response) => {
            // Check user is admin or owner of person
            let person = await this.db.oneOrNone("SELECT id FROM persons WHERE public_id = $1", [req.body.person_id]);
            let player = await this.db.oneOrNone("SELECT id FROM players WHERE id = $1", [person.player_id]);
            if(!res.locals.isAdmin && player.id != res.locals.player.id) {
                return res.status(403).json({ errors: [{msg: "You are not authorized to do this"}] });
            }

            let ip = req.body.ip ?? (Math.floor(Math.random() * 255) + 1)+"."+(Math.floor(Math.random() * 255))+"."+(Math.floor(Math.random() * 255))+"."+(Math.floor(Math.random() * 255));
            let data = req.body.data ?? {};
            let device = await this.db.oneOrNone("INSERT INTO devices (public_id, owner_person_id, name, ip, data) VALUES ($1, $2, $3, $4, $5) RETURNING public_id, name, ip, data, created_at, updated_at", [nanoid(), person.id, req.body.name, ip, data]);
            delete person.id;
            device.owner_person = person;
            res.send(device);
        }
    ]};

    protected Patch(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        async (req: Request, res: Response) => {
            
        }
    ]};

    protected Delete(): RequestHandler<ParamsDictionary, any, any, ParsedQs, Record<string, any>>[] { return [
        IsUserOrAdmin(this.db),
        async (req: Request, res: Response) => {

        }
    ]};
}