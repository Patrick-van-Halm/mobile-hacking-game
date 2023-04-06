import { BaseService } from "./@BaseService";
import axios from "axios";

export class JobGenerator extends BaseService {
    private MAX_JOBS = 10;
    private JOB_TYPES = ['UserCredentials', 'AnyCredentials'];

    constructor() {
        super(1000 * 60); // 1 minutes
    }

    protected async Start(): Promise<void> {
        if(await this.GetRandomNPC() && await this.GetRandomDevice()) 
            this.GenerateJobsIfNeeded();
    }

    protected async Update(): Promise<void> {
        if(await this.GetRandomNPC() && await this.GetRandomDevice()) 
            this.GenerateJobsIfNeeded();
    }

    private async GenerateJobsIfNeeded(): Promise<void> {
        const jobs = (await this.GetJobs()).filter(j => j.completed === false && j.performing_player_id === null);
        for(let i = jobs.length; i < this.MAX_JOBS; i++) {
            await this.GenerateJobs();
        }
    }

    private async GetJobs(): Promise<any[]> {
        try{
            return (await axios.get(`${process.env.API_URL}/jobs`, {
                headers: {
                    Authorization: `Bearer ${process.env.ADMIN_TOKEN}`
                }
            })).data;
        }
        catch(e) {
            console.error(e);
            return [];
        }
    }

    private async GenerateJobs(): Promise<void> {
        const jobType = this.JOB_TYPES[Math.floor(Math.random() * this.JOB_TYPES.length)];
        axios.post(`${process.env.API_URL}/jobs`, {
            contact_person_id: (await this.GetRandomNPC()).public_id,
            job_type: jobType,
            payout_amount: Math.floor(Math.random() * 1000),
            min_level: Math.floor(Math.random() * 10),
            min_faction_reputation: Math.floor(Math.random() * 100),
            data: JSON.stringify({
                ...(await this.GatherDataForJob(jobType))
            }),
            deadline: new Date(Date.now() + 1000 * 60 * 60 * 2).toISOString() // 2 hours
        }, {
            headers: {
                Authorization: `Bearer ${process.env.ADMIN_TOKEN}`
            }
        })
    }

    private async GatherDataForJob(jobType: string): Promise<any> {
        const targetDevice = await this.GetRandomDevice();
        switch(jobType) {
            case 'UserCredentials':
                return {
                    target_device: targetDevice.public_id,
                    username: targetDevice.data.users[Math.floor(Math.random() * targetDevice.data.users.length)].username,
                }
                break;
            case 'AnyCredentials':
                return {
                    target_device: targetDevice.public_id,
                }

            default: 
                return {};
        }
    }

    private async GetRandomNPC(): Promise<any> {
        try{
            const npcs = (await axios.get(`${process.env.API_URL}/persons`, {
                headers: {
                    Authorization: `Bearer ${process.env.ADMIN_TOKEN}`
                }
            })).data.filter((p: any) => p.player_id == null)
            return npcs[Math.floor(Math.random() * npcs.length)];
        }
        catch(e) {
            console.error(e);
            return null;
        }
    }

    private async GetRandomDevice(): Promise<any> {
        try {
            const devices = (await axios.get(`${process.env.API_URL}/devices`, {
                headers: {
                    Authorization: `Bearer ${process.env.ADMIN_TOKEN}`
                }
            })).data;
            return devices[Math.floor(Math.random() * devices.length)];
        }
        catch(e) { 
            console.error(e);
            return null;
        }
    }
}