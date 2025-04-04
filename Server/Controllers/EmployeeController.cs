using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly EmployeeContext _employeecontext; 
        public EmployeeController(EmployeeContext employeeContext)
        {
            _employeecontext = employeeContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
           if(_employeecontext.Employees == null)
            {
                return NotFound();
            }
            return await _employeecontext.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            if (_employeecontext.Employees == null)
            {
                return NotFound();
            }
            var employee = await _employeecontext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            
            return employee;
        }


        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            if (_employeecontext.Employees == null)
            {
                return Problem("Entity set 'EmployeeContext.Employees' is null.");
            }

            employee.Id = 0;

            _employeecontext.Employees.Add(employee);
            await _employeecontext.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            _employeecontext.Entry(employee).State = EntityState.Modified;

            try
            {
                await _employeecontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               
                    throw;
                
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_employeecontext.Employees == null)
            {
                return NotFound();
            }
            var employee = await _employeecontext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _employeecontext.Employees.Remove(employee);
            await _employeecontext.SaveChangesAsync();

            return Ok();
        }
    }
}

