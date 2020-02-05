import requests, os
import json
class Network:
    @staticmethod
    def _api(lampadaId, status):
        url = "http://soalv3dfgc01:8080/hack/api/lampada/%i/status" % lampadaId
        payload = {
            "status": status,
            "idEquipe": os.environ["ID_EQUIPE"]
        }
        headers = {
        'Content-Type': 'application/json'
        }
        response = requests.request("PUT", url, headers = headers, data = json.dumps(payload))
        print(response.text.encode('utf8'))

    @staticmethod
    def atualizaStatus(lampadaId, status):
        Network._api(lampadaId, status)