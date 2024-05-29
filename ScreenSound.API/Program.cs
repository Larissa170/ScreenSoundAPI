using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>(); // para nao repetir var dal = new DAL<Artista>(new ScreenSoundContext());
builder.Services.AddTransient<DAL<Musica>>();

//Desserializa��o  
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.AddEndPointsArtistas();
app.AddEndPointsMusicas();

app.Run();
