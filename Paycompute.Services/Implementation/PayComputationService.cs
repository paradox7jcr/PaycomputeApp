﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Paycompute.Entity;
using Paycompute.Persistence;

namespace Paycompute.Services.Implementation
{
    public class PayComputationService : IPayComputationService
    {
        private readonly ApplicationDbContext _context;
        public PayComputationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
        {
            var contractualEarnings = contractualHours * hourlyRate;
            if(hoursWorked < contractualHours)
            {
                contractualEarnings = hoursWorked * hourlyRate;
            }
            return contractualEarnings;
        }

        public async Task CreateAsync(PaymentRecord paymentRecord)
        {
            await _context.PaymentRecords.AddAsync(paymentRecord);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<PaymentRecord> GetAll()
        {
            return _context.PaymentRecords.OrderBy(pr => pr.EmployeeId);
        }

        public IEnumerable<SelectListItem> GetAllTaxYear()
        {
            return _context.TaxYears.Select(taxYears => new SelectListItem
            {
                Text = taxYears.YearOfTax,
                Value = taxYears.Id.ToString(),
            });
        }

        public PaymentRecord GetById(int id)
        {
            return _context.PaymentRecords.FirstOrDefault(pay => pay.Id == id);
        }

        public TaxYear GetTaxYearById(int id)
        {
            return _context.TaxYears.FirstOrDefault(year => year.Id == id);
        }

        public decimal NetPay(decimal totalEarnings, decimal totalDeduction)
        {
            return totalEarnings - totalDeduction;
        }

        public decimal OvertimeEarnings(decimal overtimeRate, decimal overtimeHours)
        {
            return overtimeHours * overtimeRate;
        }

        public decimal OvertimeHours(decimal hoursWorked, decimal contractualHours)
        {
            var overtimeHours = 0.00m;
            if (hoursWorked > contractualHours)
            {
                overtimeHours = hoursWorked - contractualHours;
            }

            return overtimeHours;
        }

        public decimal OvertimeRate(decimal hourlyRate)
        {
            return hourlyRate * 1.5m;
        }

        public decimal TotalDeduction(decimal tax, decimal nationalInsuranceContribution, decimal studentLoanRepayment, decimal unionFees)
        {
            return tax + nationalInsuranceContribution + studentLoanRepayment + unionFees;
        }

        public decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings)
        {
            return overtimeEarnings + contractualEarnings;
        }
    }
}
