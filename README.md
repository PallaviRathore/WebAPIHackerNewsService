# WebAPIHackerNewsService
Using ASP.NET Core, implement a RESTful API to retrieve the details of the best n stories from the Hacker News API, as determined by their score, where n is specified by the caller to the API.

How to run the Application 
==================================

Open .Net CLI and Navigate to project directory: 
cd WebAPIHackerNewsService 

Build the project: 
dotnet build 

if the build is sucess Run the project using cmd : 
dotnet run 

The API will be accessible at https://localhost:5001 (or a different URL if configured differently).

Usage
========================
You can use any HTTP client, such as curl or a tool like Swagger, to interact with the API. Here's an example of how to retrieve the best 10 stories:

https://localhost:7136/api/HackerNews/best-stories/10
This will return the best 10 stories in descending order of score in JSON format.

API Endpoints
========================
GET /api/HackerNews/best-stories/{n}: Retrieve the best n stories based on their score.

Parameters:
n (required): The number of stories to retrieve.
Example:
https://localhost:7136/api/HackerNews/best-stories/10

Precautions
========================
The API endpoints (https://hacker-news.firebaseio.com/v0/beststories.json and https://hacker-news.firebaseio.com/v0/item/{storyId}.json) are assumed to be accessible.
The API is configured to run on the default URL https://localhost:5001. You can adjust this in the launchSettings.json or by setting environment variables as needed.

# FutureChanges

Would have added ReddisCache or SQL Server Chache if required.
In a multi-threaded environment, fetching and storing data in the cache could lead to race conditions. We could think about adding lock to ensure that only one instance of the application updates the cache at a time.
