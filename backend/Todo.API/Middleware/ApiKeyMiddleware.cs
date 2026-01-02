namespace Todo.API.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "X-Api-Key"; // Nombre del header

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        var path = context.Request.Path.Value;
        if (path != null && (path.Contains("openapi") || path.Contains("swagger")))
        {
            await _next(context);
            return;
        }


        // 1. Verificar si el header existe
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key no proporcionada.");
            return;
        }

        // 2. Obtener la clave configurada en appsettings
        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = appSettings.GetValue<string>("ApiKeySettings:ApiKey");

        // 3. Comparar
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key invalida.");
            return;
        }

        // Si todo está bien, continuar al siguiente middleware (o controlador)
        await _next(context);
    }
}