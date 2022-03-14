# PKHeX-API
![License](https://img.shields.io/badge/license-GNU%20Affero%20General%20Public%20License%20Version%203%20or%20Later-blue.svg)

ASP.Net Core REST API providing PKHeX & ALM functionality.

This API currently supports creating Pokémon files using showdown sets for the following games:
- LGPE
- SWSH
- BDSP
- PLA

# Deployment
[Railway.app](https://railway.app/) is a platform where you can host and deploy apps in minutes. Clone this project as a private repo, click on "start a new project" on the railway dashboard, followed by "deploy from repo".

## Variables
These are needed variables that must be labeled for the project to deploy:
```
ASPNETCORE_ENVIRONMENT=Development
PKHEX_DEFAULT_LANGUAGE=English
PKHEX_DEFAULT_OT=SysBot.py
PKHEX_DEFAULT_SID=3383
PKHEX_DEFAULT_TID=258339
PORT=80
```

Note: OT, SID, and TID are customizable as long as they are within parameters.

## Swagger
Add `/swagger/index.html` to the end of your deployment to see an online interface for your instance.

## Public API Link
- [For development](https://pkhex-api-test-production.up.railway.app)
- [Online Swagger Interface](https://pkhex-api-test-production.up.railway.app/swagger/index.html)

## Credits
* [kwsch](https://github.com/kwsch) - developed PKHeX.Core
* [architdate](https://github.com/architdate) - developed Auto Legality Mod
* [DevYukine](https://github.com/DevYukine) - initial work for Pokegen.PkHex