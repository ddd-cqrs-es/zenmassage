// Typescript definitions for PebbleKit v3.0 plugin for Cordova.

interface Plugins {
    Pebble: Pebble
}

interface Pebble {
    setAppUUID(uuid: string, success: (event: any) => any, failure: (event: any) => any): void;

    onConnect(success: (event: any) => any, failure: (event: any) => any): void;

    launchApp(success: (event: any) => any, failure: (event: any) => any): void;

    killApp(success: (event: any) => any, failure: (event: any) => any): void;

    sendAppMessage(message: any, success: (event: any) => any, failure: (event: any) => any): void;

    onAppMessageReceived(success: (event: any) => any, failure: (event: any) => any): void;
}