import Axios from "axios"
import { Enviroment } from '../enviroment';


export const UpdateLampadaStatus = async (codigo: number, status: boolean) => {
    const response = await Axios.put(`${Enviroment.BaseUrl}/lampada/${codigo}/status`, {
        "idEquipe": Enviroment.CodigoEquipe,
        "status": status
    })

    console.log(response.data);

    return response.data;
}

export const Sleep = (timeout, callback = null) => {
    return new Promise((resolve) => {
        setTimeout(() => {
            if (callback) callback();
            resolve();
        }, timeout);
    });
}
