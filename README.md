![Azure DevOps builds](https://dev.azure.com/SofteqDevelopment/NetKit/_apis/build/status/Auth/Auth-CI-Build)

# Softeq.Netkit.Auth

Open source product is used to communicate people in real time. 
This project uses ASP.NET Core 2.2

## About

This project is maintained by Softeq Development Corp.

We specialize in .NET core applications.

## Get started

Initially, there are created 2 default roles and users in the database:

```json
{
  "Roles": [
    "User",
    "Admin"
  ],
  "Users": [
    {      
      "Email": "user@test.test",
      "Password": "123QWqw!",
      "Role": "User"      
    },
    {      
      "Email": "admin@test.test",
      "Password": "123QWqw!",
      "Role": "Admin",      
    }
  ]
}
```

To change initial data edit SeedValues.json file

## Use

To receive access and refresh token by user credentials send the following request:
POST /connect/token
```csharp
var values = new Dictionary<string, string>
{
	{ "grant_type", "password" },
	{ "password", "userPassword" },
	{ "username", "userName" },
	{ "scope", "offline_access" },
	{ "client_id", "clientId" },
	{ "client_secret", "clientSecret" }
};

var content = new FormUrlEncodedContent(values);
var response = await httpClient.PostAsync($"{authServerUrl}/connect/token", content);
```

To receive access and refresh token by refresh token send the following request:
POST /connect/token
```csharp
var values = new Dictionary<string, string>
{
	{ "grant_type", "refresh_token" },
	{ "scope", "offline_access" },
	{ "client_id", "clientId" },
	{ "client_secret", "clientSecret" },
	{ "refresh_token", "refreshToken" }
};

var content = new FormUrlEncodedContent(values);
var response = await httpClient.PostAsync($"{authServerUrl}/connect/token", content);
```

## Contributing

We welcome any contributions.

## License

The Query Utils project is available for free use, as described by the [LICENSE](/LICENSE) (MIT).