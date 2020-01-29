import { UpdateLampadaStatus, Sleep } from "./services/lampadasService";
const Main = async () => {
    await UpdateLampadaStatus(1, true);
    await Sleep(1000);
    await UpdateLampadaStatus(2, true);
    await Sleep(1000);
    await UpdateLampadaStatus(3, true);
    await Sleep(1000);
    await UpdateLampadaStatus(4, true);
    await Sleep(1000);
    await UpdateLampadaStatus(5, true);
    await Sleep(1000);
    await UpdateLampadaStatus(6, true);
    await Sleep(1000);
    await UpdateLampadaStatus(7, true);
    await Sleep(1000);
    await UpdateLampadaStatus(8, true);
    await Sleep(1000);
    await UpdateLampadaStatus(9, true);
    await Sleep(1000);
    await UpdateLampadaStatus(10, true);
    await Sleep(1000);
}

try {
    Main()
} catch (error) {
    console.log(error)
}

