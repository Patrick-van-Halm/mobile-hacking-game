import { Application } from "express";
import { Server } from "socket.io";
import { db } from './database';
import { PlayerService } from "./services/PlayerService";
import { DeviceService } from "./services/DeviceService";
import { PersonService } from "./services/PersonService";
import { EmailService } from "./services/EmailService";
import { TransactionService } from "./services/TransactionService";
import { JobService } from "./services/JobService";

export function SetupServices(app: Application, io: Server) {
    io.on("connection", async (socket) => {
        // Get the player id using the auth token
        let token = socket.handshake.query.token;
        if(token == null){
            socket.disconnect();
            return;
        }

        // Get the player id using the auth token
        const data = await db.oneOrNone("SELECT * FROM auth_tokens WHERE token = $1", [token]);
        if(data == null){
            socket.disconnect();
            return;
        }

        // Join the player id room
        console.log("Player connected: " + data.player_id);
        socket.join(data.player_id);
    });

    app.use(new PlayerService(db, io).router);
    app.use(new DeviceService(db, io).router);
    app.use(new PersonService(db, io).router);
    app.use(new EmailService(db, io).router);
    app.use(new TransactionService(db, io).router);
    app.use(new JobService(db, io).router);
}