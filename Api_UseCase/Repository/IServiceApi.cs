using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_UseCase.Models;
using Api_UseCase.ViewModel;

namespace Api_UseCase.Repository
{
    public interface IServiceApi
    {
        Task<List<EmployeeDetails>> GetEmployees();
        Task<int> AddEmployee(Employee employee);

        Task<string> UpdateEmployee(Employee employee);
        Task<int> DeleteEmployee(int? empId);
        Task<string> EmployeeStatus(int? empid);
    }
}
