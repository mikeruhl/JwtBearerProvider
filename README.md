# JwtBearerProvider
JwtBearer Token Provider Demo

Quick Readme for now...

### Description
This project shows how to provide jwt tokens from a /token endpoint in asp.net core 2.  By default, it uses the provided Identity.EntityFramework stuff.  It uses the InMemory database provider to start with and creates 2 users: user/password123 and AdminMaster/password123.


### Setup
Open in visual studio and debug.  This is not production-ready code and should not be used in production.  To use in production, the secret should be stored somewhere else and the audience and issuer needs to be changed (and stored somewhere else).  Also, you will want to use a real store for your Database Context and not the InMemory one (which is not production-ready).


### Examples

Example of getting a token:

`POST /token
{
"grant_type": "refresh_token",
"refresh_token":"AdminMaster",
"client_secret":"password123"
}`

Example of refreshing a token:

`POST /token
{
"grant_type": "refresh_token",
"refresh_token":"2EmXfptSTWLhagfgKYpqulTGyhu9JzJf3oFS6o1T2A5"
}`

Example Reply:

`
{
    "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImViMDA0YWQwLTExOTgtNDAxOC1hYzAxLWU0ZDI3Y2JjMTY4MSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJBZG1pbk1hc3RlciIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiWEtUS1dFUUpPRVJIVUdKV0ZKTElSWTJWSlBFVkRBWjciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsIm5iZiI6MTUyNzY4Nzc5OCwiZXhwIjoxNTI3Njg4MDk4LCJpc3MiOiJtaWtlLm1pa2UubWlrZSIsImF1ZCI6Im1pa2UubWlrZSJ9.u6oFMrQbfk1s6i5uUMeNDcjULoX8bt54FXTGj2GNSAs",
    "expires_in": 300,
    "refresh_token": "1PDiWbhrCgRMUZWQjpMBTFZkWjyuRX7gIBbV2lyR8uL"
}`

There are 2 demo api endpoints:

/api/user

/api/admin

Add the Header: Authorization: Bearer [token] to hit these endpoints.


/api/user both users can hit once authorized.  It will return the username of the authorized user.

/api/admin only users with "admin" role can hit it.  It will tell you if you're admin or not.

### Tests
No tests, this was done quickly for an example for a friend.  Sorry!
