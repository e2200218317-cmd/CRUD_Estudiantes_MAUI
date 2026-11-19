using CRUD_Estudiantes_MAUI.Models;
using SQLite;

namespace CRUD_Estudiantes_MAUI.Data;

public class EstudianteDatabase
{
    private SQLiteAsyncConnection? _database;

    private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_database is not null) return _database;
        string path = Path.Combine(FileSystem.AppDataDirectory, "estudiantes.db3");
        var connection = new SQLiteAsyncConnection(path);
        await connection.CreateTableAsync<Estudiante>();
        _database = connection;
        return connection;
    }

    public async Task<List<Estudiante>> ListarAsync() =>
        await (await GetDatabaseAsync()).Table<Estudiante>().OrderBy(e => e.Codigo).ToListAsync();

    public async Task<int> GuardarAsync(Estudiante estudiante)
    {
        var db = await GetDatabaseAsync();
        return estudiante.Codigo == 0
            ? await db.InsertAsync(estudiante)
            : await db.UpdateAsync(estudiante);
    }

    public async Task<int> EliminarAsync(Estudiante estudiante) =>
        await (await GetDatabaseAsync()).DeleteAsync(estudiante);
}
