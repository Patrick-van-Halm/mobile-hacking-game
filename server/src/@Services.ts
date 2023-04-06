import { JobGenerator } from "./services/JobGenerator";
import { NPCGenerator } from "./services/NPCGenerator";

export function SetupServices() {
    new NPCGenerator();
    new JobGenerator();
}