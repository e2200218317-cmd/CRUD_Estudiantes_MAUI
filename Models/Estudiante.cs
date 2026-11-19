using SQLite;

namespace CRUD_Estudiantes_MAUI.Models;

public class Estudiante
{
    [PrimaryKey, AutoIncrement]
    public int Codigo { get; set; }

    [NotNull]
    public string Nombre { get; set; } = string.Empty;

    [NotNull]
    public string Apellido { get; set; } = string.Empty;

    public int Edad { get; set; }
}
