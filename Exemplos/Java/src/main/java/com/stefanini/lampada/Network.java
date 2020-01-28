package com.stefanini.lampada;

import kong.unirest.Unirest;

public class Network {

	private final String PATH_BASE = "http://SOALV3DFGC01:8080/hack/";
	private final int ID_EQUIPE = 0;

	public Network() {}

	public LampadaPutResponseBody setLampadaStatus(Integer codLampada, boolean status) {
		String url = String.format("%s%s", PATH_BASE, "api/lampada/{codLampada}/status");
		return Unirest.put(url)
				.header("Content-Type", "application/json")
				.routeParam("codLampada", codLampada.toString())
				.body(new LampadaPutRequestBody(ID_EQUIPE, status))
				.asObject(LampadaPutResponseBody.class).getBody();
	}

	public class LampadaPutRequestBody {
		public int idEquipe;
		public boolean status;

		public LampadaPutRequestBody(int idEquipe, boolean status) {
			this.idEquipe = idEquipe;
			this.status = status;
		}
	}

	public class LampadaPutResponseBody {
		public int lampada;
		public String status;

		@Override
		public String toString() {
			return "Lampada " + lampada + " -> " + status;
		}
	}
}
