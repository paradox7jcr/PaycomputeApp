using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Paycompute.Entity;
using Paycompute.Persistence;

namespace Paycompute.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private decimal studentLoanAmount;
        private decimal unionFee;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Employee newEmployee)
        {
            await _context.Employees.AddAsync(newEmployee);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int employeeId)
        {
            var employee = GetById(employeeId);
            _context.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees;
        }

        public IEnumerable<SelectListItem> GetAllEmployeesForPayRoll()
        {
            return GetAll().Select(emp => new SelectListItem
            {
                Text = emp.FullName,
                Value = emp.Id.ToString(),
            });
        }

        public Employee GetById(int employeeId)
        {
            return _context.Employees.Where(e => e.Id == employeeId).FirstOrDefault();
        }

        public decimal StudentLoanRepaymentAmount(int id, decimal totalAmount)
        {
            var employee = GetById(id);
            if(employee.StudentLoan == StudentLoan.Yes)
            {
                if(totalAmount < 1750)
                    studentLoanAmount = 0m;
                else if(totalAmount < 2000)
                    studentLoanAmount = 15m;
                else if(totalAmount < 2250)
                    studentLoanAmount = 38m;
                else if(totalAmount < 2500)
                    studentLoanAmount = 60m;
                else
                    studentLoanAmount = 83m;

                return studentLoanAmount;
            }
            else
            {
                return 0.0m;
            }
        }

        public decimal UnionFees(int id)
        {
            var employee = GetById(id);
            if(employee.UnionMember == UnionMember.Yes)
                unionFee = 10m;
            else
                unionFee = 0m;

            return unionFee;
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id)
        {
            var employee = GetById(id);
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
