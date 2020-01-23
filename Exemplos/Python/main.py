from service.lampada import Lampada
import time

def lampadaStatus(idLampada, status):
    if status:
        Lampada.acender(idLampada)
    else:
        Lampada.apagar(idLampada)

def main():
    for i in range(1,11):
        if i % 2 == 0:
            time.sleep(1)
            lampadaStatus(i, True)
main()