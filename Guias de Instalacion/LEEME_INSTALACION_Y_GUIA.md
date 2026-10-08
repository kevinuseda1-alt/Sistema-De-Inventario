# Bike Store · gestión de inventario

Esta entrega parte de `PedidosApp_LISTO_KEVIN_V2(4).zip`. Mantiene las tres
capas y los datos de ventas anteriores, pero la navegación principal muestra
productos, categorías, proveedores, inventario, usuarios y un resumen de stock.
El botón de proveedores ahora permite crear, editar, desactivar y asociar
proveedores con productos. El inventario conserva entradas, salidas, historial,
saldo disponible y mínimo editable. Los importes se **presentan** como C$;
ningún número existente se multiplicó por una tasa de cambio.

## Si ya usás Bike_Store en tu computadora

1. En SSMS, hacé clic derecho en `Bike_Store` → **Tareas → Copia de seguridad**.
   Elegí **Completa**, un archivo `.bak` y verificá que terminó correctamente.
2. Abrí una consulta conectada a la misma instancia de la aplicación y ejecutá
   **solo** `SQL/02_Ampliar_Inventario.sql`. Conserva los datos existentes.
3. Abrí `PedidosApp.sln` en Visual Studio. En
   `CapaPresentacion/App.config`, cambia `Data Source=.\SQLEXPRESS` por el
   nombre de tu instancia si es distinto. Compilá la solución y ejecutá.

## Instalar en otra computadora sin llevar los registros anteriores

1. Instalá SQL Server, SSMS, Visual Studio y .NET Framework 4.7.2.
2. Conectate en SSMS a la instancia de esa computadora (por ejemplo
   `.\SQLEXPRESS`). Abrí **una Nueva consulta** y ejecutá el archivo completo
   `SQL/INSTALAR_TODO_NUEVA_PC.sql` **solo si Bike_Store aún no existe**.
   El script no borra otras bases. Crea el esquema anterior, el usuario de
   prueba y la ampliación de proveedores e inventario.
3. Abrí `PedidosApp.sln`. En `CapaPresentacion/App.config` escribí esa misma
   instancia en `Data Source`. Es autenticación de Windows: la cuenta de
   Windows que ejecute C# debe poder entrar a `Bike_Store` en SQL Server.
4. Restaurá paquetes NuGet si Visual Studio lo solicita, **Recompilar solución**,
   configurá `CapaPresentacion` como proyecto de inicio y ejecutá.
5. Usuario inicial heredado: `josue`; clave inicial: `123456`. Cambiala
   después de la instalación. La ampliación convierte esa clave a PBKDF2;
   otros usuarios antiguos se convierten al ingresar con su clave correcta.

## Llevar también los registros reales de una PC a otra

En la PC de origen, creá la **copia completa `.bak`** de `Bike_Store` desde
SSMS. Copiá el `.bak` a la otra PC. En SSMS de destino, clic derecho en
**Bases de datos → Restaurar base de datos**, seleccioná **Dispositivo**,
agregá el archivo y restaurá `Bike_Store`. Si la base ya existe en destino,
respaldala primero y revisá las opciones de reemplazo; no ejecutes el script
de instalación limpia encima. Tras restaurar, ejecutá
`SQL/02_Ampliar_Inventario.sql` y configurá `App.config`.

El archivo antiguo destructivo se guardó como
`SQL/REFERENCIA_ORIGINAL_DESTRUCTIVO_NO_EJECUTAR.sql`; **no lo ejecutes**.

## Prueba rápida

1. Entrá al programa, creá un proveedor y asocialo con un producto.
2. Registrá una entrada, luego una salida menor al saldo; verificá historial.
3. Modificá el mínimo del producto. El tablero y la consulta de reposición
   deben reflejar la alerta cuando el saldo sea menor o igual al mínimo.
4. En SSMS, ejecutá `SQL/03_Consultas_Exposicion.sql` y comprobá que aparecen
   los datos creados en C#.

## Relación con la guía académica (pasos 1 al 19)

La guía entregada es de **Registro de Estudiantes, WinForms .NET 8 y
Microsoft.Data.SqlClient**. Bike Store usa **.NET Framework 4.7.2 y
System.Data.SqlClient**; por eso esta tabla registra equivalencias de
inventario, no una implementación literal del sistema de estudiantes.

| Paso | Equivalente de inventario |
| --- | --- |
| 1–2 | Copia de la versión anterior; conservar capas y archivos originales. |
| 3–4 | `Bike_Store`, tablas de productos, categorías, usuarios, inventario y proveedores. |
| 5–6 | ADO.NET con `System.Data.SqlClient`; instancia editable en `App.config`. |
| 7–9 | Producto y operaciones de inserción en `DProducts`/`NProducts`. |
| 10–11 | Categorías y listado de productos desde SQL en `DataGridView`. |
| 12–13 | Búsqueda parametrizada y edición de productos/proveedores. |
| 14 | Desactivación lógica de productos y proveedores con `is_active`. |
| 15 | Procedimientos almacenados del sistema, incluido el de inventario. |
| 16 | Entrada/salida con comprobación de stock y transacción en SQL. |
| 17 | Login y roles existentes; claves PBKDF2 + sal, con migración de usuarios antiguos. |
| 18 | Historial de movimientos por producto y usuario. |
| 19 | Consulta de existencias bajo mínimo con proveedores para reposición. |

No existen asignaturas aprobadas ni prerrequisitos en un inventario. Si el
docente exige literalmente esas funciones o .NET 8, esta adaptación no las
sustituye. Tampoco se verificó la ejecución en Visual Studio/SQL Server de
Windows desde este entorno: la compilación y prueba rápida indicadas arriba
son necesarias antes de entregar.
