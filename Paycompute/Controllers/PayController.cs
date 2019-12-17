using Microsoft.AspNetCore.Mvc;
using Paycompute.Models;
using Paycompute.Services;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Paycompute.Entity;

namespace Paycompute.Controllers
{
    public class PayController : Controller
    {
        private IPayComputationService _payComputationService;
        private IEmployeeService _employeeService;
        private ITaxService _taxService;
        private INationalInsuranceContributionService _nationalInsuranceContributionService;
        public PayController(
            IPayComputationService payComputationService,
            IEmployeeService employeeService,
            ITaxService taxService,
            INationalInsuranceContributionService nationalInsuranceContributionService)
        {
            _payComputationService = payComputationService;
            _employeeService = employeeService;
            _taxService = taxService;
            _nationalInsuranceContributionService = nationalInsuranceContributionService;
        }

        public IActionResult Index()
        {
            var payRecords = _payComputationService.GetAll().Select(pay => new PaymentRecordIndexViewModel
            {
                Id = pay.Id,
                EmployeeId = pay.EmployeeId,
                Employee = pay.Employee,
                FullName = pay.FullName,
                PayDate = pay.PayDate,
                PayMonth = pay.PayMonth,
                TaxYearId = pay.TaxYearId,
                Year = _payComputationService.GetTaxYearById(pay.TaxYearId).YearOfTax,
                TotalEarnings = pay.TotalEarnings,
                TotalDeductions = pay.TotalDeduction,
                NetPayment = pay.NetPayment,
            });
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.employees = _employeeService.GetAllEmployeesForPayRoll();
            ViewBag.taxYears = _payComputationService.GetAllTaxYear();
            var model = new PaymentRecordCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentRecordCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                decimal overtimeHours, overtimeEarnings, contractualEarnings;

                decimal totalEarnings, totalDeduction;
                decimal tax, nic, slc, unionFee;
                var payRecord = new PaymentRecord()
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    FullName = _employeeService.GetById(model.EmployeeId).FullName,
                    NiNo = _employeeService.GetById(model.EmployeeId).NationalInsuranceNumber,
                    PayDate = model.PayDate,
                    PayMonth = model.PayMonth,
                    TaxYearId = model.TaxYearId,
                    TaxCode = model.TaxCode,
                    HourlyRate = model.HourlyRate,
                    HoursWorked = model.HoursWorked,
                    ContractualHours = model.ContractualHours,
                    OvertimeHours = overtimeHours = _payComputationService.OvertimeHours(model.HoursWorked, model.ContractualHours),
                    ContractualEarnings = contractualEarnings = _payComputationService.ContractualEarnings(model.ContractualHours, model.HoursWorked, model.HourlyRate),
                    OvertimeEarnings = overtimeEarnings = _payComputationService.OvertimeEarnings(_payComputationService.OvertimeRate(model.HourlyRate), overtimeHours),
                    TotalEarnings = totalEarnings = _payComputationService.TotalEarnings(overtimeEarnings, contractualEarnings),
                    
                    Tax = tax = _taxService.TaxAmount(totalEarnings),
                    UnionFee = unionFee = _employeeService.UnionFees(model.EmployeeId),
                    SLC = slc = _employeeService.StudentLoanRepaymentAmount(model.EmployeeId, totalEarnings),
                    NIC = nic = _nationalInsuranceContributionService.NIContribution(totalEarnings),
                    TotalDeduction = totalDeduction = _payComputationService.TotalDeduction(tax, nic, slc, unionFee),
                    NetPayment = _payComputationService.NetPay(totalEarnings, totalDeduction),
                };
                await _payComputationService.CreateAsync(payRecord);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.employees = _employeeService.GetAllEmployeesForPayRoll();
            ViewBag.taxYears = _payComputationService.GetAllTaxYear();
            return View();
        }
    }
}
