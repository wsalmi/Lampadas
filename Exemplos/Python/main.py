from __future__ import annotations

import argparse
import time

from service.network import LampadasApi


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Executa uma sequência de lâmpadas no painel.")
    parser.add_argument("--lampada", type=int, default=1, help="lâmpada usada no desafio (padrão: 1)")
    parser.add_argument("--delay", type=float, default=1.0, help="intervalo entre estados em segundos")
    parser.add_argument("--once", action="store_true", help="executa uma única rodada (padrão)")
    parser.add_argument("--repeat", action="store_true", help="repete até Ctrl+C")
    return parser.parse_args()


def main() -> None:
    args = parse_args()
    api = LampadasApi()
    try:
        while True:
            print(api.atualiza_status(args.lampada, True))
            time.sleep(max(0, args.delay))
            print(api.atualiza_status(args.lampada, False))
            if not args.repeat:
                break
    finally:
        api.close()


if __name__ == "__main__":
    main()
