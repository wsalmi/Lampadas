import { TaskBase } from "./task.base";
import { UpdateLampadaStatus, Sleep } from "../services/lampadasService";

export const Task7 = async () => {
    await TaskBase.setupLampadas();
    
    let array1 = [10,2,7,3,9,4,6,1]
    let array2 = [5,2,9,4,7]
    let array3 = [1,8]

    array1.sort(function(a,b){
        if (a < b)        
        return 1
        else if (a > b)
        return -1
        else return 0
    })

    array2.sort(function(a,b){
        if (a < b)        
        return 1
        else if (a > b)
        return -1
        else return 0
    })

    array3.sort(function(a,b){
        if (a < b)        
        return 1
        else if (a > b)
        return -1
        else return 0
    })

    for(let index = 0; index < array1.length; index++){
        await UpdateLampadaStatus(array1[index], true)
        await Sleep(500)
        await UpdateLampadaStatus(array1[index], false)
        await Sleep(500)
    }
    await Sleep(3000)
    for(let index = 0; index < array2.length; index++){
        await UpdateLampadaStatus(array2[index], true)
        await Sleep(500)
        await UpdateLampadaStatus(array2[index], false)
        await Sleep(500)
    }
    await Sleep(3000)
    for(let index = 0; index < array3.length; index++){
        await UpdateLampadaStatus(array3[index], true)
        await Sleep(500)
        await UpdateLampadaStatus(array3[index], false)
        await Sleep(500)
    }
    await Sleep(3000)
}