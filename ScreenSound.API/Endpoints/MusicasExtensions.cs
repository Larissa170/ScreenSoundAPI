using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Responses;
using ScreenSound.Banco;
using ScreenSound.Modelos.Modelos;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{
	public static void AddEndPointsMusicas(this WebApplication app)
	{
		app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
		{
			return EntityListToResponseList(dal.Listar());
		});

		app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> DAL, string nome) =>
		{
			var musica = DAL.RecuperarPor(m => m.Nome.ToUpper().Equals(nome.ToUpper()));
			if (musica is null)
			{
				return Results.NotFound();
			}
            return Results.Ok(EntityToResponse(musica));
		});

		app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal, [FromServices] DAL<Genero> dalGenero,[FromBody] MusicaRequest musicaRequest) => {
			var musica = new Musica(musicaRequest.nome)
			{
				AnoLancamento = musicaRequest.anoLancamento,
				ArtistaId = musicaRequest.ArtistaId,
				Generos = musicaRequest.generos is not null ? GeneroRequestConverter(musicaRequest.generos,dalGenero) : new List<Genero>()
			};
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

		app.MapPut("/Musicas", ([FromServicesAttribute] DAL<Musica> dal, [FromBody] MusicaRequestEdit musicaRequest) =>
		{
			var musicaAtualizar = dal.RecuperarPor(m => m.Id == musicaRequest.Id);
			if (musicaAtualizar is null)
			{
				return Results.NotFound();
			}
			musicaAtualizar.Nome = musicaRequest.nome;
			musicaAtualizar.ArtistaId = musicaRequest.ArtistaId;
			musicaAtualizar.AnoLancamento = musicaRequest.anoLancamento;

			dal.Atualizar(musicaAtualizar);
			return Results.Ok();
		});
	}

    private static ICollection<Genero> GeneroRequestConverter(ICollection<GeneroRequest> generos,DAL<Genero>dalGenero)
    {
        var lista = new List<Genero>();
		foreach (var item in generos)
		{
			var entity = RequestToEntity(item);
			var genero = dalGenero.RecuperarPor(g => g.Nome.ToUpper().Equals(item.nome.ToUpper()));
			if(genero is not null)
			{
				lista.Add(genero);
			}else
			{
				lista.Add(entity);
			}
		}
		return lista;
    }

    private static Genero RequestToEntity(GeneroRequest genero)
    {
		return new Genero()
		{
			Nome = genero.nome,
			Descricao = genero.descricao,

		};
    }

    private static ICollection<MusicaResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
	{
		return musicaList.Select(a => EntityToResponse(a)).ToList();
	}

	private static MusicaResponse EntityToResponse(Musica musica)
	{
		return new MusicaResponse(musica.Id, musica.Nome!, musica.Artista!.Id, musica.Artista.Nome);
	}


}
