using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api_UseCase.Repository;
using Api_UseCase.Models;

namespace Api_UseCase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {

        IServiceApi serviceapi;
        public EmployeeController(IServiceApi _serviceapi)
        {
            serviceapi = _serviceapi;
        }
        [HttpGet()]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            
            try
            {
                var employees = await serviceapi.GetEmployees();
                if (employees == null)
                {
                    return NotFound();
                }

                return Ok(employees);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var empId = await serviceapi.AddEmployee(model);
                    if (empId == 0)
                    {
                        return Ok("Only one employee with no manager is allowed. Kinldy provide a manager ID.");

                    }
                    else if (empId != 0 || empId != 1)
                    {
                        return Ok("Employee created successfully. Email has been sent to the user.");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception)
                {

                    return BadRequest();
                }

            }

            return BadRequest();
        }
        

        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string message =await serviceapi.UpdateEmployee(model);
                   
                    return Ok(message);
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName ==
                             "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int? empId)
        {
            int result = 0;

            if (empId == null)
            {
                return BadRequest();
            }

            try
            {
                result = await serviceapi.DeleteEmployee(empId);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok("Employee record deleted successfully. ");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet]
        [Route("GetEmployeeStatus")]
        public async Task<IActionResult> GetEmployeeStatus(int? empId)
        {
            if (empId == null)
            {
                return BadRequest();
            }

            try
            {
                var post = await serviceapi.EmployeeStatus(empId);

                if (post == null)
                {
                    return NotFound();
                }

                return Ok("Employee is "+post+" in this company.");
            }
            catch (Exception ex)
            {
                string x = ex.Message;
                return BadRequest();
            }
        }

    }
}
