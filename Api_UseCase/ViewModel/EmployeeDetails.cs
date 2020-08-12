using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_UseCase.ViewModel
{
    public class EmployeeDetails
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public int DeptId { get; set; }
        public int ManagerId { get; set; }

        public string EmployeeStatus { get; set; }
        public string Department { get; set; }
        public string Manager { get; set; }
    }
}
