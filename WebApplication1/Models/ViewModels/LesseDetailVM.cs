using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LesseDetailVM
    {
        public string LESSEE { get; set; }
        public Nullable<System.DateTime> LesseDate { get; set; }
        public Nullable<System.DateTime> FundDate { get; set; }
        public Nullable<System.DateTime> FirstPaymentDate { get; set; }
        public Nullable<System.DateTime> FirstPaymentToBankDate { get; set; }
        public Nullable<System.DateTime> LesseMaturityDate { get; set; }
        public string Description { get; set; }
        public string LeaseID { get; set; }
        public string DealOrigin { get; set; }
        public string LeaseType { get; set; }
        public string TotalLeaase { get; set; }
        public string MonthlyPayment { get; set; }
        public string Term { get; set; }
        public string Lender { get; set; }
        public string LeaseFee { get; set; }
        public string VIN { get; set; }
        public string Residual { get; set; }
        public string EstimatedPropertyTax { get; set; }
        public string MMA { get; set; }
        public string AccountRep1 { get; set; }
        public string AccountRep2 { get; set; }
        public string AccountRep3 { get; set; }
        public string InsuranceProvider { get; set; }
        public Nullable<System.DateTime> InsuranceExpiry { get; set; }
        public string VendorCost { get; set; }
        public string LeaseDocumentForm { get; set; }
        public string TitleHolder { get; set; }
        public string IsSaleTaxExept { get; set; }
        public Nullable<System.DateTime> LeaseTerminationDate { get; set; }
        public string TitleStatus { get; set; }
        public string UCCStatus { get; set; }
        public Nullable<System.DateTime> UCCDate { get; set; }
        public string PropertyTaxStatus { get; set; }
        public string CollateralStreet { get; set; }
        public string CollateralCity { get; set; }
        public string CollateralState { get; set; }
        public string CollateralZipCode { get; set; }
        public string CollateralCountry { get; set; }
        public string MailingStreet { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingZipCode { get; set; }
        public string Comments1 { get; set; }
        public string Comments2 { get; set; }
        public string LeaseYear { get; set; }
        public string LeaseMonthAndYear { get; set; }
        public string FundedMonthAndYear { get; set; }
        public string LeaseCount { get; set; }
        public string BANKCHK_LESSEEBANK_TAB { get; set; }
        public string BANKCHK_BANKYEAR_TAB { get; set; }
        public string BANKCHK_BANKMONTH_TAB { get; set; }
        public string LESSEECHK_LESSEEBANK_TAB { get; set; }
        public string LESSEECHK_LESSEEYEAR_TAB { get; set; }
        public string LESSEECHK_LESSEEMONTH_TAB { get; set; }
    }
}