var builder = WebApplication.CreateBuilder(args);

const string POLICY = "Policy";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddPolicy(POLICY, builder =>
    builder.WithOrigins("http://localhost:5173")
           .AllowCredentials()
           .AllowAnyHeader()
           .AllowAnyMethod()
           .WithExposedHeaders("WWW-Authenticate"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(POLICY);
app.UseAuthorization();
app.MapControllers();
app.Run();