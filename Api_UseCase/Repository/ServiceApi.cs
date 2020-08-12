using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_UseCase.Models;
using Api_UseCase.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_UseCase.Repository
{
    public class ServiceApi : IServiceApi
    {
        WB_UseCaseContext db;
        public ServiceApi(WB_UseCaseContext _db)
        {
            db = _db;
        }

        public bool CheckNullManager()
        {
            var x = (from p in db.Employee
                     where p.ManagerId == null
                     select p);
            if (x != null)
                return true;
            else
                return false;

        }
        public async Task<List<EmployeeDetails>> GetEmployees()
        {
            if (db != null)
            {
                return await (from p in db.Employee
                              join c in db.Department

                              on p.DeptId equals c.DeptId
                              join s in db.Employee
                              on p.ManagerId equals s.EmpId
                              select new EmployeeDetails
                              {
                                  EmpId = p.EmpId,
                                  FirstName = p.FirstName,
                                  LastName = p.LastName,
                                  EmailId = p.EmailId,
                                  Department = c.DepartmentName,
                                  Manager = s.FirstName + ' ' + s.LastName
                              }).ToListAsync();
            }

            return null;
        }
        public async Task<int> AddEmployee(Employee employee)
        {
            if (db != null)
            {
                if (employee.ManagerId == null)
                {
                    bool x = CheckNullManager();
                    if (x == true)
                        return 0;
                }
                else
                {
                    await db.Employee.AddAsync(employee);
                    await db.SaveChangesAsync();
                    return employee.EmpId;
                }
            }

            return 1;
        }

        public async Task<string> UpdateEmployee(Employee employee)
        {
            if (db != null)
            {
                if (employee.ManagerId == null)
                {
                    bool status = CheckNullManager();
                    if (status == true)
                        return "Only one employee is allowed with no manager. Kindly provide a manager ID.";
                }
                else
                {
                    //Delete that post
                    db.Employee.Update(employee);

                    //Commit the transaction
                    await db.SaveChangesAsync();
                    return "Employee details updated Succesfully.";
                }
                return "Error updating employee details.";
            }
            else
                return "Error updating employee details.";
        }

        public async Task<int> DeleteEmployee(int? empid)
        {
            int result = 0;

            if (db != null)
            {
                //Find the post for specific post id
                var post = await db.Employee.FirstOrDefaultAsync(x => x.EmpId == empid);

                if (post != null)
                {
                    //Delete that post
                    db.Employee.Remove(post);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }
        
        public async Task<string> EmployeeStatus(int? empid)
        {
            //int result = 0;
            string level=null;
            if (db != null)
            {
               
                //Find the post for specific post id
                var post = await db.Employee.FirstOrDefaultAsync(x => x.EmpId == empid);

                if (post != null)
                {
                   // string sqlQuery = "EXEC [dbo].[GetProductByPriceGreaterThan1000] @EmpID = {0}";
                    var x = db.Employee.FromSqlRaw(" execute dbo.GetEmployeeLevel @EmpID = {0}",empid).IgnoreQueryFilters();
                    level = x.FirstOrDefault().EmployeeStatus;
                    
                }
                return level;
            }

            return level;
        }


    }
}
