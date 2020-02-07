import { UpdateLampadaStatus } from "../services/lampadasService";

export class TaskBase { 
    static readonly setupLampadas = async () => { 
        for (let index = 1; index <= 10; index++) {
            await UpdateLampadaStatus(index, false)            
        }
    }
}