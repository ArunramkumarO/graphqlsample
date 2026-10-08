using EmployeeGraphQLDemo.Data;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace EmployeeGraphQLDemo.GraphQL;

// WHAT: the root "Query" type. Every field here is an entry point for READING data.
// WHY:  a GraphQL schema must have a Query type.
// HOW:  each field has a *resolver* (the lambda). GraphQL.NET calls it only if the client
//       asked for that field, then picks the requested sub-fields from the result.
// EF CORE: the schema lives for the whole app, but AppDbContext is per-request (scoped),
//       so each resolver fetches it from context.RequestServices.
public class EmployeeQuery : ObjectGraphType
{
    public EmployeeQuery()
    {
        Name = "Query";
        Description = "Read operations.";

        // query { employees { id name } }
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<EmployeeType>>>>("employees")
            .Description("Get all employees.")
            .ResolveAsync(async context =>
            {
                var db = context.RequestServices!.GetRequiredService<AppDbContext>();
                return await db.Employees.ToListAsync();
            });

        // query { employee(id: 1) { name } }   -> returns null if not found
        Field<EmployeeType>("employee")
            .Description("Get one employee by id. Returns null if not found.")
            .Argument<NonNullGraphType<IntGraphType>>("id", "The employee id.")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                var db = context.RequestServices!.GetRequiredService<AppDbContext>();
                return await db.Employees.FindAsync(id);
            });
    }
}
