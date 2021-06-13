# mjonesmmttest
MMT Senior Server-Side Technical Test

ASPNET WEB API Coded in .NET 5, written largely test-first, with a basic integration testing suite added as a final stage. Unit tests and integration tests are labelled differently to allow selective running based on their categories. 

To ensure that sensitive information is not checked into the repository, a user-secrets configuration was setup, targeted at developer environments.
The user-secrets were installed at the command-line in the root of the MJonesMmtTest.Api project. The IntegrationTests project reuses these via the TestFixture and HostBuilder approach. The keys hidden away are "OrderDbConnection", "ApiKey" and "ApiEndpoint". For clarity's sake, the endpoint stored was the entire URI supplied, up to and including the "api" element. So, "https://*****/api"

It still remains a basic project but does its best to obey the spirit of the technical test document without building up too much of the very necessary infrasture required for anything that gets close to a production environment. Some of those elements would be an expanded approach to logging and instrumentation, a better approach to controlling secrets (e.g., a KeyVault and application registration via Azure), enforcement of secure sockets and, depending on the consumer, the use of HMAC or JWT-token authentication techniques. If order data is unlikely to change then some basic caching mechanisms may assist scalability as the application grows.

As an example of something to expand on, feature-wise, the mismatch of a customer with an order is potentially a difficult scenario. Any such instance would need to be escalated to a sales team or some such for investigation - notifications such as these would be a perfect candidate for workflow (logic app)/event grid/service bus.

Thank you and all the best
Mark
