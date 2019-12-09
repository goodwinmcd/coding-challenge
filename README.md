## Hosting
I chose the backend focused track.

The app is hosted with Heroku here: https://ambassador-referral-api.herokuapp.com/

That is the base url. All API endpoints have /api/referrals appended to them.

Swagger docs are located here for more information on the endpoints: https://ambassador-referral-api.herokuapp.com/swagger.

A postman suite for all endpoints is provided in the repository under the Postman folder.

## Running locally
The application is dockerized and I have provided a docker-compose.yaml file.

Running `docker-compose build` and `docker-compose up` should be enough to get the app running. This
will also spin up a postgres database and initialize a referrals table. The postgres connection string
environment variable will be overriden to connect to the database instance defined in the docker-compose file.
You'll need to change this if there is a different database instance you would like to connect to.

You can also run the app using the dotnet cli with `dotnet build` and `dotnet run`. There are
several different database connection strings in the launch settings file depending on whether
you want to connect to a local, docker hosted, or heroku hosted database.

## The application

I wrote the application in C# using the latest version of the .net core framework.

The application architecture is straight forward. It has a controller, service (AKA logic), and
repository layer. There is a single controller with a corresponding service and repo for
performing CRUD operations on a referral. The service layer prevents the controllers from getting
bloated as features are added to the application. The repository layer is only concerned with
database actions. The service layer also prevents the repo layer from becoming bloated with
business logic in the future. This separation of concerns allows the app to be more maintainable
in the future.

The application was simple enough to not use any advanced design patterns. IOC/DI was the only
notable pattern I used.

I created two separate endpoints for retrieving multiple referrals. One is pagenated to be able
to scale performance in the future and the other one gets all referrals at once. I thought that for demonstration
and testing purposes that this endpoint would be convenient to have.

I was not sure if consumers should have access to referral counts. I went ahead and made the
assumption that they shouldn't. That is why there are two separate edit endpoints. One of the
PUT endpoints increments the count of a referral and the other one changes the title. This
prevents someone from being able to falsify referral counts.

Note that the db connection is injected on the service layer even though it is never used there.
This is so that if a service would need to do multiple DB request then it would not trigger a transaction
for each one. There would only be a single transaction done until the service method is complete.

I used dapper as my ORM because I like the control it gives you over writing the
SQL vs EF Core that *tries* to do all the work for you but usually ends up
making everything worse.

I provided decent test coverage of the controller and service layer. I did not write unit test for the
repository layer because of the overhead in mocking a db and the value it would provide. I would usually
right integration test to cover the end to end usage of external services like that.

I checked the database connection string into source control. If this was a production environment
situation then I would not have done that.

I accidently mispelled referral with only one r at the start of the project. Whoops!

## Other things to consider

Metrics!! I would have setup metrics next. I have previously configured
Prometheus to collect endpoint data such as execution time in the past. I've connected
a Grafana instance to Prometheus to visualize that data. This has proven very
valuable in the past to identify bottle necks and monitor progress of our code
base overtime.

Logging! I would've set up a log aggregator such as graylog. I have had trouble in the
past troubleshooting issues because of unorganized logs and huge dumps of log information
from multiple service. I would make sure to maintain a structured log foundation going
forward to save myself time in the past.

I mention those because I have seen them generally overlooked in the past but found to be
extremely helpful when used.

## Conclusions

Fun project. I could see how something like this could expand into something more. Having more data
on the referrals would be really interesting.

Here is another API I build recently for finding anagrams: https://gitlab.com/goodwinmcd/anagram-api

The readme goes into a lot of technical details of the decisions that I made and the route I took for
finding anagrams in the most efficient way.

Here is a small service that I made for scraping an imdb id based off a movie title and release year: https://gitlab.com/goodwinmcd/movie-data

I plan on making that scraper a part of a larger project for collecting movie data. I ultimately will deploy a model
that leverages that data for predicting movie ratings from rotten tomatoes, imdb, and meta critic.

I plan to use this scraper/ML Model as my practicum for my masters degree.