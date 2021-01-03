#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet PoseDatabaseWebApi.dll


#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
## Default working folder
#WORKDIR /source/PoseDatabaseWebApi
#
## copy csproj and restore as distinct layers
##COPY *.sln ./
## copy everything else and build app
#COPY * .
#RUN dotnet restore
#
#WORKDIR /source/PoseDatabaseWebApi
#RUN dotnet publish -c release -o /app --no-restore
#
## final stage/image
#FROM mcr.microsoft.com/dotnet/aspnet:5.0
#WORKDIR /app
#COPY --from=build /app ./
##ENTRYPOINT ["dotnet", "PoseDatabaseWebApi.dll"] <--When this was here the app would crash on heroku
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet PoseDatabaseWebApi.dll #<---This made the app run on heroku but something is still not working