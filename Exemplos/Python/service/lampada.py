import requests
import json
class Lampada:
    @staticmethod
    def _api(lampadaId, status):
        url = "http://soalv3dfgc01:8080/hack/api/lampada/%i/status" % lampadaId
        payload = {
            "status": status,
            "idEquipe": 1
        }
        headers = {
        'Content-Type': 'application/json'
        }
        response = requests.request("PUT", url, headers = headers, data = json.dumps(payload))
        print(response.text.encode('utf8'))

    @staticmethod
    def acender(lampadaId):
        Lampada._api(lampadaId, True)

    @staticmethod
    def apagar(lampadaId):
        Lampada._api(lampadaId, False)


