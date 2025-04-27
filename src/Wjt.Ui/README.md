# Wjt.UI

Wjt.Ui is using [Next.js](https://nextjs.org) with [Jest](https://jestjs.io) for testing.

## Getting Started

First, create `.env` file with the following details.

```bash
NEXT_PUBLIC_MOVIES_API_BASE_URL=http://localhost:5256
ALLOWED_IMAGE_SRC=https://m.media-amazon.com
```

1. `NEXT_PUBLIC_MOVIES_API_BASE_URL` is the base URL of the Movies API
2. `ALLOWED_IMAGE_SRC` is the allowed third party image source for UI to load movie image

To run the development server:

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the Movie Price Check App.


To run test:

```bash
npm run test
```

or with watch

```bash
npm run test:watch
```

## Production build

The `Makefile` is setup to build this app into different environment using `Docker`

Create the following `.env` files for each environment

- development - `.env.development`
- staging - `.env.staging`
- production - `.env.production`

1. Run `make build-<environment>` to build the docker image.
2. Run `make start-<environment>` to start the app in container.
3. Run `make stop-<environment>` to stop the app container.

For example, `make build-development` to build the docker image for development.

## Note

1. Content Security Policy (CSP) is implemented via `middleware.ts`. External image source is allowed
2. The page is not using server side rendering
3. No authentication is not implemented, which mean any one can use this app without signin
