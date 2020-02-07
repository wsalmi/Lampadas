import { TaskBase } from "./task.base";
import { UpdateLampadaStatus, Sleep } from "../services/lampadasService";

export const Task3 = async () => {
    await TaskBase.setupLampadas();
    const size = 10;
    let loop = 8;
    while(loop < size){
        for (let index = 0; index < 5; index++) {        
            await UpdateLampadaStatus(index + 1, true)
            await UpdateLampadaStatus(size - index, true)
            await Sleep(500)
            await UpdateLampadaStatus(index + 1, false)
            await UpdateLampadaStatus(size - index, false)
            await Sleep(500)
        }
        for (let index = 6; index < size; index++) {        
            await UpdateLampadaStatus(index + 1, true)
            await UpdateLampadaStatus(size - index, true)
            await Sleep(500)
            await UpdateLampadaStatus(index + 1, false)
            await UpdateLampadaStatus(size - index, false)
            await Sleep(500)
        }
        loop += 1;
    }
}