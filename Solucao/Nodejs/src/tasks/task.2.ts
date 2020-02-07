import { UpdateLampadaStatus, Sleep } from "../services/lampadasService";
import { TaskBase } from "./task.base";

export const Task2 = async () => {
    await TaskBase.setupLampadas()
    for (let index = 1; index <= 10; index++) {
        if (index % 2 == 0)
            await UpdateLampadaStatus(index, true)        
   }
   await Sleep(3000)
   for (let index = 1; index <= 10; index++) {
        await UpdateLampadaStatus(index, true)        
    }
    await Sleep(1000)
}