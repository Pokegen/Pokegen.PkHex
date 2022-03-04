# Pokégen.PkHex

ASP.Net Core REST API providing PKHeX & ALM functionality

## Getting Started

This API currently supports creating pokemon files (.pk*, .pb* etc.) from Showdown Sets via [ALM](https://github.com/architdate/PKHeX-Plugins), checking legality of Showdown Sets/Pokemon files via [PKHeX](https://github.com/kwsch/PKHeX) & getting trainer data from the data block!

For a full overview of all routes you want to set the `ASPNETCORE_ENVIRONMENT` environment variable to `Development` which should expose an Swagger (OpenAPI integration) page at /swagger/index.html showing all routes and parameters.

## Deployment
TBD

## Built With

* [ASP.NET Core](https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core) - The web framework used
* [Nuget](https://www.nuget.org/) - Dependency Management
* [Serilog](https://serilog.net/) - The logging framework used
* [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) - Swagger (OpenAPI) integration
* [PKHeX](https://github.com/kwsch/PKHeX) - Pokemon Save File Editor
* [AutoLegalityMod](https://github.com/architdate/PKHeX-Plugins) - PKHeX Automated Modifications 

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/Pokegen/Pokegen.PkHex/tags). 

## Authors

* **Kwsch** - *Creation of [PKHeX](https://github.com/kwsch/PKHeX)* - [Kwsch](https://github.com/kwsch)
* **architdate** - *Creation of [AutoLegalityMod](https://github.com/architdate/PKHeX-Plugins)* - [architdate](https://github.com/architdate)
* **DevYukine** - *Initial work* - [DevYukine](https://github.com/DevYukine)
* **6A** - *LGPE support* - [6A](https://github.com/6A-Realm)

See also the list of [contributors](https://github.com/Pokegen/Pokegen.PkHex/contributors) who participated in this project.

## License

This project is licensed under the GPL-3.0 License - see the [LICENSE](LICENSE) file for details