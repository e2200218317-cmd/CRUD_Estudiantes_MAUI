# CRUD Estudiantes (.NET MAUI + SQLite)

Aplicación Android en .NET 10 MAUI con interfaz de formulario **Nombre / Apellido / Edad** y tabla **Código / Nombre / Apellido / Edad** (encabezados amarillos), de acuerdo con la actividad académica.

## Funciones
- **CREATE** Guardar: agrega un registro nuevo, con código autoincremental.
- **READ** Consultar: muestra todos los estudiantes en la tabla al abrir la aplicación y tras cada cambio.
- **UPDATE** Actualizar: toca una fila, edita los campos y presiona Actualizar.
- **DELETE** Eliminar: toca una fila, presiona Eliminar y confirma.
- **Limpiar**: vacía el formulario y deselecciona el registro.
- **Validaciones**: nombres y apellidos obligatorios; edad numérica entre 1 y 120.
- **Persistencia**: SQLite local en almacenamiento privado de la aplicación Android.

## Requisitos
- SDK .NET 10 y carga de trabajo de MAUI Android (`dotnet workload install maui-android`).
- Android SDK y un emulador configurado o teléfono Android con depuración USB.
- IDE: VS Code o Visual Studio.

## Ejecutar

```bash
dotnet restore
dotnet build -f net10.0-android
# Comprueba el dispositivo con: adb devices
dotnet build -t:Run -f net10.0-android
```

Si tu Android SDK requiere una instalación o ruta específica, configúrala antes de ejecutar. Los comandos se corren desde la carpeta que contiene el .csproj.

## Prueba recomendada y capturas para la tarea
1. Pantalla inicial con encabezados amarillos y formulario.
2. Crear tres estudiantes (cada uno con Nombre, Apellido y Edad), mostrar la tabla de tres filas.
3. Seleccionar una fila, cambiar la edad y presionar Actualizar; mostrar el resultado.
4. Seleccionar una fila, presionar Eliminar y confirmar; mostrar la tabla actualizada.
5. En GitHub, mostrar la URL pública del repositorio y el código fuente.

**Nota:** Debes ejecutar la aplicación y tomar capturas REALES del emulador o dispositivo. No se incluyen capturas inventadas.

## GitHub
Desde esta carpeta:
```bash
git init
git add .
git commit -m "CRUD completo estudiantes .NET MAUI con SQLite"
git branch -M main
git remote add origin https://github.com/TU_USUARIO/TU_REPOSITORIO.git
git push -u origin main
```
Primero crea el repositorio vacío en GitHub y reemplaza TU_USUARIO / TU_REPOSITORIO. Si ya hay un repositorio, evita repetir `git init` o `git remote add origin` sin verificar.

## Nota sobre Supabase
Este proyecto usa SQLite **local** (no Supabase). Para que varios compañeros compartan los mismos registros desde teléfonos diferentes, requiere adaptar el servicio de datos a Supabase con autenticación y políticas RLS seguras.
