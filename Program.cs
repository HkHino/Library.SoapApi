using Library.SoapApi.db;
using Microsoft.EntityFrameworkCore;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=db/library.db"));

builder.Services.AddSoapCore();
builder.Services.AddSingleton<ILibrarySoapService, LibrarySoapService>();

var app = builder.Build();

// Required for routing
app.UseRouting();

// Map SOAP endpoint correctly
app.UseEndpoints(endpoints =>
{
    _ = endpoints.UseSoapEndpoint<ILibrarySoapService>(
        "/soap",
        new SoapEncoderOptions(),
        SoapSerializer.DataContractSerializer
    );
});

app.Run();