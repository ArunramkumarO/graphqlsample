using GraphQL.Types;

namespace EmployeeGraphQLDemo.GraphQL;

// WHAT: the schema = the complete contract of the API: "here is the Query root, here is the Mutation root".
// WHY:  GraphQL.NET executes every request against a single ISchema. Everything reachable
//       (types, fields, arguments) is discovered by walking from these two roots.
// HOW:  GraphQL.NET creates this class through DI; the IServiceProvider lets it build
//       EmployeeQuery / EmployeeMutation (registered in Program.cs via AddGraphTypes).
public class EmployeeSchema : Schema
{
    public EmployeeSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<EmployeeQuery>();
        Mutation = serviceProvider.GetRequiredService<EmployeeMutation>();
    }
}
