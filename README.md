# mjonesmmttest
MMT Senior Server-Side Technical Test

ASPNET WEB API Coded in .NET 5
Written lagely test-first, with the basic integration testing suite added as a final stage.
To ensure that sensitive information is not checked into the repository, a user-secrets configuration was setup, targeted at developer environments.
The user-secrets were installed at the command-line in the root of the MJonesMmtTest.Api project. The IntegrationTests project reuses these via the TestFixture and HostBuilder approach. The keys hidden away are "OrderDbConnection", "ApiKey" and "ApiEndpoint". For clarity's sake, the endpoint stored was the entire URI supplied, up to and including the "api" element. So, "https://*****/api"

It still remains a basic project but does its best to obey the spirit of the technical test document without building up too much of the very necessary infrasture required for anything that gets close to a production environment. Some of those elements would be an expanded approach to logging and instrumentation, a better approach to controlling secrets (e.g., a KeyVault and application registration via Azure), enforcement of secure sockets and, depending on the consumer, the use of HMAC or JWT-token authentication techniques. If order data is unlikely to change then some basic caching mechanisms may assist scalability as the application grows.

Thank you and all the best
Mark
