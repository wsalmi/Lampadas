import { Task1 } from "./tasks/task.1";
import { Task2 } from "./tasks/task.2";
import { Task3 } from "./tasks/task.3";
import { Task4 } from "./tasks/task.4";
import { Task5 } from "./tasks/task.5";
import { Task6 } from "./tasks/task.6";
import { Task7 } from "./tasks/task.7";
import { Task8 } from "./tasks/task.8";
import { Sleep } from "./services/lampadasService";
const Main = async () => {
  //await Sleep(10000);
  let tasks = [
    Task1,
    Task2,
    Task3,
    Task4,
    Task5,
    Task6,
    Task7,
    Task8
  ]; 

  while (true) {
    await tasks[Math.round(Math.random() * 7)]();
  }
};

try {
  Main();
} catch (error) {
  console.log(error);
}
