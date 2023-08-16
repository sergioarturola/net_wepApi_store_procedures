var builder = WebApplication.CreateBuilder(args);

// configurando las politicas de Cors para que nuestra API pueda ser ejecutada
//desde cualquier sitio y no este restringida
var miReglaCors = "ReglaCors";//le damos un nombre para que pueda ser identificado

//ahora lo configuramos permitiendo cualquier origen, cabecera y metodo hhtp
builder.Services.AddCors(option =>
    option.AddPolicy(
        name: miReglaCors,
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
        )
    );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//llamando a las reglas
app.UseCors(miReglaCors);
app.UseAuthorization();

app.MapControllers();

app.Run();
