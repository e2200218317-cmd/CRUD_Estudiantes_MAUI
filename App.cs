namespace CRUD_Estudiantes_MAUI;

public class App : Application
{
    public App(MainPage page)
    {
        MainPage = new NavigationPage(page) { BarBackgroundColor = Color.FromArgb("#1456A0"), BarTextColor = Colors.White };
    }
}
