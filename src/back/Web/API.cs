using Geracao.Sudoku;
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


        public async Task start()
        {
            // _app.MapPost("/{content}", (string content) =>
            // {
            //     return content;
            // }).WithName("processar");

            _app.MapGet("/sudoku" , (Sudoku s) =>
            {
                return Results.Ok(s.api_get_boards());
            }).WithName("sudoku");

            await _app.RunAsync();
        }



        public API()
        {
            _builder = WebApplication.CreateBuilder(new string[0]);

            //_builder.Services.AddSingleton<MatchMaker>(new MatchMaker());
            _builder.Services.AddSingleton<Sudoku>(sp =>
            {
                return new Sudoku(6, 3, 2);
            });
            _app = _builder.Build();
           
        }

    }
}