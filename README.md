# WJT Test

This is a test with an API built with `dotnet` calling two flaky APIs and a frontend built with `Next.js` in TypeScript and React.
The use case of the App is to compare movie price from different provider.
This solution is built only on Visual Studio Code on Ubuntu.

## API and App location

- The API is located in `<rootDir>/src/` and solution file is `wjt-test.sln`
- The App is located in `<rootDir>/src/Wtj.Ui` and it is expected that another separate coding environment open this folder directly during development
- See `README.md` in the subfolders for more details

## Fault tolerance

The flaky APIs result are cached on the server side only when there is no error and HTTP request use `Polly` to retry.
The frontend use react query with default retry to fetch request.

## Assumptions
1. The application is busy with a lot of requests
2. The application need to run mobile device
3. No SEO required
4. .NET 9.0 can be used
5. Dev environment is Linux (Ubuntu)
6. Assumption we can cache the movies returned by the backend API. The API response are valid for a period of time so response can be cached
7. Cinema World and Film World are separate services which could be hosted separately with different access token
8. Title, Year form a movie unique key - with different vendor providing the same contents
9. Image might need to go through a proxy, the UI need to trust https://m.media-amazon.com through and require content security policy

## Missing Production features
1. Web application firewall and rate limit are not implemented
2. Secret vault is not implemented, just use environment variables / app settings / `.env` file only
3. Logging solution is not required - telemetry can be implmented through ASP.NET Core and Next.js
4. Image loading is also flaky but not enough to time to work out what is wrong or how it can be improved
5. Production CORS is not setup for the API
6. No CI/CD is setup

## Requirements API
1. API support CORS
2. API has internal retry using polly
3. API need circuit breaker
4. Caching with HybridCache on server side
5. Two libraries for two different backend API

## Requirements UI
1. Lightweight UI

## Milestones
1. Initial structure setup
2. MVP - end to end API and search box without any fault tolerance
3. Add Unit tests
4. Add caching
5. Add security like CORS
6. Add fault tolerance
7. Dockerize

## Unknown
1. How many movies can the backend API return?
2. What should happen when the API actually fail? how important for user to know some vendors are not working?
3. How many more movies API we need to add?
4. How many more type of items we need to support?