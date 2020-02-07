import { TaskBase } from "./task.base";
import { UpdateLampadaStatus, Sleep } from "../services/lampadasService";

export const Task8 = async () => {
    await TaskBase.setupLampadas();
    
    let array1 = [1,5,10,8,3]
    let array2 = [2,3,8,5,6]
    let arrayU = array2;
    let arrayI = []

    array1.map(e => {
        if(array2.find(f => f == e)) arrayI.push(e)
        else arrayU.push(e)
    })

    console.log(arrayI)
    console.log(arrayU)

    for(let index = 0; index < arrayU.length; index++){
        await UpdateLampadaStatus(arrayU[index], true)
    }
    await Sleep(3000)
    await TaskBase.setupLampadas()
    for(let index = 0; index < arrayI.length; index++){
        await UpdateLampadaStatus(arrayI[index], true)
    }

}