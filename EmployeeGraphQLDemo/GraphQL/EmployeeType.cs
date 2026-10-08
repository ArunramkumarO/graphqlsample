using EmployeeGraphQLDemo.Models;
using GraphQL.Types;

namespace EmployeeGraphQLDemo.GraphQL;

// WHAT: describes the C# Employee class as a GraphQL *type* (the "Employee" type in the schema).
// WHY:  GraphQL has its own type system. A client can only ask for fields listed here.
// HOW:  GraphQL.NET reads these Field(...) lines to build the schema, and for each field the
//       client requests, it reads the matching property off the Employee object that
//       EF Core loaded. (No resolver needed: Field(x => x.Name) is a "property resolver".)
// DESCRIPTIONS: every Description(...) below is exposed through introspection, so it shows up
//       in the Docs panel of GraphiQL.
public class EmployeeType : ObjectGraphType<Employee>
{
    public EmployeeType()
    {
        Name = "Employee";
        Description = "A person who works at the company.";

        // Non-nullable C# types become non-null GraphQL types (e.g. Int!, String!).
        Field(x => x.Id).Description("Database-generated id.");
        Field(x => x.Name).Description("Full name.");
        Field(x => x.Email).Description("Work email address.");
        Field(x => x.Department).Description("Department, e.g. IT, HR, Finance.");
        Field(x => x.Designation).Description("Job title, e.g. Developer.");
        Field(x => x.Salary).Description("Annual salary.");
    }
}
