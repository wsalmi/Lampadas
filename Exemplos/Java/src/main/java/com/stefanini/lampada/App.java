package com.stefanini.lampada;

public final class App {
    private App() {}

    public static void main(String[] args) throws Exception {
        boolean repeat = java.util.Arrays.asList(args).contains("--repeat");
        int delay = Integer.parseInt(System.getenv().getOrDefault("DELAY_MS", "500"));
        var network = new Network();
        do {
            for (int lampId = 1; lampId <= 10; lampId++) {
                System.out.println(network.setLampadaStatus(lampId, false));
                Thread.sleep(Math.max(delay, 0));
                System.out.println(network.setLampadaStatus(lampId, true));
                Thread.sleep(Math.max(delay, 0));
            }
        } while (repeat);
    }
}
