import { TaskBase } from "./task.base";
import { UpdateLampadaStatus, Sleep } from "../services/lampadasService";

export const Task4 = async () => {
    await TaskBase.setupLampadas();
    const mult = 10;
    const numeros = [];

    let num = Math.round(Math.random() * mult);
    while(mult > numeros.length){
        if(numeros.filter(e => e == num).length > 0){
            num = Math.round(Math.random() * mult)
            if(num == 0) num += 1
        }else{
            await UpdateLampadaStatus(num, true)
            await Sleep(1000)
            await UpdateLampadaStatus(num, false)
            numeros.push(num);
        }
    }
}