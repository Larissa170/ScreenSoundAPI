namespace ScreenSound.API.Requests;

public record MusicaRequestEdit(int Id,string nome, int anoLancamento, int ArtistaId) : MusicaRequest(nome,ArtistaId,anoLancamento);
