using nucleotidz.token.limiter;
using nucleotidz.token.limiter.configuration.Enums;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAITokenLimiter(builder.Configuration, LimiterType.FixedWindow);
builder.Services.AddAITokenLimiterFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
