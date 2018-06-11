# AngryWasp.Logger

## Prerequisites

Mono/.NET

- Ubuntu 17.10

    `sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF`  
    `echo "deb http://download.mono-project.com/repo/ubuntu xenial/snapshots/5.8.0.127 main" | sudo tee /etc/apt/sources.list.d/mono.list`  
    `sudo apt update`  
    `sudo apt install mono-complete`

## Building

Linux requires .NET Core mono to build

- Ubuntu 17.10

    `cd ./AngryWasp.Logger`  
    `dotnet restore`  
    `msbuild /p:configuration=Release`

## How-To

Check the file `AngryWasp.Logger.Sample/Program.cs` for example usage
