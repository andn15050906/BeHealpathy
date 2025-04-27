var builder = WebApplication.CreateBuilder(args);

const string POLICY = "Policy";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddPolicy(POLICY, builder =>
    builder.WithOrigins(
            [
                "http://localhost:5173",
                "https://healpathy.netlify.app",
                "https://healpathy-gdftexbcfga9g5cs.southeastasia-01.azurewebsites.net"
            ]
        )
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("WWW-Authenticate"))
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors(POLICY);
app.UseAuthorization();
app.MapControllers();
app.Run();