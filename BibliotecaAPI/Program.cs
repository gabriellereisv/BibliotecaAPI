using BibliotecaAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Realiza a leitura da conex�o com o banco
builder.Services.AddSingleton<LivroRepository>(
    provider =>
    new LivroRepository(builder.Configuration
    .GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<UsuarioRepository>(
    provider =>
    new UsuarioRepository(builder.Configuration
    .GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<EmprestimoRepository>(
    provider =>
    new EmprestimoRepository(builder.Configuration
    .GetConnectionString("DefaultConnection")));


//Swagger Parte 1
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});


var app = builder.Build();

//Swagger Parte 2
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Crud Biblioteca V1");
        c.RoutePrefix = string.Empty;
    });
}


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();