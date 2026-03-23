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

        public void start()
        {
            _app.MapGet("/{content}", (string content) =>
            {
                return content;
            }).WithName("processar");




            _app.MapGet("/new" , (Sudoku jogo) =>
            {
                return Results.Ok(jogo.api_get_boards());
            }).WithName("new");

            _app.Run();
        }

        public API()
        {
            _builder = WebApplication.CreateBuilder(new string[0]);
            _builder.Services.AddSingleton<Sudoku>(sp =>
            {
                return new Sudoku(6, 3, 2);
            });
            _app = _builder.Build();
           
        }

    }
}