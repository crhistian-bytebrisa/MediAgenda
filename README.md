# MediAgenda

Este es un proyecto centrado en la gestión de consultas médicas para un médico en particular, pero con ciertas configuraciones que permitirían en un futuro implementarlo para varios médicos.

Este proyecto nació a partir de una necesidad de un familiar médico e implementé la versión inicial para uno de los proyectos necesarios en el ITLA. Este repositorio contiene una API más profesional sobre el mismo proyecto, además de documentación con Swagger y un frontend.

# Tecnologías

<h3 align="center">🛠️ Backend</h3>

<table align="center">
  <tr>
    <td width="120" align="center" style="padding:10px; border:2px solid #512BD4; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dotnetcore/dotnetcore-original.svg" width="50" />
      <br><strong style="color:white;">ASP.NET</strong>
    </td>
    <td width="120" align="center" style="padding:10px; border:2px solid #239120; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" width="50" />
      <br><strong style="color:white;">C#</strong>
    </td>
    <td width="120" align="center" style="padding:10px; border:2px solid #CC2927; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/microsoftsqlserver/microsoftsqlserver-plain.svg" width="50" />
      <br><strong style="color:white;">SQL Server</strong>
    </td>
  </tr>  
</table>
<table align="center">
  <tr>
    <td width="120" align="center" style="padding:10px; border:2px solid #F7DF1E; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://avatars.githubusercontent.com/u/5691010?s=200&v=4" width="50" />
      <br><strong style="color:white;">Serilog</strong>
    </td>
    <td width="120" align="center" style="padding:10px; border:2px solid #3178C6; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/entityframeworkcore/entityframeworkcore-original.svg" width="50"/>
      <br><strong style="color:white;">Entity Framework</strong>
    </td>
    <td width="120" align="center" style="padding:10px; border:2px solid #61DAFB; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/swagger/swagger-original.svg" width="50"/>
      <br><strong style="color:white;">Swagger</strong>
    </td>
    <td width="120" align="center" style="padding:10px; border:2px solid #61DAFB; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://api.nuget.org/v3-flatcontainer/fluentvalidation/12.1.1/icon" width="50" />
      <br><strong style="color:white;">FluentValidation</strong>      
    </td>
    <td width="120" align="center" style="padding:10px; border:2px solid #61DAFB; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://cloud.githubusercontent.com/assets/5763993/26522718/d16f3e42-4330-11e7-9b78-f8c7402624e7.png" width="50" />
      <br><strong style="color:white;">Mapster</strong>      
    </td>
  </tr>
</table>

<h3 align="center">🎨 Frontend</h3>

<table align="center">
  <tr>
    <td width="120" align="center" style="padding:10px; border:2px solid #F7DF1E; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/javascript/javascript-original.svg" width="50" />
      <br><strong style="color:white;">JavaScript</strong>
    </td>
    <td width="120" align="center" style="padding:10px; border:2px solid #3178C6; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/typescript/typescript-original.svg" width="50" />
      <br><strong style="color:white;">TypeScript</strong>
    </td>   
  </tr>
</table>
<table align="center">
  <tr>
    <td width="120" align="center" style="padding:10px; border:2px solid #61DAFB; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/react/react-original.svg" width="50" />
      <br><strong style="color:white;">React</strong>
    </td>
  </tr>
</table>
<br/>

## 🏷️ Funcionalidades

🧑 Desde el punto de vista del paciente:

* Agendar consultas, cancelarlas o reagendarlas.
* Historial de medicamentos y análisis recetados por su médico.
* Historial de certificados médicos.
* Acceso a documentos entregados al médico para mayor facilidad a la hora de ser referidos.

🧑‍⚕️ Desde el punto de vista del médico:

* Gestión de días disponibles para consultas, además de un control sobre la cantidad de citas por la web.
* Gestión de medicinas, análisis y certificados médicos.
* Gestión de notas para las consultas y los pacientes, solo accesibles para el médico.
* Capacidad de imprimir recetas.
* Acceso total al historial del paciente.
<br/>

## 🔐 Seguridad

* Autenticación basada en JWT
* Autorización por roles
* Validaciones con FluentValidation
* Métodos personalizados de autorización
* Logging centralizado con Serilog
<br/>

## 🏗️ Arquitectura

### Backend

* Arquitectura en N-capas:

  * API
  * Application
  * Domain
  * Infrastructure
* Inyección de dependencias

<br/>
<br/>

# ▶️ Cómo ejecutar el proyecto

### Backend

```bash
cd Backend/src
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend

```bash
cd Frontend
npm install
npm run dev
```

