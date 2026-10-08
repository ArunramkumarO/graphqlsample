using EmployeeGraphQLDemo.Data;
using EmployeeGraphQLDemo.Models;
using GraphQL;
using GraphQL.Types;

namespace EmployeeGraphQLDemo.GraphQL;

// WHAT: the root "Mutation" type. Fields here CHANGE data (create / update / delete).
// WHY:  GraphQL separates reads (Query) from writes (Mutation) by convention and by keyword.
// HOW:  same as Query: a field + arguments + resolver. The resolver writes through EF Core
//       and returns the saved object, so the client can select fields from the result.
public class EmployeeMutation : ObjectGraphType
{
    public EmployeeMutation()
    {
        Name = "Mutation";
        Description = "Write operations.";

        Field<EmployeeType>("createEmployee")
            .Description("Create a new employee and return it (including the generated id).")
            .Argument<NonNullGraphType<StringGraphType>>("name", "Full name.")
            .Argument<NonNullGraphType<StringGraphType>>("email", "Work email address.")
            .Argument<NonNullGraphType<StringGraphType>>("department", "Department, e.g. IT.")
            .Argument<NonNullGraphType<StringGraphType>>("designation", "Job title.")
            .Argument<NonNullGraphType<DecimalGraphType>>("salary", "Annual salary.")
            .ResolveAsync(async context =>
            {
                var db = context.RequestServices!.GetRequiredService<AppDbContext>();
                var employee = new Employee
                {
                    Name = context.GetArgument<string>("name"),
                    Email = context.GetArgument<string>("email"),
                    Department = context.GetArgument<string>("department"),
                    Designation = context.GetArgument<string>("designation"),
                    Salary = context.GetArgument<decimal>("salary"),
                };
                db.Employees.Add(employee);
                await db.SaveChangesAsync(); // fills in employee.Id
                return employee;
            });

        // Only "id" is required; any other argument the client omits is left unchanged.
        Field<EmployeeType>("updateEmployee")
            .Description("Update an employee. Only the arguments you pass are changed. Returns null if the id doesn't exist.")
            .Argument<NonNullGraphType<IntGraphType>>("id", "Id of the employee to update.")
            .Argument<StringGraphType>("name", "New name (optional).")
            .Argument<StringGraphType>("email", "New email (optional).")
            .Argument<StringGraphType>("department", "New department (optional).")
            .Argument<StringGraphType>("designation", "New job title (optional).")
            .Argument<DecimalGraphType>("salary", "New salary (optional).")
            .ResolveAsync(async context =>
            {
                var db = context.RequestServices!.GetRequiredService<AppDbContext>();
                var employee = await db.Employees.FindAsync(context.GetArgument<int>("id"));
                if (employee is null) return null;

                if (context.HasArgument("name")) employee.Name = context.GetArgument<string>("name");
                if (context.HasArgument("email")) employee.Email = context.GetArgument<string>("email");
                if (context.HasArgument("department")) employee.Department = context.GetArgument<string>("department");
                if (context.HasArgument("designation")) employee.Designation = context.GetArgument<string>("designation");
                if (context.HasArgument("salary")) employee.Salary = context.GetArgument<decimal>("salary");

                await db.SaveChangesAsync();
                return employee;
            });

        // Returns true if something was deleted, false if the id didn't exist.
        Field<NonNullGraphType<BooleanGraphType>>("deleteEmployee")
            .Description("Delete an employee. Returns true if deleted, false if the id doesn't exist.")
            .Argument<NonNullGraphType<IntGraphType>>("id", "Id of the employee to delete.")
            .ResolveAsync(async context =>
            {
                var db = context.RequestServices!.GetRequiredService<AppDbContext>();
                var employee = await db.Employees.FindAsync(context.GetArgument<int>("id"));
                if (employee is null) return false;

                db.Employees.Remove(employee);
                await db.SaveChangesAsync();
                return true;
            });
    }
}
