# 🏥 MediAgenda 

Este es un proyecto centrado en la gestión de consultas médicas para un médico en particular, con una arquitectura pensada para que en el futuro pueda escalar y soportar múltiples médicos.

El proyecto nació a partir de una necesidad real de un familiar médico y fue implementado inicialmente como parte de uno de los proyectos requeridos en el ITLA. Esta versión corresponde a una **API REST profesional**, enfocada exclusivamente en el **backend**, con documentación interactiva mediante **Swagger**.

El mismo esta desplegado en [Somee](https://somee.com/) una plataforma la cual permite hostear aplicaciones de .NET con bases de datos SQL Server.

[Despliegue](https://mediagenda.somee.com/swagger/index.html)

---

## 🛠️ Tecnologías

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
    <td width="120" align="center" style="padding:10px; border:2px solid #3178C6; border-radius:10px; background-color:#1e1e1e;">
      <img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/entityframeworkcore/entityframeworkcore-original.svg" width="50"/>
      <br><strong style="color:white;">Entity Framework Core</strong>
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

---

## 🏷️ Funcionalidades de la API

### 👤 Funcionalidades orientadas al paciente

* Agendamiento, cancelación y reprogramación de consultas médicas.
* Consulta del historial de medicamentos y análisis recetados.
* Acceso al historial de certificados médicos.
* Gestión y consulta de documentos médicos entregados al doctor.

### 👨‍⚕️ Funcionalidades orientadas al médico

* Gestión de días y horarios disponibles para consultas.
* Control de la cantidad de citas permitidas por día.
* Gestión de medicamentos, análisis y certificados médicos.
* Registro de notas médicas privadas por consulta y paciente.
* Acceso completo al historial clínico del paciente.
* Generación e impresión de recetas médicas.

---

## 🔐 Seguridad

* Autenticación basada en **JWT**.
* Autorización por **roles**.
* Validaciones de entrada con **FluentValidation**.
* Métodos personalizados de autorización.
* Logging centralizado y estructurado.

---

## 🏗️ Arquitectura

* Arquitectura en **N-capas**:

  * API
  * Application
  * Domain
  * Infrastructure
* Separación clara de responsabilidades.
* Uso de **inyección de dependencias**.
* Preparada para escalar y desacoplar clientes (web, móvil, desktop).

---

## ▶️ Cómo ejecutar el proyecto

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Una vez iniciado, la documentación de la API estará disponible en **Swagger**, lo que permite probar y explorar todos los endpoints expuestos.

---

## 🌐 O visitar la API desplegada

Puedes acceder desde el [link](https://mediagenda.somee.com/swagger/index.html) de aca y te encontraras con la API desplegada en Somee.

Veras algo asi:

<img width="1516" height="728" alt="imagen del despliegue" src="https://github.com/user-attachments/assets/d6636f85-bb84-4c05-bfe2-767197989afb" />



