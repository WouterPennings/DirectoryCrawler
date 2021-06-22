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
        const json = GetJSON(message);
        console.log('[server] Received message, ClientId: %s', json.ClientId);
        if(json !== false){
            wss.clients.forEach(function each(client: any){
                if(client.readyState === WebSocket.OPEN){
                    if(json.hasOwnProperty('files')) client.send(json.files);
                    else client.send('{"error":"Key: files, was missing."}');
                }
            });
        }
        else ws.send('{"error":"JSON was invalid."}');
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