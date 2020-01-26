import Axios from "axios"
import { Enviroment } from '../enviroment';

export class LampadasService {

    static UpdateLampadaStatus = async (codigo: number, status: boolean) => {
        return await Axios.put(`${Enviroment.BaseUrl}/lampada/${codigo}/status`, {
            "idEquipe": Enviroment.CodigoEquipe,
            "status": status
        })
    }
}