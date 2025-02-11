using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Add authentication services to the builder
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
    {
        // Set the metadata address for the OpenID configuration
        o.MetadataAddress = "http://localhost:8080/realms/your-realm-name/.well-known/openid-configuration";

        // Set the authority for the authentication server
        o.Authority = "http://localhost:8080/realms/your-realm-name";

        // Set the audience for the JWT token
        o.Audience = "account";

        // For testing, you might want to disable HTTPS metadata requirement
        // Set this to true in production for security
        o.RequireHttpsMetadata = false;
    });
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
