I can't build or run it yet. The SDK still isn't installed here: dotnet --list-sdks prints nothing, and sqllocaldb isn't found either, so LocalDB is missing too.

I haven't tried installing anything myself, because that changes your machine. Type these with the ! prefix, and I'll take it from there:

! winget install Microsoft.DotNet.SDK.10
! winget install Microsoft.SQLServer.2022.LocalDB
                                                                                                                                              Then restart this Claude Code session. The new PATH onlo the current session won't see dotnet until you do.After that, tell me and I'll run:                                                                                                            
dotnet tool install --global dotnet-ef                                                                                                        dotnet build
dotnet ef migrations add InitialCreate                                                                                                        dotnet ef database update
dotnet run                                                                                                                                   
Where to see the output: dotnet run prints a line like Now listening on: http://localhost:5xxx. I'll tell you the real port once it runs. Add these paths to that address:

┌────────────────────────────────────────────────────┬──────────┐
│                        What                        │                  URL                  │
├────────────────────────────────────────────────────┼──────────┤
│ Swagger (REST)                                     │ /swagger                              │
├────────────────────────────────────────────────────┼──────────┤
│ GraphiQL (GraphQL UI, run queries and see results) │ /graphiql                             │
├────────────────────────────────────────────────────┼──────────┤
│ GraphQL endpoint (raw POST)                        │ /graphql                              │
├────────────────────────────────────────────────────┼──────────┤
│ REST list                                          │ /api/employees (opens in the browser) │
└────────────────────────────────────────────────────┴──────────┘

In GraphiQL, paste a query into the left pane and pressn the right pane.
