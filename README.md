# APIGateway-Ocelot

![image](https://github.com/Chamaxonline/APIGateway-Ocelot/assets/14096914/b23cb1c6-0e24-45ab-ba37-9fa2504a307e)

## Step 1: Setting up the Microservices

Begin by creating a new ASP.NET Core Solution in Visual Studio and name it “Microservices.WebApi”. Inside this solution, create a folder named “Microservices” and add two new projects to it: “Product.Microservice” and “Customer.Microservice”. These projects will serve as our microservices for handling product and customer-related operations, respectively.

For simplicity, let’s assume that you have already set up the CRUD operations and Swagger for these microservices using Entity Framework Core for data access.

In the root of the Solution, add new ASP.NET Core Project and name it “Gateway.WebApi”. This will be an Empty Project, as there will be not much things inside this gateway.

This is how your project structure should look like:

![image](https://github.com/Chamaxonline/APIGateway-Ocelot/assets/14096914/756a78d8-c095-47d1-b291-87ee436143b2)

## Step 2: Introducing Ocelot API Gateway
Ocelot is an open-source API Gateway for the .NET/Core platform that allows us to unify multiple microservices under a single domain, providing a clean separation between the client and microservices. It acts as a gateway for incoming requests from the client and forwards them to the appropriate microservices.

To get started with Ocelot, install the Ocelot NuGet package in the “Gateway.WebApi” project.

## Step 3: Configuring Ocelot Routes
Create a new JSON file named “ocelot.json” at the root of the “Gateway.WebApi” project. This file will contain the configurations needed for Ocelot.

In the “ocelot.json” file, we will define routes for the “Product.Microservice” and “Customer.Microservice” endpoints. For example:
```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/product",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 44337
        }
      ],
      "UpstreamPathTemplate": "/gateway/product",
      "UpstreamHttpMethod": [ "POST", "PUT", "GET" ]
    },
    {
      "DownstreamPathTemplate": "/api/customer",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 44338
        }
      ],
      "UpstreamPathTemplate": "/gateway/customer",
      "UpstreamHttpMethod": [ "POST", "PUT", "GET" ]
    }
  ]
}
```

The JSON configuration provided is used to set up routing rules for the Ocelot API Gateway. This gateway is responsible for managing incoming client requests and routing them to appropriate microservices. It helps to simplify the client-side interactions and hides the complexities of the microservice architecture.

In this configuration, there are two routes defined, one for the “Product.Microservice” and the other for the “Customer.Microservice.”

Let’s take a closer look at the configuration for the “Product.Microservice” route:

**1.DownstreamPathTemplate**: This setting defines the path template for the downstream service, which is the “Product.Microservice” in this case. When a client sends a request to the API Gateway with the path “/gateway/product,” Ocelot will forward that request to the “Product.Microservice” with the path “/api/product”.

**2.DownstreamScheme**: This indicates the scheme to be used for the downstream request. In this example, it is set to “https,” which means the communication with the “Product.Microservice” will be over HTTPS.

**3.DownstreamHostAndPorts**: Here, we specify the host and port information of the “Product.Microservice”. In this case, the “Product.Microservice” is running on the localhost at port 44337. So, any request from the API Gateway will be forwarded to the “Product.Microservice” at “https://localhost:44337".

**4.UpstreamPathTemplate**: This setting defines the path template for the upstream request, which is the request made by the client to the API Gateway. When a client sends a request to the API Gateway with the path “/gateway/product,” Ocelot will map it to the “Product.Microservice” with the path “/api/product”.

**5.UpstreamHttpMethod**: This specifies the supported HTTP methods for this route. In this case, the “Product.Microservice” supports POST, PUT, and GET requests.
The configuration for the “Customer.Microservice” route is similar, but with different paths and port numbers specific to the “Customer.Microservice.”

With this configuration in place, the Ocelot API Gateway will be able to handle incoming client requests, forward them to the appropriate microservices, and hide the complexities of the microservice architecture from the clients.

## Step 4: Modify the program.cs
we need to modify the  Gateway.WebApi program.cs class like below(I used .NET 6.0 application)

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("ocelot.json",optional:false,reloadOnChange:true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
await app.UseOcelot();

app.Run();
```

## Step 5: Running the Application
Build the solution to ensure there are no errors. Now, run the solution and navigate to “localhost:5170/gateway/product” to access the product-related endpoints via the API Gateway. Similarly, you can access customer-related endpoints via “localhost:5170/gateway/customer.”

reference : https://medium.com/cloud-native-daily/microservice-architecture-in-asp-net-87a0994483f4
