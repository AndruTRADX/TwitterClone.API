# Twitter Clone API Documentation

## **Overview**

Welcome to the Twitter Clone API documentation 😋! This API mimics the functionality and features of Twitter (now known as X), allowing users to create tweets, comment on tweets, like tweets, and view user profiles.

## **What is Twitter Clone API?**

The Twitter Clone API is built using C# and .NET technologies to provide a reliable, scalable, and secure platform for users to interact with each other similar to how they would on Twitter.

## **Features**

- **Tweet Management**: Users can create new tweets and interact with existing tweets by commenting and liking them.
- **User Profiles**: Users can view other users' profiles to see their tweets and interactions.
- **Scalability**: The API is designed to handle a growing number of users and interactions efficiently.
- **Security**: Built with security best practices in mind to protect user data and ensure privacy.

## **Deployment**

This project is deployed on Azure, leveraging seamless integration with Microsoft technologies for robust performance and scalability.

- **Root API Endpoint**: [app-twitterclone-eastus-dev-001.azurewebsites.net](https://app-twitterclone-eastus-dev-001.azurewebsites.net/)

## **Database**

The API utilizes a SQL Server Database to store all the necessary information., ensuring data integrity and reliability.

## **Developer**

This API was developed with 💜 by AndruTRADX (myself). You can explore more about me and my work on my [portfolio website](https://andrutradx.vercel.app/), where you can also find ways to connect with me and provide valuable feedback on the project.

# Endpoints

Those will be all the paths that we project is able to support, I will explain each functionality further. These will be divided in several kinds of request such as:

## **Authentication**

Let's dive into the endpoints supported by our Twitter Clone API. Each endpoint serves a specific purpose and supports various HTTP request methods.

- **POST: /api/auth/register**
    - Registers a new user account in the app.
    - Request Body Schema:
        
        ```json
        {
          "firstName": "string",
          "lastName": "string",
          "biography": "string",
          "userName": "string", // Must be unique
          "email": "user@example.com", // Must be unique
          "password": "string"
        }
        ```
        
- **POST: /api/auth/login**
    - Logs a user into their account and returns a JWT (JSON Web Token) for session management.
    - Request Body Schema:
        
        ```json
        {
          "email": "user@example.com",
          "password": "string"
        }
        ```
        

## **Tweets**

Endpoints for managing tweets within the application.

- **GET: /api/tweets**
    - Retrieves tweets stored in the database.
    - Parameters:
        - **`Size`**: Number of tweets to retrieve (default = 10).
        - **`Offset`**: Page number for pagination (default = 1).
- **GET: /api/tweets/{tweetId}**
    - Retrieves a specific tweet by its unique **`tweetId`** (Guid value).
- **[Authorized] POST: /api/tweets**
    - Creates a new tweet.
    - Requires user authentication with a valid JWT sent in the authorization header.
    - Request Body Schema:
        
        ```json
        {
          "content": "string"
        }
        ```
        
- **[Authorized] PATCH: /api/tweets/{tweetId}**
    - Edits an existing tweet identified by **`tweetId`**.
    - Requires user authentication with a valid JWT sent in the authorization header.
    - Request Body Schema:
        
        ```json
        {
          "content": "string"
        }
        ```
        
    - Note: Only the creator of the tweet can edit it; unauthorized attempts will receive a generic response.
- **[Authorized] DELETE: /api/tweets/{tweetId}**
    - Deletes a tweet identified by **`tweetId`**.
    - Requires user authentication with a valid JWT sent in the authorization header.
    - Note: Only the creator of the tweet can delete it; unauthorized attempts will receive a generic response.

## **Comments**

The Comments endpoint manages interactions with comments on tweets. Comments cannot be edited due to business logic requirements.

- **GET: /api/comments/{tweetId}**
    - Retrieves the last 10 comments of a specific tweet.
    - Parameters:
        - **`pageNumber`**: Page number for pagination (default = 1).
        - **`pageSize`**: Number of comments to retrieve (default = 10).
- **[Authorized] POST: /api/comments/{tweetId}**
    - Adds a new comment to a tweet identified by **`tweetId`** (Guid value).
    - Requires user authentication with a valid JWT sent in the authorization header.
    - Request Body Schema:
        
        ```json
        {
          "content": "string"
        }
        ```
        
- **[Authorized] DELETE: /api/comments/{commentId}**
    - Deletes a comment identified by **`commentId`**.
    - Requires user authentication with a valid JWT sent in the authorization header.
    - Note: Only the creator of the comment can delete it; unauthorized attempts will receive a generic response.

## **Likes**

The Likes endpoint manages interactions related to liking tweets.

- **[Authorized] POST: /api/likes/{tweetId}**
    - Likes or unlikes a tweet identified by **`tweetId`** (Guid value). This endpoint acts as a toggle; if the user has already liked the tweet, the like will be removed.
    - Requires user authentication with a valid JWT sent in the authorization header.

## **LikesToComments**

The LikesToComments endpoint handles liking comments.

- **[Authorized] POST: /api/likestocomments/{commentId}**
    - Likes or unlikes a comment identified by **`commentId`** (Guid value). Similar to the Likes endpoint, this feature toggles the like status.
    - Requires user authentication with a valid JWT sent in the authorization header.

## **Profile**

The Profile endpoint provides functionalities related to user profiles within the API. Enabling users to retrieve and manage their profiles securely.

- **GET: /api/profile/{userName}**
    - Retrieves the profile of a user based on their unique **`userName`** (which serves as the identifier).
- **GET: /api/profile/tweets/{tweetId}**
    - Retrieves the last 10 tweets of a specific user.
    - Parameters:
        - **`pageNumber`**: Page number for pagination (default = 1).
        - **`pageSize`**: Number of tweets to retrieve (default = 10).
- **GET: /api/profile/{search}**
    - Searches for users in the database based on the provided search parameter.
    - Conducts a weighted search using the following parameters:
        - **`UserName`** (most important)
        - **`FirstName`** (second most important)
        - **`LastName`** (less important but still considered)
- **[Authorized] POST: /api/profile**
    - Edits the profile information of the logged-in user.
    - Request body schema:
        
        ```csharp
        {
          "firstName": "string",
          "lastName": "string",
          "biography": "string",
          "userName": "string",
          "email": "user@example.com"
        }
        ```
        
    - Requires user authentication with a valid JWT sent in the authorization header.

## **JWT Information**

If you see on the **authorized required** endpoints that even though we do not sent some kind of information the application is able to get info from the user is because the JWT stores the following information of the user.

### **JWT Contents:**

The JWT (JSON Web Token) used for authorization carries specific user information, which is utilized by authorized endpoints even if not explicitly provided in requests.

- **`UserId`**: This unique identifier is crucial for internal application validations and authorization logic but is never exposed to users.
- **`UserName`**: Serves as the primary identifier for users within the application without compromising security. Users can use this identifier to interact with their own accounts and identify others.
- **`FirstName`**: This information is included to personalize the display of user names in tweets and comments.

The JWT ensures that authorized actions are performed only by authenticated users while protecting sensitive information.

## **Closing**

Thank you for exploring the Twitter Clone API documentation! We've covered a range of functionalities that empower users to interact and connect within our platform. Whether you're creating tweets, commenting, liking, or managing your profile, our API is designed to provide a seamless and secure experience.

We encourage you to leverage the endpoints and features offered by our API. Your feedback is invaluable to us and can be shared through my [portfolio website](https://andrutradx.vercel.app/). Let's continue to build and innovate together!

Happy tweeting and exploring! 😊🚀
