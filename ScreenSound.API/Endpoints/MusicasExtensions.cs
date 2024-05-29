using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos.Modelos;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{
	public static void AddEndPointsMusicas(this WebApplication app)
	{
		app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
		{
			return Results.Ok(dal.Listar());
		});

		app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> DAL, string nome) =>
		{
			var musica = DAL.RecuperarPor(m => m.Nome.ToUpper().Equals(nome.ToUpper()));
			if (musica is null)
			{
				return Results.NotFound();
			}
			return Results.Ok(musica);
		});

		app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] Musica musica) => {
			dal.Adicionar(musica);
			return Results.Ok();
		});
		app.MapDelete("/Musicas/{id}", ([FromServices] DAL<Musica> dal, int id) =>
		{
			var musica = dal.RecuperarPor(m => m.Id == id);
			if (musica is null)
			{
				return Results.NotFound();
			}
			dal.Deletar(musica);
			return Results.NoContent();
		});

		app.MapPut("/Musicas", ([FromServicesAttribute] DAL<Musica> dal, [FromBody] Musica musica) =>
		{
			var musicaAtualizar = dal.RecuperarPor(m => m.Id == musica.Id);
			if (musicaAtualizar is null)
			{
				return Results.NotFound();
			}
			musicaAtualizar.Nome = musica.Nome;
			musicaAtualizar.Artista = musica.Artista;
			musicaAtualizar.AnoLancamento = musica.AnoLancamento;

			dal.Atualizar(musicaAtualizar);
			return Results.Ok();
		});
	}
}
