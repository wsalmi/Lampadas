import { TaskBase } from "./task.base";
import { UpdateLampadaStatus, Sleep } from "../services/lampadasService";

export const Task6 = async () => {
    await TaskBase.setupLampadas();
    
    let aux = 0

    for(let index = 1; index < 10; index++){
        aux = 0
        for(let y = 1; y < index; y++){
            if(index % y == 0){
                if(index != y && y != 1){
                    aux++
                }
            }
        }

        if(aux == 0){
            await UpdateLampadaStatus(index, true)
            await Sleep(500)
        }

    }

}