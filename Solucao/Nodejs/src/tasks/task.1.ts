import { UpdateLampadaStatus, Sleep } from "../services/lampadasService";
import { TaskBase } from "./task.base";

export const Task1 = async () => { 
    await TaskBase.setupLampadas()
    for (let index = 1; index <= 10; index++) {
         await UpdateLampadaStatus(index, true)
         await Sleep(500)
         await UpdateLampadaStatus(index, false)
         await Sleep(500)
    }
}