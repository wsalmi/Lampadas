package com.stefanini.lampada;

public class App {
	public static final Network network = new Network();

	public static void lampadaStatus(int codLampada, boolean status) {
		System.out.println(network.setLampadaStatus(codLampada, status));
	}

	public static void main(String[] args) throws InterruptedException {

		int delay = 500;
		while (true) {
			lampadaStatus(1, false);
			Thread.sleep(delay);
			lampadaStatus(1, true);
			Thread.sleep(delay);
			lampadaStatus(2, false);
			Thread.sleep(delay);
			lampadaStatus(2, true);
			Thread.sleep(delay);
			lampadaStatus(3, false);
			Thread.sleep(delay);
			lampadaStatus(3, true);
			Thread.sleep(delay);
			lampadaStatus(4, false);
			Thread.sleep(delay);
			lampadaStatus(4, true);
			Thread.sleep(delay);
			lampadaStatus(5, false);
			Thread.sleep(delay);
			lampadaStatus(5, true);
			Thread.sleep(delay);
			lampadaStatus(6, false);
			Thread.sleep(delay);
			lampadaStatus(6, true);
			Thread.sleep(delay);
			lampadaStatus(7, false);
			Thread.sleep(delay);
			lampadaStatus(7, true);
			Thread.sleep(delay);
			lampadaStatus(8, false);
			Thread.sleep(delay);
			lampadaStatus(8, true);
			Thread.sleep(delay);
			lampadaStatus(9, false);
			Thread.sleep(delay);
			lampadaStatus(9, true);
			Thread.sleep(delay);
			lampadaStatus(10, false);
			Thread.sleep(delay);
			lampadaStatus(10, true);
			Thread.sleep(delay);
		}
	}
}
