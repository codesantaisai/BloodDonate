using BloodDonate.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodDonate.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private static EmployeeDbContext _context;
    public EmployeeController(EmployeeDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        if(_context.Employees == null)
        {
            return NotFound();
        }

        return await _context.Employees.ToListAsync();
    }

    [HttpGet("Employee")]

    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
        if (employee==null)
        {
            return BadRequest("Invalid Id");
        }

        return employee;
    }

    [HttpPost]

    public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
    {
       await _context.Employees.AddAsync(employee);
       await _context.SaveChangesAsync();
       return CreatedAtAction(nameof(GetEmployee),new{id= employee});
    }
    
    [HttpPut("{id}")]

    public async Task<ActionResult> PutEmployee(int id, Employee employee)
    {
        if (id!= employee.Id)
        {
            return BadRequest();
        }

        _context.Entry(employee).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch ( DbUpdateConcurrencyException e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Ok();

    }

    [HttpDelete]
    public async Task<ActionResult<Employee>> DeleteEmployee( int id)
    {
        var result = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
        if (result == null)
        {
            return NotFound();
        }
        _context.Employees.Remove(result);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}