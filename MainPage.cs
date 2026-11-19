using CRUD_Estudiantes_MAUI.Data;
using CRUD_Estudiantes_MAUI.Models;

namespace CRUD_Estudiantes_MAUI;

public class MainPage : ContentPage
{
    private readonly EstudianteDatabase _database;
    private readonly Entry _nombre = new() { Placeholder = "Nombre", MaxLength = 80 };
    private readonly Entry _apellido = new() { Placeholder = "Apellido", MaxLength = 80 };
    private readonly Entry _edad = new() { Placeholder = "Edad", Keyboard = Keyboard.Numeric, MaxLength = 3 };
    private readonly VerticalStackLayout _filas = new() { Spacing = 0 };
    private readonly Label _estado = new() { TextColor = Color.FromArgb("#315674"), FontSize = 12 };
    private Estudiante? _seleccionado;
    private bool _cargando;

    private static readonly Color Azul = Color.FromArgb("#1456A0");
    private static readonly Color Amarillo = Color.FromArgb("#FFFF00");
    private static readonly Color Borde = Color.FromArgb("#333333");

    public MainPage(EstudianteDatabase database)
    {
        _database = database;
        Title = "Estudiante";
        BackgroundColor = Colors.White;

        var formulario = new VerticalStackLayout { Spacing = 7, WidthRequest = 190 };
        formulario.Children.Add(new Label { Text = "Datos del estudiante", FontAttributes = FontAttributes.Bold, FontSize = 16, TextColor = Azul });
        formulario.Children.Add(new Label { Text = "Nombre", FontSize = 13, TextColor = Colors.Black });
        formulario.Children.Add(_nombre);
        formulario.Children.Add(new Label { Text = "Apellido", FontSize = 13, TextColor = Colors.Black });
        formulario.Children.Add(_apellido);
        formulario.Children.Add(new Label { Text = "Edad", FontSize = 13, TextColor = Colors.Black });
        formulario.Children.Add(_edad);

        var guardar = CrearBoton("Guardar", "#16814D");
        guardar.Clicked += Guardar_Clicked;
        var actualizar = CrearBoton("Actualizar", "#1456A0");
        actualizar.Clicked += Actualizar_Clicked;
        var eliminar = CrearBoton("Eliminar", "#BD2C32");
        eliminar.Clicked += Eliminar_Clicked;
        var limpiar = CrearBoton("Limpiar", "#777777");
        limpiar.Clicked += (_, _) => Limpiar();
        formulario.Children.Add(guardar);
        formulario.Children.Add(actualizar);
        formulario.Children.Add(eliminar);
        formulario.Children.Add(limpiar);
        formulario.Children.Add(_estado);

        var cabecera = CrearFila("Código", "Nombre", "Apellido", "Edad", encabezado: true);
        var tabla = new VerticalStackLayout { Spacing = 0, MinimumWidthRequest = 510 };
        tabla.Children.Add(cabecera);
        tabla.Children.Add(_filas);

        var zona = new HorizontalStackLayout { Spacing = 20, Padding = new Thickness(12, 12, 18, 12) };
        zona.Children.Add(formulario);
        zona.Children.Add(tabla);

        Content = new ScrollView
        {
            Orientation = ScrollOrientation.Both,
            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Padding = new Thickness(10, 15),
                Children =
                {
                    new Label { Text = "CRUD COMPLETO DE ESTUDIANTES", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Azul },
                    new Label { Text = "Completa los datos y selecciona una fila para editar o eliminar.", FontSize = 13, TextColor = Colors.Gray },
                    zona
                }
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarAsync();
    }

    private static Button CrearBoton(string texto, string color) => new()
    {
        Text = texto, BackgroundColor = Color.FromArgb(color), TextColor = Colors.White,
        CornerRadius = 5, HeightRequest = 41, FontSize = 14
    };

    private static View Celda(string texto, int columna, bool encabezado)
    {
        var etiqueta = new Label
        {
            Text = texto, TextColor = Colors.Black, FontSize = 13,
            VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Start,
            Padding = new Thickness(6, 7), LineBreakMode = LineBreakMode.TailTruncation
        };
        var border = new Border
        {
            Stroke = Borde, StrokeThickness = 0.5,
            BackgroundColor = encabezado ? Amarillo : Colors.White,
            Content = etiqueta
        };
        Grid.SetColumn(border, columna);
        return border;
    }

    private static Grid CrearFila(string codigo, string nombre, string apellido, string edad, bool encabezado = false)
    {
        var grid = new Grid { ColumnSpacing = 0, RowSpacing = 0, HeightRequest = 41 };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(165) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
        grid.Children.Add(Celda(codigo, 0, encabezado));
        grid.Children.Add(Celda(nombre, 1, encabezado));
        grid.Children.Add(Celda(apellido, 2, encabezado));
        grid.Children.Add(Celda(edad, 3, encabezado));
        return grid;
    }

    private async Task CargarAsync()
    {
        if (_cargando) return;
        _cargando = true;
        try
        {
            var estudiantes = await _database.ListarAsync();
            _filas.Children.Clear();
            foreach (var e in estudiantes)
            {
                var fila = CrearFila(e.Codigo.ToString(), e.Nombre, e.Apellido, e.Edad.ToString());
                var gesto = new TapGestureRecognizer();
                gesto.Tapped += (_, _) => Seleccionar(e);
                fila.GestureRecognizers.Add(gesto);
                _filas.Children.Add(fila);
            }
            _estado.Text = $"Total: {estudiantes.Count} estudiante(s)";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron cargar los estudiantes: " + ex.Message, "Aceptar");
        }
        finally { _cargando = false; }
    }

    private void Seleccionar(Estudiante e)
    {
        _seleccionado = e;
        _nombre.Text = e.Nombre;
        _apellido.Text = e.Apellido;
        _edad.Text = e.Edad.ToString();
        _estado.Text = $"Seleccionado: código {e.Codigo}";
    }

    private bool Validar(out int edad)
    {
        edad = 0;
        if (string.IsNullOrWhiteSpace(_nombre.Text) || string.IsNullOrWhiteSpace(_apellido.Text))
        {
            _ = DisplayAlert("Validación", "Ingrese nombre y apellido.", "Aceptar");
            return false;
        }
        if (!int.TryParse(_edad.Text, out edad) || edad < 1 || edad > 120)
        {
            _ = DisplayAlert("Validación", "La edad debe ser un número entre 1 y 120.", "Aceptar");
            return false;
        }
        return true;
    }

    private async void Guardar_Clicked(object? sender, EventArgs e)
    {
        if (!Validar(out var edad)) return;
        try
        {
            await _database.GuardarAsync(new Estudiante
            {
                Nombre = _nombre.Text!.Trim(), Apellido = _apellido.Text!.Trim(), Edad = edad
            });
            Limpiar();
            await CargarAsync();
            await DisplayAlert("Éxito", "Estudiante guardado correctamente.", "Aceptar");
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Aceptar"); }
    }

    private async void Actualizar_Clicked(object? sender, EventArgs e)
    {
        if (_seleccionado is null)
        {
            await DisplayAlert("Aviso", "Seleccione una fila de la tabla para actualizar.", "Aceptar");
            return;
        }
        if (!Validar(out var edad)) return;
        try
        {
            _seleccionado.Nombre = _nombre.Text!.Trim();
            _seleccionado.Apellido = _apellido.Text!.Trim();
            _seleccionado.Edad = edad;
            await _database.GuardarAsync(_seleccionado);
            Limpiar();
            await CargarAsync();
            await DisplayAlert("Éxito", "Estudiante actualizado correctamente.", "Aceptar");
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Aceptar"); }
    }

    private async void Eliminar_Clicked(object? sender, EventArgs e)
    {
        if (_seleccionado is null)
        {
            await DisplayAlert("Aviso", "Seleccione una fila de la tabla para eliminar.", "Aceptar");
            return;
        }
        bool confirmar = await DisplayAlert("Eliminar estudiante", $"¿Desea eliminar a {_seleccionado.Nombre} {_seleccionado.Apellido}?", "Sí, eliminar", "Cancelar");
        if (!confirmar) return;
        try
        {
            await _database.EliminarAsync(_seleccionado);
            Limpiar();
            await CargarAsync();
            await DisplayAlert("Éxito", "Estudiante eliminado correctamente.", "Aceptar");
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Aceptar"); }
    }

    private void Limpiar()
    {
        _seleccionado = null;
        _nombre.Text = string.Empty;
        _apellido.Text = string.Empty;
        _edad.Text = string.Empty;
        _nombre.Focus();
    }
}
