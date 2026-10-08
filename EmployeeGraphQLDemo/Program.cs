using EmployeeGraphQLDemo.Data;
using EmployeeGraphQLDemo.GraphQL;
using GraphQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQL Server. Scoped = one AppDbContext per HTTP request.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// REST API + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Employee API", Version = "v1", Description = "REST CRUD for employees. The same data is available via GraphQL at /graphql." });
    // Show the /// comments from the controller and model in Swagger UI.
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "EmployeeGraphQLDemo.xml"));
});

// GraphQL.NET
builder.Services.AddGraphQL(graphql => graphql
    .AddSchema<EmployeeSchema>()                       // register our schema as ISchema/EmployeeSchema
    .AddSystemTextJson()                               // serialize requests/responses with System.Text.Json
    .AddGraphTypes(typeof(EmployeeSchema).Assembly));  // register EmployeeType, EmployeeQuery, EmployeeMutation

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(); // /swagger

app.MapControllers();

app.UseGraphQL<EmployeeSchema>("/graphql");  // the GraphQL endpoint (POST/GET)
app.UseGraphQLGraphiQL("/graphiql");         // browser UI to try queries

app.Run();
