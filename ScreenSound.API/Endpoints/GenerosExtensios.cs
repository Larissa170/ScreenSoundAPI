using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Responses;
using ScreenSound.Banco;
using ScreenSound.Modelos.Modelos;
using System.Runtime.CompilerServices;

namespace ScreenSound.API.Endpoints;

public static class GenerosExtensios
{
    public static void AddEndpointsGeneros(this WebApplication app)
    {
        app.MapGet("/Generos", ([FromServices] DAL<Genero> dal) =>
        {
            return EntityListToResponseList(dal.Listar());
        });

        app.MapPost("/Generos", ([FromServices] DAL<Genero> dal, [FromBody] GeneroRequest generoRequest) =>
        {
            dal.Adicionar(RequestToEntity(generoRequest));
            return Results.Ok();
        });

        app.MapGet("/Generos/{nome}", ([FromServices] DAL<Genero> DAL, string nome) =>
        {
            var genero = DAL.RecuperarPor(g => g.Nome.ToUpper().Equals(nome.ToUpper()));
            if (genero is null)
            {
                return Results.NotFound();
            }
            return EntityToResponse(genero);
        });

        app.MapDelete("/Generos/{id}", ([FromServices] DAL<Genero> dal, int id) =>
        {
            var genero = dal.RecuperarPor(g => g.Id == id);
            if (genero is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(genero);
            return Results.NoContent();
        });

        app.MapPut("/Generos", ([FromServicesAttribute] DAL<Genero> dal, [FromBody] GeneroRequestEdit generoRequest) =>
        {
            var generoAtualizar = dal.RecuperarPor(m => m.Id == generoRequest.Id);
            if (generoAtualizar is null)
            {
                return Results.NotFound();
            }
            generoAtualizar.Nome = generoRequest.nome;
            generoAtualizar.Descricao = generoRequest.descricao;            

            dal.Atualizar(generoAtualizar);
            return Results.Ok();
        });

    }
    private static Genero RequestToEntity(GeneroRequest genero)
    {
        return new Genero()
        {
            Nome = genero.nome,
            Descricao = genero.descricao,

        };
    }

    private static ICollection<GeneroResponse> EntityListToResponseList(IEnumerable<Genero> generoList)
    {
        return generoList.Select(a => EntityToResponse(a)).ToList();
    }

    private static GeneroResponse EntityToResponse(Genero genero)
    {
        return new GeneroResponse(genero.Id, genero.Nome!, genero.Descricao);
    }

}


