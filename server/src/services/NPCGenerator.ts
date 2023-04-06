import { BaseService } from "./@BaseService";
import { faker } from "@faker-js/faker";
import axios from "axios";

export class NPCGenerator extends BaseService {
    private MAX_NPCs = 100;
    private NPCFactions = ["Police", "Hacker", "Civilian"];
    private Services = ["SSH", "FTP", "SQL", "HTTP", "Port Party", "MailMover", "CodeConnect", "GameGate", "FileForge", "TalkTime"];
    private OPEN_PORT_RATIO = .4;

    constructor() {
        super(-1);
    }

    protected async Start(): Promise<void> {
        const npcs = await this.GetNPCs();
        for(let i = npcs.length; i < this.MAX_NPCs; i++) {
            const npc = await this.GenerateNPC();
            if(!npc) continue;
            await this.GenerateDevice(npc);
        }
    }

    protected async Update(): Promise<void> {}

    private async GenerateNPC(): Promise<any> {
        try {
            return (await axios.post(`${process.env.API_URL}/persons`, {
                name: faker.name.firstName() + " " + faker.name.lastName(),
                faction: this.NPCFactions[Math.floor(Math.random() * this.NPCFactions.length)],
            }, {
                headers: {
                    Authorization: `Bearer ${process.env.ADMIN_TOKEN}`
                }
            })).data;
        }
        catch(e: any) {
            if(e.response && e.response.data)
                console.error(e.response.data);
            else
                console.error(e);
        }
    }

    private async GenerateDevice(npc: any): Promise<any> {
        try {
            let firstName = npc.name.split(" ")[0];
            let lastName = npc.name.split(" ")[1];
            let deviceUsername = faker.internet.userName(firstName, lastName);
            if(Math.random() > 0.5) {
                deviceUsername = firstName;
            }
            let deviceName = `${deviceUsername}'s Device`;
            return await axios.post(`${process.env.API_URL}/devices`, {
                person_id: npc.public_id,
                name: deviceName,
                ip: faker.internet.ip(),
                data: JSON.stringify({
                    users: this.GenerateUsers(deviceUsername),
                    files: this.GenerateFiles(),
                    services: this.GenerateServices()
                })
            }, {
                headers: {
                    Authorization: `Bearer ${process.env.ADMIN_TOKEN}`
                }
            })
        }
        catch(e: any) {
            if(e.response && e.response.data) {
                return console.error(e.response.data);
            }
            console.error(e);
        }
    }

    private GenerateUsers(deviceUsername: string): any[] {
        let users = [];
        users.push({
            username: deviceUsername,
            password: faker.internet.password(),
            isAdmin: faker.datatype.boolean(),
        })
        for(let i = 0; i < Math.floor(Math.random() * 3); i++) {
            users.push({
                username: faker.internet.userName(),
                password: faker.internet.password(),
                isAdmin: faker.datatype.boolean(),
            });
        }
        users.push({
            username: "root",
            password: faker.internet.password(),
            isAdmin: true,
        })
        return users;
    }

    private GenerateFiles(): any {
        let files: any = {
            "var": {
                "log": {
                    "auth.log": {
                        filetype: "log",
                    }
                }
            },
            "etc": {
                "passwd": {
                    filetype: "device-passwords",
                }
            }
        };

        if(Math.random() > 0.5) {
            files.var.www = {};
            files.var.www["index.html"] = {
                filetype: "text",
                content: `<h1>My Website</h1>\n<p>My name is ${faker.name.firstName()} ${faker.name.lastName()}.</p>\n<p>${faker.lorem.paragraphs(5)}</p>`,
            }
        }

        return files;
    }

    private GenerateServices(): any {
        const services = [];
        const portCount = Math.floor(Math.random() * 4) + 1;

        for(let i = 0; i < portCount; i++) {
            services.push({
                name: this.Services[Math.floor(Math.random() * this.Services.length)],
                port: Math.floor(Math.random() * 65534) + 1,
                isUPNP: faker.datatype.boolean(),
                isOpen: i < portCount * this.OPEN_PORT_RATIO ? true : faker.datatype.boolean(),
            });
        }
        return services;
    }

    private async GetNPCs(): Promise<any[]> {
        try{
            return (await axios.get(`${process.env.API_URL}/persons`, {
                headers: {
                    Authorization: `Bearer ${process.env.ADMIN_TOKEN}`
                }
            })).data.filter((p: any) => p.player_id == null);
        }
        catch(e: any) {
            if(e.response && e.response.data)
                console.error(e.response.data);
            else console.error(e);
            return [];
        }
    }
}