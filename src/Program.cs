
using System;
using System.Diagnostics;

using Sockets.WebSocketServer;
using Web.API;
using Geracao.Sudoku;
using System.Threading.Tasks;


class Program
{
    
    static async Task Main()
    {

        API api = new API();

        Task apiTask = api.start();
        Task wsTask = WebSocketServer.start();

        await Task.WhenAll(apiTask, wsTask);
    }
}