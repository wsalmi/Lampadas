package com.stefanini.lampada;

import static org.junit.jupiter.api.Assertions.assertTrue;

import org.junit.jupiter.api.Test;

class AppTest {
    @Test
    void smokeTest() {
        assertTrue(System.getProperty("java.version").length() > 0);
    }
}
