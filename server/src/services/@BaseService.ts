export abstract class BaseService {
    constructor(interval: number = 100) {
        this.Start();
        if(interval < 0) return;
        setInterval(() => {
            try{ this.Update() } 
            catch(e) { console.error(e) }
        }, interval);
    }

    /**
     * Update the service every interval
     */
    protected abstract Update(): void;

    /**
     * Start the service
     */
    protected abstract Start(): void;
}