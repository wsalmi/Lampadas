import { TaskBase } from "./task.base";
import { UpdateLampadaStatus, Sleep } from "../services/lampadasService";

export const Task5 = async () => {
    await TaskBase.setupLampadas();
    
    let ultimo = 1
    let penultimo = 0
    let aux = 0

    while(ultimo <= 8){
        
        await UpdateLampadaStatus(ultimo, true)
        await Sleep(500)

        aux = penultimo
        penultimo = ultimo
        ultimo = aux+ultimo
        
    }

}