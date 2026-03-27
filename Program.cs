using SoapCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();
builder.Services.AddSingleton<ILibrarySoapService, LibrarySoapService>();

var app = builder.Build();

// Required for routing
app.UseRouting();

// Map SOAP endpoint correctly
app.UseEndpoints(endpoints =>
{
    endpoints.UseSoapEndpoint<ILibrarySoapService>(
        "/soap",
        new SoapEncoderOptions(),
        SoapSerializer.DataContractSerializer
    );
});

app.Run();