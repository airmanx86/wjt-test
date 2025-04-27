# wjt-test.sln

`wjt-test.sln` is using .NET 9.0.
There is an API project and a couple of class libraries. Each class library has its own test project.

## Getting Started

First, set the missing API base URL and access token in `Wjt.Api/appsettings.Development.json`.

```bash
    "BaseUrl": "<set base url here>",
    "AccessToken": "<set x-access-token here>",
```

To run the development server:

```bash
dotnet run --project Wjt.Api/Wjt.Api.csproj
```

Access the API on [http://localhost:5256](http://localhost:5256).

To run test:

```bash
dotnet test
```

## Production build

The `Makefile` is setup to build this app into different environment using `Docker`

1. Run `make build-<environment>` to build the docker image.
2. Run `make start-<environment>` to start the app in container.
3. Run `make stop-<environment>` to stop the app container.

For example, `make build-development` to build the docker image for development.

## Note

1. CORS is not implemented in production build, but development has a custom CORS to allow any origin
2. Authentication is not implemented, which mean any one can use this API without signin
