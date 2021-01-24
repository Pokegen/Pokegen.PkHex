# Pokégen.PkHex

ASP.Net Core REST API providing PkHex & ALM functionality

## Getting Started

### Routes

#### Get Pokemon Data (including Legality Checks)
- `/showdown` 
  
  Expects JSON Body with `showdownSet` property which contains an [Pokemon Showdown](https://pokemonshowdown.com/) set.

- `/url`

  Expects a Query parameter named `url` containing the url of the Pokemon file, Optional, the size of the file if already known ahead of making a request.

- `/file`

  Expects a Multipart form parameter called `file` which contains a Pokemon file.

These endpoints all are `GET` endpoints (`/showdown` & `/file` also accept `POST` requests because of browser compatibility) & return the Pokemon Data with `application/octet-stream` MIME type.	

#### Legality Checks
- `/showdown/legality`

  Expects JSON Body with `showdownSet` property which contains an [Pokemon Showdown](https://pokemonshowdown.com/) set.

- `/url`

  Expects a Query parameter named `url` containing the url of the Pokemon file, Optional, the size of the file if already known ahead of making a request.

- `/file`

  Expects a Multipart form parameter called `file` which contains a Pokemon file.

These endpoints all are `GET` endpoints (`/showdown` & `/file` also accept `POST` requests because of browser compatibility) & return either `204 No Content` if Legal or `400 Bad Request` if they are illegal.

## Deployment
TBD

## Built With

* [ASP.NET Core](https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core) - The web framework used
* [Nuget](https://www.nuget.org/) - Dependency Management
* [Serilog](https://serilog.net/) - The logging framework used
* [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) - Swagger (OpenAPI) Integration

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/Pokegen/Pokegen.PkHex/tags). 

## Authors

* **Kwsch** - *Creation of PkHex* - [Kwsch](https://github.com/kwsch/PKHeX)
* **architdate** - *Creation of AutoLegalityMod* - [architdate](https://github.com/architdate)
* **DevYukine** - *Initial work* - [DevYukine](https://github.com/DevYukine)

See also the list of [contributors](https://github.com/Pokegen/Pokegen.PkHex/contributors) who participated in this project.

## License

This project is licensed under the GPL-3.0 License - see the [LICENSE.md](LICENSE.md) file for details
