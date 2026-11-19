using CRUD_Estudiantes_MAUI.Data;
using Microsoft.Extensions.Logging;

namespace CRUD_Estudiantes_MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();
        builder.Services.AddSingleton<EstudianteDatabase>();
        builder.Services.AddSingleton<MainPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
