var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
Console.WriteLine("\n  ____                _         _           ____  ___  _   \r\n |  _ \\ ___  __ _  __| |_   _  | |_ ___    / ___|/ _ \\| |  \r\n | |_) / _ \\/ _` |/ _` | | | | | __/ _ \\  | |  _| | | | |  \r\n |  _ <  __/ (_| | (_| | |_| | | || (_) | | |_| | |_| |_|  \r\n |_| \\_\\___|\\__,_|\\__,_|\\__, |  \\__\\___/   \\____|\\___/(_)  \r\n                        |___/                              ");
Console.WriteLine("\n");

app.UseAuthorization();

app.MapControllers();

app.Run();
