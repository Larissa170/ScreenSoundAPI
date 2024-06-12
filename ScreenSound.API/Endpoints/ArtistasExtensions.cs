using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Responses;
using ScreenSound.Banco;
using ScreenSound.Modelos.Modelos;

namespace ScreenSound.API.Endpoints;

public static class ArtistasExtensions
{
	public static void AddEndPointsArtistas(this WebApplication app)
	{
		
		app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
		{
			return EntityListToResponseList(dal.Listar());
		});

		//buscar artistas por nome
		app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
		{
			var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
			if (artista is null)
			{
				return Results.NotFound();
			}
			return EntityToResponse(artista);
		});

		app.MapPost("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequest artistaRequest) =>
		{
			var artista = new Artista(artistaRequest.nome, artistaRequest.bio);
			dal.Adicionar(artista);
			return Results.Ok();
		});

		app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
		{
			var artista = dal.RecuperarPor(a => a.Id == id);
			if (artista is null)
			{
				return Results.NotFound();
			}
			dal.Deletar(artista);
			return Results.NoContent();
		});

		app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequestEdit artistaRequest) =>
		{
			var artistaAtualizar = dal.RecuperarPor(a => a.Id == artistaRequest.Id);
			if (artistaAtualizar is null)
			{
				return Results.NotFound();
			}
			artistaAtualizar.Nome = artistaRequest.nome;
			artistaAtualizar.Bio = artistaRequest.bio;
			//artistaAtualizar.FotoPerfil = artistaRequest.FotoPerfil;

			dal.Atualizar(artistaAtualizar);
			return Results.Ok();
		});
	}
	private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
	{
		return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
	}

	private static ArtistaResponse EntityToResponse(Artista artista)
	{
		return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
	}

}
