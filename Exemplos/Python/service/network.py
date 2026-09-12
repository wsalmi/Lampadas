from __future__ import annotations

import os
from dataclasses import dataclass
from typing import Any

import requests


@dataclass(frozen=True)
class Settings:
    api_url: str = os.getenv("LAMPADAS_API_URL", "http://localhost:3000/api").rstrip("/")
    team_code: int = int(os.getenv("COD_EQUIPE", "0"))
    timeout_seconds: float = float(os.getenv("REQUEST_TIMEOUT_SECONDS", "5"))


class LampadasApi:
    def __init__(self, settings: Settings | None = None) -> None:
        self.settings = settings or Settings()
        self.session = requests.Session()

    def atualiza_status(self, lampada_id: int, status: bool) -> dict[str, Any]:
        if lampada_id < 1:
            raise ValueError("lampada_id deve ser maior que zero")
        response = self.session.put(
            f"{self.settings.api_url}/lampada/{lampada_id}/status",
            json={"idEquipe": self.settings.team_code, "status": status},
            timeout=self.settings.timeout_seconds,
        )
        response.raise_for_status()
        return response.json()

    def close(self) -> None:
        self.session.close()
