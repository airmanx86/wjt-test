FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG TARGETARCH
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY *.props .
COPY Wjt.Api/*.csproj ./Wjt.Api/
COPY Wjt.CinemaWorld/*.csproj ./Wjt.CinemaWorld/
COPY Wjt.FilmWorld/*.csproj ./Wjt.FilmWorld/
COPY Wjt.Movies/*.csproj ./Wjt.Movies/
COPY Wjt.Test.CinemaWorld/*.csproj ./Wjt.Test.CinemaWorld/
COPY Wjt.Test.FilmWorld/*.csproj ./Wjt.Test.FilmWorld/
COPY Wjt.Test.Movies/*.csproj ./Wjt.Test.Movies/
RUN dotnet restore -a $TARGETARCH

# copy everything else and build app
COPY Wjt.Api/. ./Wjt.Api/
COPY Wjt.CinemaWorld/. ./Wjt.CinemaWorld/
COPY Wjt.FilmWorld/. ./Wjt.FilmWorld/
COPY Wjt.Movies/. ./Wjt.Movies/
COPY Wjt.Test.CinemaWorld/. ./Wjt.Test.CinemaWorld/
COPY Wjt.Test.FilmWorld/. ./Wjt.Test.FilmWorld/
COPY Wjt.Test.Movies/. ./Wjt.Test.Movies/
WORKDIR /source/Wjt.Api
RUN dotnet publish -a $TARGETARCH -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
EXPOSE 8080
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Wjt.Api.dll"]