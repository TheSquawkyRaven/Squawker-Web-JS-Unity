using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Collections.Concurrent;
using WebSocketSharp;
using WebSocketSharp.Server;

public class ChatReceiver
{

    private static ChatReceiver receiver;

    private const string address = "ws://127.0.0.1:8080";
    private readonly WebSocketServer server;

    public readonly ConcurrentQueue<string> messages = new ConcurrentQueue<string>();
    public readonly ConcurrentQueue<string> onConnectMessages = new ConcurrentQueue<string>();

    public ChatReceiver()
    {
        receiver = this;

        server = new WebSocketServer(address);

        server.AddWebSocketService<Reader>("/Reader");

        server.Start();


    }

    private class Reader : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            receiver.OnOpen(Context.Origin);
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            receiver.OnMessage(e.Data);
        }
        protected override void OnClose(CloseEventArgs e)
        {
            receiver.OnClose(Context.Origin, e.Reason);
        }
    }

    private void OnOpen(string origin)
    {
        onConnectMessages.Enqueue($"Connected! ({origin})");
    }
    private void OnClose(string origin, string reason)
    {
        onConnectMessages.Enqueue($"Closed! ({origin}, {reason})");
    }

    private void OnMessage(string text)
    {
        messages.Enqueue(text);
    }

    public void ApplicationClose()
    {
        server.Stop();
        
    }

}
