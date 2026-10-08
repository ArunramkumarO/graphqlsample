namespace EmployeeGraphQLDemo.Models;

/// <summary>A person who works at the company.</summary>
// Plain C# class. EF Core maps it to a table; GraphQL describes it via EmployeeType.
public class Employee
{
    /// <summary>Database-generated id. Ignored on create.</summary>
    public int Id { get; set; }

    /// <summary>Full name.</summary>
    public string Name { get; set; } = "";

    /// <summary>Work email address.</summary>
    public string Email { get; set; } = "";

    /// <summary>Department, e.g. IT, HR, Finance.</summary>
    public string Department { get; set; } = "";

    /// <summary>Job title, e.g. Developer.</summary>
    public string Designation { get; set; } = "";

    /// <summary>Annual salary.</summary>
    public decimal Salary { get; set; }
}
