using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;


namespace Sockets.WebSocketServer;
public class WebSocketServer
{
    
    private static ConcurrentDictionary<string, WebSocket> _clients = new();


    static async Task MessageClientAsync(string message, WebSocket webSocket)
    {
        try
        {
            byte[] response = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        catch
        {

        }
    }

    static async Task HandleClientAsync(string id, WebSocket webSocket)
    {
        var buffer = new byte[1024];
        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close) break;


                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"{id}: {message}");

                if (message == "jogar")
                {
                    await MessageClientAsync("jogará.", webSocket);
                }
                else
                {
                    await MessageClientAsync(message, webSocket);
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\nerro com '{id}': {ex.Message}\n\n");
        }
        finally
        {
            _clients.TryRemove(id, out _);
            if (webSocket.State != WebSocketState.Aborted)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
        }
    }


    public static async Task start()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");
        listener.Start();
        
        Console.WriteLine("Servidor ligado");


        while (true)
        {
            var context = await listener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                var wsContext = await context.AcceptWebSocketAsync(null);
                string clientID = Guid.NewGuid().ToString();

                Console.WriteLine($"novo cliente: {clientID}\n");
                _clients.TryAdd(clientID, wsContext.WebSocket);
                _ = Task.Run(() => HandleClientAsync(clientID, wsContext.WebSocket));
                await MessageClientAsync($"voce é {clientID}", wsContext.WebSocket);
            }
        }
    }

}