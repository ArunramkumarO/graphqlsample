using EmployeeGraphQLDemo.Data;
using EmployeeGraphQLDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeGraphQLDemo.Controllers;

/// <summary>Create, read, update and delete employees.</summary>
// The REST way: one URL per operation, the server decides which fields come back.
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _db;

    public EmployeesController(AppDbContext db) => _db = db;

    /// <summary>Get all employees.</summary>
    /// <returns>Every employee, with all fields.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Employee>>> GetAll() =>
        await _db.Employees.ToListAsync();

    /// <summary>Get one employee by id.</summary>
    /// <param name="id">The employee id.</param>
    /// <response code="200">The employee.</response>
    /// <response code="404">No employee with that id.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        return employee is null ? NotFound() : employee;
    }

    /// <summary>Create a new employee.</summary>
    /// <param name="employee">The employee to create. Any Id sent is ignored.</param>
    /// <response code="201">The created employee, with its generated id.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Employee>> Create(Employee employee)
    {
        employee.Id = 0; // let the database generate the id
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    /// <summary>Replace an employee's data.</summary>
    /// <param name="id">The employee id.</param>
    /// <param name="input">New values for all fields.</param>
    /// <response code="204">Updated.</response>
    /// <response code="404">No employee with that id.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, Employee input)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee is null) return NotFound();

        employee.Name = input.Name;
        employee.Email = input.Email;
        employee.Department = input.Department;
        employee.Designation = input.Designation;
        employee.Salary = input.Salary;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Delete an employee.</summary>
    /// <param name="id">The employee id.</param>
    /// <response code="204">Deleted.</response>
    /// <response code="404">No employee with that id.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee is null) return NotFound();

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
