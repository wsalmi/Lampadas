package com.stefanini.lampada;

import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Duration;

public final class Network {
    private final HttpClient http;
    private final String apiUrl;
    private final int teamCode;

    public Network() {
        this.apiUrl = System.getenv().getOrDefault("LAMPADAS_API_URL", "http://localhost:3000/api").replaceAll("/+$", "");
        this.teamCode = Integer.parseInt(System.getenv().getOrDefault("COD_EQUIPE", "0"));
        this.http = HttpClient.newBuilder().connectTimeout(Duration.ofSeconds(5)).build();
    }

    public String setLampadaStatus(int lampId, boolean status) throws IOException, InterruptedException {
        var body = String.format("{\"idEquipe\":%d,\"status\":%s}", teamCode, status);
        var request = HttpRequest.newBuilder()
                .uri(URI.create(apiUrl + "/lampada/" + lampId + "/status"))
                .timeout(Duration.ofSeconds(5))
                .header("Content-Type", "application/json")
                .PUT(HttpRequest.BodyPublishers.ofString(body))
                .build();
        var response = http.send(request, HttpResponse.BodyHandlers.ofString());
        if (response.statusCode() < 200 || response.statusCode() >= 300) {
            throw new IOException("API retornou HTTP " + response.statusCode() + ": " + response.body());
        }
        return response.body();
    }
}
