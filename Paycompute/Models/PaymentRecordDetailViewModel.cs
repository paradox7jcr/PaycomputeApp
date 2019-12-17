using System;
using System.ComponentModel.DataAnnotations;
using Paycompute.Entity;

namespace Paycompute.Models
{
    public class PaymentRecordDetailViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public string NiNo { get; set; }

        [DataType(DataType.Date), Display(Name = "Pay Date")]
        public DateTime PayDate { get; set; }

        [Display(Name = "Month")]
        public string PayMonth { get; set; }

        [Display(Name = "Year")]
        public string Year { get; set; }

        [Display(Name = "Tax Year")]
        public int TaxYearId { get; set; }

        public TaxYear TaxYear { get; set; }

        [Display(Name = "Tax Code")]
        public string TaxCode { get; set; }

        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }

        [Display(Name = "Contractual Hours")]
        public decimal ContractualHours { get; set; }

        [Display(Name = "Overtime Hours")]
        public decimal OvertimeHours { get; set; }

        [Display(Name="Overtime Rate")]
        public decimal OvertimeRate { get; set; }

        [Display(Name = "Contractual Earnings")]
        public decimal ContractualEarnings { get; set; }

        [Display(Name = "Overtime Earnings")]
        public decimal OvertimeEarnings { get; set; }

        [Display(Name = "Tax")]
        public decimal Tax { get; set; }

        [Display(Name = "NIC")]
        public decimal NIC { get; set; }

        [Display(Name = "Union Fee")]
        public decimal? UnionFee { get; set; }

        [Display(Name = "SLC")]
        public decimal? SLC { get; set; }

        [Display(Name = "Total Earnings")]
        public decimal TotalEarnings { get; set; }

        [Display(Name = "Total Deduction")]
        public decimal TotalDeduction { get; set; }

        [Display(Name = "Net Payment")]
        public decimal NetPayment { get; set; }
    }
}
