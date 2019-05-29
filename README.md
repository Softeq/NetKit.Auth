![Azure DevOps builds](https://img.shields.io/azure-devops/build/SofteqDevelopment/NetKit/20.svg)

# Softeq.Netkit.Auth


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

## Contributing

We welcome any contributions.

## License

The Query Utils project is available for free use, as described by the [LICENSE](/LICENSE) (MIT).