import { LampadasService } from "./services/lampadasService";
const Main = async () => {
   let lampada = await LampadasService.UpdateLampadaStatus(1, true);
   console.log(lampada.data)
   lampada = await LampadasService.UpdateLampadaStatus(1, false);
   console.log(lampada.data)
   lampada = await LampadasService.UpdateLampadaStatus(1, true);
   console.log(lampada.data)
   lampada = await LampadasService.UpdateLampadaStatus(1, false);
   console.log(lampada.data)
   lampada = await LampadasService.UpdateLampadaStatus(1, true);
   console.log(lampada.data)
}
try {
    Main()    
} catch (error) {
    console.log(error)
}

