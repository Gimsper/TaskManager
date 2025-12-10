# Task Manager

## Introducción
Un gestor de tareas que permite consultar, crear, editar y eliminar. Cuenta con una gestión de estados para cada tarea, lo que permite una clasificación y categorización de la información.

## Tecnologías utilizadas
### Base de datos
Se utiliza Sql Server como lenguaje principal para la creación de una base de datos relacional.

### Backend
Una API RESTful desarrollada con .NET 8.0. Para la gestión de los datos de utiliza EntityFramework como ORM que facilita la implementación de un CRUD a la hora de realizar la API. Se utiliza una arquitectura de capas (Repositorio, Servicio, Controlador) con inyección de dependencias para cada clase implementada. Se implementan pruebas unitarias para el StateService, utiles para asegurar el correcto funcionamiento de la lógica de negocio.

### Frontend
Se utiliza **Vite con React.ts**, lo que permite una interfaz y experiencia de usuario fluida entre las diferentes acciones que se pueden realizar.

## Instalación y uso
### Para la API
* Se debe configurar las cadenas de conexión de la base de datos que se utilizará, esto en el archivo **appsettings.json** del proyecto **TaskManagerAPI.WebAPI**.

```
"ConnectionStrings": {
  "ConnetionToken": "fz43IbcV/pqE/jvEbOJW/PPW/sUzmKblti5DENkxwWmeC5ZsNATNqqdvX3jxDU3qWLZE1LrzpWg3ub0xtRrq9cdIUIzAkEsIgoHxH4wuQssxJC4YaG07Qye+VnIHOuDHqyVxt7xZANozEZZBTNyzliVC1o/5WnFpnbGQ8tVUXm1eRuEmPl9rHw9MGBfdB88dBJK6YQQb0Zw=",
  "ConnetionGenerico": "fz43IbcV/pqE/jvEbOJW/PPW/sUzmKblti5DENkxwWmeC5ZsNATNqqdvX3jxDU3qWLZE1LrzpWg3ub0xtRrq9cdIUIzAkEsIgoHxH4wuQssxJC4YaG07Qye+VnIHOuDHqyVxt7xZANozEZZBTNyzliVC1o/5WnFpnbGQ8tVUXm1eRuEmPl9rHw9MGBfdB88dBJK6YQQb0Zw="
}
```

* En caso de no estar creada, se debe ejecutar el siguiente comando desde la **consola de administración de paquetes nuget de Vision Studio 2022** seleccionando como proyecto predeterminado **TaskManagerAPI.Domain**

```
Update-Database
```

Con esto se actualizará la base de datos con el nombre que se haya en la instancia que se haya configurado en el paso anterior.

* Luego, se debe ejecutar el siguiente script para habilitar el uso de usuario para la obtención del token JWT

```
IF OBJECT_ID('Usuarios', 'U') IS NOT NULL
BEGIN
	PRINT 'La tabla Usuarios ya existe'
END
ELSE
BEGIN
	CREATE TABLE Usuarios(
		[UsuarioId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
		[Nombres] [varchar](50) NULL,
		[Apellidos] [varchar](50) NULL,
		[Login] [nvarchar](max) NOT NULL,
		[Password] [varbinary](2000) NULL,
		[Salt] [varbinary](2000) NULL,
		[Tipo] [int] NOT NULL,
		[Activo] [bit] NOT NULL,
		[Email] [nvarchar](max) NULL,
		[TipoPass] [int] NOT NULL,
		[RefreshToken] [nvarchar](500) NULL,
		[TokenRestablecer] [nvarchar](2000) NULL,
		[TokenActivacion] [nvarchar](max) NULL,
		[FechaCreacion] [datetime2](7) NOT NULL,
		[FechaActivacion] [datetime2](7) NULL,
		[FechaUltIngreso] [datetime2](7) NULL,
		[Rol] [int] NULL
	)
	PRINT 'La tabla Usuarios ha sido creada.'
END

IF NOT EXISTS (SELECT * FROM Usuarios WHERE Nombres = 'User')
BEGIN
	INSERT INTO Usuarios (
		Nombres,
		Apellidos,
		Login,
		Password,
		Salt,
		Tipo,
		Activo,
		Email,
		TipoPass,
		RefreshToken,
		TokenRestablecer,
		TokenActivacion,
		FechaCreacion,
		FechaActivacion,
		FechaUltIngreso,
		Rol
	)
	VALUES (
		'User',
		'Token',
		'vwa-us',
		0x45C8511CC7988CEC7F4475B85D34BAD1D93BB17EAE264C5B339582FC88208FEDB412109DD978F752A81E3D6A96DC7196503C3E537A51771EFB8EB17017CCDB0E,
		0xE836A823A0E37BF564C5117C24508D886AF489A6788B72F6F14964AEAF81698EA25D09214D639D5ABFE907096D4AF619DD21FE94B30E09BABD9E0F1EC7F29FEADA0B50EECA60C50CEE768759D901952C3CA9E915BE5AD411D9BD960E63C5A90ECC0AD7EA3E27BE44A5315732414821B00462C3C4E1FC78D3757322C0D46DD507,
		1,
		1,
		'notificaciones@sistemasentry.com.co',
		1,
		'fdzZecM8HZSPh6V5GhBXCqlDgA+zY4RDDzQZEKSic7s=',
		NULL,
		NULL,
		'2021-09-30 13:28:35.4610911',
		'2021-09-30 13:28:35.4602008',
		'2025-08-08 09:29:28.6400000',
		1
	);
	PRINT 'Se han insertado los datos del usuario en la tabla "Usuarios"';
END
ELSE
BEGIN
	PRINT 'Los datos del usuario ya existen';
END

IF OBJECT_ID('UsuariosRol', 'U') IS NOT NULL
BEGIN
    PRINT 'La tabla UsuariosRol ya existe.'
END
ELSE
BEGIN
    CREATE TABLE UsuariosRol (
        IdUsuariosRol INT IDENTITY(1,1) PRIMARY KEY,
        Rol VARCHAR(50)
    );
    PRINT 'La tabla UsuariosRol ha sido creada.'
END
GO

IF NOT EXISTS (SELECT * FROM UsuariosRol WHERE Rol = 'Administrador')
BEGIN
    INSERT INTO UsuariosRol (Rol)
    VALUES ('Administrador');
    PRINT 'El rol Administrador ha sido insertado en UsuariosRol.'
END
ELSE
BEGIN
    PRINT 'El rol Administrador ya existe en UsuariosRol.'
END
GO

IF COL_LENGTH('Usuarios', 'Rol') IS NOT NULL
BEGIN
    PRINT 'La columna Rol ya existe en la tabla Usuarios.'
END
ELSE
BEGIN
    ALTER TABLE Usuarios
    ADD Rol INT;
    PRINT 'La columna Rol ha sido añadida a la tabla Usuarios.'
END
GO

UPDATE Usuarios
SET Rol = 1
WHERE Nombres = 'User';
GO
```
Finalmente, se podrá ejecutar y probar las funcionalidades de la API

### Para el App

* Usando el CMD se debe ingresar al directorio raiz del proyecto y se debe ejecutar el siguiente comando para la instalación de dependencias

```
npm install
```

* Luego de eso, para ejecutar la aplicación, se debe ingresar el siguiente comando estando en el directorio raiz del proyecto

```
npm run dev
```
