# payphone-practice
Prueba tecnica del equipo de payphone
Descripción
Este proyecto es una API REST creada con .NET 8 para gestionar transferencias de saldo entre dos billeteras. Permite a los usuarios realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre sus billeteras y operaciones CR (Crear, Leer) sobre el historial de movimientos.

Requisitos
Stack
Framework: .NET 8

Arquitectura: Clean Architecture

Persistencia: Entity Framework Core y/o Dapper

Base de datos: De libre elección (ej. SQL Server, MySQL, etc.)

Pruebas: Unitarias e integración

Enfoque de desarrollo: Principios SOLID y buenas prácticas

Modelo de Datos
Billetera
id: Identificador único (número entero autoincremental)

documentId: Documento de identidad de la persona propietaria de la billetera

name: Nombre del propietario de la billetera

balance: Saldo de la billetera

createdAt: Fecha de apertura de la billetera

updatedAt: Fecha de última actualización

Historial de Movimientos
id: Identificador único (número entero autoincremental)

walletId: Identificador de la billetera

amount: Monto de la transferencia

type: Tipo de operación (Débito/Crédito)

createdAt: Fecha del movimiento

Instrucciones para ejecutar el proyecto
Paso 1: Clonar el repositorio
Primero, clona el repositorio en tu máquina local:

bash
Copy
Edit
git clone https://github.com/tu_usuario/tu_repositorio.git
Paso 2: Crear la base de datos
Antes de ejecutar la API, necesitas configurar la base de datos. Ejecuta el siguiente script para crear las tablas necesarias:

Abre tu entorno de base de datos preferido (por ejemplo, SQL Server, MySQL).

Ejecuta el script de creación de la base de datos proporcionado en el archivo db-setup.sql para crear las tablas correspondientes a las billeteras y los movimientos.

Paso 3: Configuración de la base de datos
Asegúrate de que la cadena de conexión en appsettings.json esté correctamente configurada para tu entorno:

json
Copy
Edit
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WalletDb;User Id=sa;Password=yourpassword;"
  }
}
Paso 4: Ejecutar el proyecto
Ejecuta el proyecto desde tu entorno de desarrollo:

bash
Copy
Edit
dotnet run
La API debería estar funcionando en http://localhost:5000 (o el puerto que tengas configurado).

Paso 5: Realizar pruebas
Asegúrate de que todos los endpoints funcionen correctamente. Se incluyen pruebas unitarias e integración para validar las funcionalidades del sistema.

Endpoints
(Agregar una breve descripción de los endpoints disponibles en la API aquí)

Autenticación y Autorización (Opcional)
La API soporta un sistema de autenticación básico para que solo los usuarios autenticados puedan realizar ciertas operaciones (Crear, Actualizar, Eliminar billeteras y transferencias).
