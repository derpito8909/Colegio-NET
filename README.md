<h1 align="center"> Prueba Técnica - Audisoft(Colegio CRUD)</h1>

## BackEnd Colegio API (.NET 8 + SqlServer + Fluent Validation)
### API REST: Expone Entidades para Estudiantes, Profesores y Estudiantes un CRUD basico validaciones y constrains que se Conecta a **SQLServer**.
---
<details open>
<summary>
## ✅ Pre-requisitos
</summary>
Para ejecutar la aplicacion necesita tener instalado:

- Instacia de SQLServer
- Crear BD + tablas del programa con este script: 
[Create_SQL.sql](./Create_SQL.sql)
</details>

<details open>
<summary>
## 🚀 Ejecutar la aplicacion
</summary> <br>
para ejecutar la aplicacion:

1. Clone el repositorio:

```shell
https://github.com/derpito8909/Colegio-NET.git
```
2. Ingrese a la carpeta /Colegio-NET/Colegio_API/ e ingrese estos comandos para inicar la aplicacion

```shell
 cd Colegio-NET/Colegio_API/
 dotnet restore
 dotnet build
 dotnet run --project src/Colegio.Api/Colegio.Api.csproj
```
</details>

<details open>
<summary>
Descripción de los Endpoint
</summary> <br />

## Endpoint: `GET /api/Estudiante`

- **Método:** `GET`
- **Descripción:** Consigue todos los estudiantes

### Respuesta

```json
[
  {
    "id": 1,
    "nombre": "David"
  }
]
```
## Endpoint: `POST /api/Estudiante`

- **Método:** `POST`
- **Descripción:** Crear un nuevo estudiante

### Parámetros de Solicitud

- `nombre` (requerido): cadena de caracteres para el nombre de estudiante

```json
{
  "nombre": "string"
}
```

### Respuesta

```json
{
  "id": 1,
  "nombre": "David"
}
```
## Endpoint GET /api/Estudiante/{id}

- **Método:** `GET`
- **Descripción:** Obtiene un estudiante por id

### Parámetros de Solicitud

- `id` (requerido): identificación del estudiante

### Respuesta

```json
{
  "id": 1,
  "nombre": "David Rodriguez"
}
```
## Endpoint PUT /api/Estudiante/{1}
- **Método:** `PUT`
- **Descripción:** Actualiza el estudiante
- 
### Parámetros de Solicitud

- `id` (requerido): identificación del estudiante
- `nombre` (requerido): nombre del estudiante
  
### Respuesta
Code 204

## Endpoint Delete /Estudiante/{id}

- **Método:** `DELETE`
- **Descripción:** Elimina un estudiante
### Parámetros de Solicitud

- `id` (requerido): identificación del estudiante
- `nombre` (requerido): nombre del estudiante
  
### Respuesta
Code 204

## Endpoint: `GET /api/Profesor`

- **Método:** `GET`
- **Descripción:** Consigue todos los profesores

### Respuesta

```json
[
  {
    "id": 1,
    "nombre": "Francisco"
  }
]
```
## Endpoint: `POST /api/Profesor`

- **Método:** `POST`
- **Descripción:** Crear un nuevo Profesor

### Parámetros de Solicitud

- `nombre` (requerido): cadena de caracteres para el nombre del profesor

```json
{
  "nombre": "string"
}
```

### Respuesta

```json
{
  "id": 1,
  "nombre": "Francisco"
}
```
## Endpoint GET /api/Profesor/{id}

- **Método:** `GET`
- **Descripción:** Obtiene un profesor por id

### Parámetros de Solicitud

- `id` (requerido): identificación del profesor

### Respuesta

```json
{
  "id": 1,
  "nombre": "Francisco"
}
```
## Endpoint PUT /api/Profesor/{1}
- **Método:** `PUT`
- **Descripción:** Actualiza el profesor
- 
### Parámetros de Solicitud

- `id` (requerido): identificación del profesor
- `nombre` (requerido): nombre del profesor
  
### Respuesta
Code 204

## Endpoint Delete /Profesor/{id}

- **Método:** `DELETE`
- **Descripción:** Elimina un profesor
### Parámetros de Solicitud

- `id` (requerido): identificación del Profesor
- `nombre` (requerido): nombre del profesor
  
### Respuesta
Code 204
## Endpoint: `GET /api/Notas`

- **Método:** `GET`
- **Descripción:** Consigue todas las notas

### Respuesta

```json
[
  {
    "id": 1,
    "nombre": "aceptable",
    "idProfesor": 1,
    "idEstudiante": 1,
    "valor": 5
  }
]
```
## Endpoint: `POST /api/Notas`

- **Método:** `POST`
- **Descripción:** Crear una nueva Nota

### Parámetros de Solicitud

- `nombre` (requerido): cadena de caracteres para el nombre de la nota
- `idProfesor` (requerido): identificacion del profesor
- `idEstudiante` (requerido): identificacion del estudiante
- `valor` (requerido): numero de 0 al 10 que corresponde al valor de la nota

```json
{
  "id": 1,
  "nombre": "Aceptable",
  "idProfesor": 1,
  "idEstudiante": 1,
  "valor": 5
}
```

### Respuesta

```json
{
  "id": 1,
  "nombre": "Aceptable",
  "idProfesor": 1,
  "idEstudiante": 1,
  "valor": 5
}
```
## Endpoint GET /api/Notas/{id}

- **Método:** `GET`
- **Descripción:** Obtiene una nota por id

### Parámetros de Solicitud

- `id` (requerido): identificación de la nota

### Respuesta

```json
{
  "id": 1,
  "nombre": "aceptable",
  "idProfesor": 1,
  "idEstudiante": 1,
  "valor": 5
}
```
## Endpoint PUT /api/Notas/{1}
- **Método:** `PUT`
- **Descripción:** Actualiza la Nota
- 
### Parámetros de Solicitud

- `nombre` (requerido): cadena de caracteres para el nombre de la nota
- `idProfesor` (requerido): identificacion del profesor
- `idEstudiante` (requerido): identificacion del estudiante
- `valor` (requerido): numero de 0 al 10 que corresponde al valor de la nota
  
### Respuesta
Code 204

## Endpoint Delete /Nota/{id}

- **Método:** `DELETE`
- **Descripción:** Elimina una
### Parámetros de Solicitud

- `nombre` (requerido): cadena de caracteres para el nombre de la nota
- `idProfesor` (requerido): identificacion del profesor
- `idEstudiante` (requerido): identificacion del estudiante
- `valor` (requerido): numero de 0 al 10 que corresponde al valor de la nota
  
### Respuesta
Code 204

## Endpoint: `GET /api/Notas/detalle`

- **Método:** `GET`
- **Descripción:** Lista Todas las notas en formato detalle (incluye nombre relacionados)

### Respuesta

```json
[
  {
    "id": 0,
    "nombre": "string",
    "valor": 0,
    "idProfesor": 0,
    "profesorNombre": "string",
    "idEstudiante": 0,
    "estudianteNombre": "string"
  }
]
```
## Endpoint GET /api/Notas/{id}/detalle

- **Método:** `GET`
- **Descripción:** Obtiene una nota por id (vista detalle), Devuelve información ampliada, incluyendo nombres de profesor y estudiante. 

### Parámetros de Solicitud

- `id` (requerido): identificación de la nota

### Respuesta

```json
{
  "id": 0,
  "nombre": "string",
  "valor": 0,
  "idProfesor": 0,
  "profesorNombre": "string",
  "idEstudiante": 0,
  "estudianteNombre": "string"
}
```
</details>
