using Geracao.Sudoku;
using MatchMaking.MatchMaker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;

namespace Web.API
{
    class API
    {
        private WebApplicationBuilder _builder;
        private WebApplication _app;


        public record MatchRequest(string name, int? elo);


        public void start()
        {
            // _app.MapPost("/{content}", (string content) =>
            // {
            //     return content;
            // }).WithName("processar");



            _app.MapGet("/sudoku" , (Sudoku s) =>
            {
                return Results.Ok(s.api_get_boards());
            }).WithName("sudoku");

            _app.MapPost("/new", (MatchRequest r, MatchMaker m) =>
            {
                if (r.name == null || r.elo == null) return Results.Ok("fail to queue");



                m.queue(r.name, (int)r.elo);
                return Results.Ok($"queued: {r.name}, {r.elo}");
            }).WithName("new");


            _app.Run();
        }

        public API()
        {
            _builder = WebApplication.CreateBuilder(new string[0]);

            _builder.Services.AddSingleton<MatchMaker>(new MatchMaker());
            _builder.Services.AddSingleton<Sudoku>(sp =>
            {
                return new Sudoku(6, 3, 2);
            });
            _app = _builder.Build();
           
        }

    }
}