import './env';
import express, { NextFunction, Request, Response } from 'express';
import { createServer } from 'http';
import { Server } from 'socket.io';
import { SetupServices } from './@Services';

const app = express();
const httpServer = createServer(app);
const port = process.env.WEBPORT || 3000;
const io = new Server(httpServer, { });

app.use(express.json());
app.use((req: Request, res: Response, next: NextFunction) => {
    console.log(`${req.headers['user-agent']} ${req.method} ${req.path}`);
    next();
});

SetupServices(app, io);

httpServer.listen(port, () => {
    console.log(`Server listening on port ${port}`);
});

//io.listen(httpServer);