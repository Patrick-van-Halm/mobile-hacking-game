import { RequestHandler, Router } from "express";
import { IDatabase } from "pg-promise";
import { Server } from "socket.io";

export abstract class BaseService {
    protected db: IDatabase<{}>;
    protected io: Server;
    public router: Router;

    constructor(db: IDatabase<{}>, io: Server, baseRoute: string) {
        this.db = db;
        this.io = io;
        this.router = Router();

        this.setupRoutes(baseRoute);
    }

    private setupRoutes(baseRoute: string): void {
        this.router.get(baseRoute, ...this.All());
        this.router.get(`${baseRoute}/:id`, ...this.Get());
        this.router.post(`${baseRoute}`, ...this.Post());
        this.router.patch(`${baseRoute}/:id`, ...this.Patch());
        this.router.delete(`${baseRoute}/:id`, ...this.Delete());
    }

    protected abstract All(): RequestHandler[];
    protected abstract Get() : RequestHandler[];
    protected abstract Post() : RequestHandler[];
    protected abstract Patch() : RequestHandler[];
    protected abstract Delete() : RequestHandler[];
}