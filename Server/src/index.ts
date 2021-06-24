// Dependencies
import express from 'express';
import WebSocket from 'ws';

const SocketServer = require('ws').Server;

const port: number = 3000;
const server = express().listen(port);

const wss = new SocketServer({ server });

console.log("[server] WebSocket server has started up, port: %s", port)

wss.on('connection', (ws: any) => {
    console.log('[server] Client is connected.');

    ws.on('close', () => {
        console.log('[server] Client disconnected.')
    });

    ws.on('message', (message: string) => {
        console.log('[server] Received message');
        wss.clients.forEach(function each(client: any){
            if(ws !== client && client.readyState === WebSocket.OPEN){ 
                client.send(message);
            }
        });
    });
});

function GetJSON(jsonStr: string){
    try{
        return JSON.parse(jsonStr);
    }
    catch(error){
        console.log(error);
        return false;
    }
}