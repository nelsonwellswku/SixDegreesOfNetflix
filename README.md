# SixDegreesOfNetflix

Inspired by [Six Degrees of Kevin Bacon](https://en.wikipedia.org/wiki/Six_Degrees_of_Kevin_Bacon). Given two actors, the app will tell you the relationship path between them. For example, when giving Johnny Depp and Tom Cruise, you may get

* Johnny Depp acted in Fear and Loathing in Las Vegas with Cameron Diaz
* Cameron Diaz acted in Vanilla Sky with Tom Cruise

The data comes from the  [The Movie Database](https://www.themoviedb.org)'s API so the results will only be as good as their data set. Additionally, the data is loaded into a database at the point that a user submits the form with the two actors. Because of that, I explicitly limit the number of requests to the API so that they aren't DOS'd on accident. Keep this in mind if the results aren't quite accurate or when you notice that loading the page takes a long time :)

This app was mostly developed so I could learn about the [CosmosDB Graph API](https://docs.microsoft.com/en-us/azure/cosmos-db/graph-introduction), [NSubstitute](http://nsubstitute.github.io/), and [ASP.Net Core](https://docs.microsoft.com/en-us/aspnet/core/).

## Development

### TMDB

This project requires an API key from [The Movie Database](https://www.themoviedb.org).

1. Register for TMDB and request API access.
2. Find the **version 3** API key in your account settings.
3. Create an environment variable named _TmdbV3ApiKey_ and set its value to your version 3 API key.

### Cosmos DB

This project uses CosmosDB as the data store.

1. Install and start the CosmosDB emulator by following [these instructions](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator).
2. Open the solution in Visual Studio 2017. Your mileage may vary with other development environments (Rider, VSCode).
3. Run the integration tests. This will create a database and a test collection if everything is working correctly.
4. Run the website. This will create the database if it doesn't already exist and also the collection.
5. Put in Tom Cruise and Cameron Diaz. You'll see that they worked together on Vanilla Sky.

## Contributing

Pull requests are welcome, if unlikely :-).