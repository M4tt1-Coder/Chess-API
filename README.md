# Chess-API

Chess-API is an ASP.NET Core MVC chess web app.  
It currently supports local game setup and interactive board play in the browser, backed by an in-memory EF Core database.

## Tech stack

- .NET 7 (ASP.NET Core MVC)
- Entity Framework Core InMemory
- Tailwind CSS (via npm)

## Prerequisites

- .NET SDK 7.x (recommended), or a newer .NET SDK **plus** the .NET 7 runtime to run this `net7.0` app
- Node.js + npm

## Installation

1. Clone the repository.
2. Install frontend dependencies:

```bash
npm install
```

3. (Optional) Review environment values in `.env` (piece image paths and UI text).

## Run the project

From the repository root:

```bash
dotnet run
```

Or run with launch profiles:

```bash
dotnet run --launch-profile http
dotnet run --launch-profile https
```

Default development URLs are defined in `Properties/launchSettings.json`:

- `http://localhost:5184`
- `https://localhost:7257`

## Build and validation

Build:

```bash
dotnet build Chess-API.sln
```

Tests (no dedicated test project yet, command is still valid):

```bash
dotnet test Chess-API.sln --no-build
```

Tailwind CSS output is generated during build via the project target (`npm run css:build`).

## General usage flow

1. Open `/` (Home) and click **Start**.
2. Go to `/settings` and choose a mode:
   - Player vs Player (local)
   - Player vs AI (local)
   - Player vs Player (online, planned/in progress)
3. In `/settings/options?modeId=...`, configure players/time/mode and submit.
4. You are redirected to `/playing`, where you interact with the board.
5. Moves are posted to `/playing/move`.

## Main routes

- `GET /` – Home page
- `GET /settings` – Mode selection
- `GET /settings/options?modeId={id}` – Game options
- `POST /settings/options/save` – Save options and start game
- `GET /playing` – Playing page
- `POST /playing/move` – Piece selection/move interaction

## Development notes

- The app uses an in-memory database (`ChessDatabase`), so game state is not persistent across app restarts.
- The project currently contains TODOs for features like castling, promotion, en passant, and additional game flow controls.
- `npm run format` expects `csharpier` to be installed globally/in your environment.

## License

This project is licensed under the MIT License. See [LICENSE](./LICENSE).
