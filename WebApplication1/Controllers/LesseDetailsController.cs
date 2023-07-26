using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using LinqToExcel;

namespace WebApplication1.Controllers
{
    [SessionTimeout]
    public class LeaseDetailsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: LesseDetails
        //[Route("/LeaseDetails")]
        public async Task<ActionResult> Index()
        {
            //var tblLesseDetails = db.tblLesseDetails.Include(t => t.tblCustomer);
            //return View(await tblLesseDetails.ToListAsync());

            var vm = new LesseDetailVMForList();
            vm.LesseDetailList = await db.tblLesseDetails.Include(t => t.tblCustomer).Include(t => t.tblLender).OrderByDescending(p => p.LesseDate).ToListAsync();

            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> Index(LesseDetailVMForList vm)
        {
            var result = await db.tblLesseDetails.Include(t => t.tblCustomer).Include(t => t.tblLender).ToListAsync();
            if (vm.FromDate != null && vm.ToDate != null)
            {
                result = result.Where(p => p.LesseDate >= vm.FromDate && p.LesseDate <= vm.ToDate).OrderByDescending(p => p.LesseDate).ToList();
            }
            vm.LesseDetailList = result;
            return View(vm);
        }
        // GET: LesseDetails/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLesseDetail tblLesseDetail = await db.tblLesseDetails.FindAsync(id);
            if (tblLesseDetail == null)
            {
                return HttpNotFound();
            }
            return View(tblLesseDetail);
        }

        // GET: LesseDetails/Create
        public ActionResult Create()
        {
            List<SelectListItem> lesse = new List<SelectListItem>() {
    new SelectListItem {
        Text = "-", Value = "0"
    }
};
            var lesseList = db.tblCustomers.ToList();
            foreach (var it in lesseList)
            {
               var newitem = new SelectListItem
                {
                    Text = it.LastName,
                    Value = it.ID.ToString()
                };
                lesse.Add(newitem);
            }
            List<SelectListItem> lender = new List<SelectListItem>() {
    new SelectListItem {
        Text = "-", Value = "0"
    }
};
            var lenderList = db.tblLenders.ToList();
            foreach (var it in lenderList)
            {
                var newitem = new SelectListItem
                {
                    Text = it.Name,
                    Value = it.ID.ToString()
                };
                lender.Add(newitem);
            }

            List<SelectListItem> leasecount = new List<SelectListItem>() {
    new SelectListItem {
        Text = "-1", Value = "-1"
    },
    new SelectListItem {
        Text = "1", Value = "1"
    }
};

            ViewBag.CustomerID = lesse;
            ViewBag.LenderID = lender;
            ViewBag.LeaseCount = leasecount;
            return View();
        }

        // POST: LesseDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Lesse,LesseDate,FundDate,FirstPaymentDate,FirstPaymentToBankDate,LesseMaturityDate,Description,LeaseID,DealOrigin,LeaseType,TotalLeaase,MonthlyPayment,Term,Lender,LeaseFee,VIN,Residual,EstimatedPropertyTax,MMA,AccountRep1,AccountRep2,AccountRep3,InsuranceProvider,InsuranceExpiry,VendorCost,LeaseDocumentForm,TitleHolder,IsSaleTaxExept,LeaseTerminationDate,TitleStatus,UCCStatus,UCCDate,PropertyTaxStatus,CollateralStreet,CollateralCity,CollateralState,CollateralZipCode,CollateralCountry,MailingStreet,MailingCity,MailingState,MailingZipCode,Comments1,Comments2,LeaseYear,LeaseMonthAndYear,FundedMonthAndYear,LeaseCount,BANKCHK_LESSEEBANK_TAB,BANKCHK_BANKYEAR_TAB,BANKCHK_BANKMONTH_TAB,LESSEECHK_LESSEEBANK_TAB,LESSEECHK_LESSEEYEAR_TAB,LESSEECHK_LESSEEMONTH_TAB,CustomerID,LenderID,LeaseRate,LicensePlate")] tblLesseDetail tblLesseDetail)
        {
            if (ModelState.IsValid)
            {
                if (tblLesseDetail.CustomerID == null || tblLesseDetail.CustomerID == 0)
                {
                    tblCustomer customerObj = new tblCustomer();
                    customerObj.LastName = tblLesseDetail.Lesse;
                    db.tblCustomers.Add(customerObj);
                    db.SaveChanges();
                    tblLesseDetail.CustomerID = customerObj.ID;
                }
                else
                {
                    tblCustomer customerObj = db.tblCustomers.Find(tblLesseDetail.CustomerID);
                    tblLesseDetail.Lesse = customerObj.LastName;
                }
                if ((tblLesseDetail.LenderID == null || tblLesseDetail.LenderID == 0) && tblLesseDetail.Lender != null)
                {
                    tblLender lenderObj = new tblLender();
                    lenderObj.Name = tblLesseDetail.Lender;
                    db.tblLenders.Add(lenderObj);
                    db.SaveChanges();
                    tblLesseDetail.LenderID = lenderObj.ID;
                }
                else if(tblLesseDetail.LenderID != null && tblLesseDetail.LenderID != 0)
                {
                    tblLender lenderObj = db.tblLenders.Find(tblLesseDetail.LenderID);
                    tblLesseDetail.Lender = lenderObj.Name;
                }
                db.tblLesseDetails.Add(tblLesseDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.tblCustomers, "ID", "LastName", tblLesseDetail.CustomerID);
            return View(tblLesseDetail);
        }

        // GET: LesseDetails/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLesseDetail tblLesseDetail = await db.tblLesseDetails.FindAsync(id);
            if (tblLesseDetail == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> leasecount = new List<SelectListItem>() {
    new SelectListItem {
        Text = "-1", Value = "-1"
    },
    new SelectListItem {
        Text = "1", Value = "1"
    }
};
            ViewBag.LeaseCount = new SelectList(leasecount, "Value", "Text", tblLesseDetail.LeaseCount);
            ViewBag.CustomerID = new SelectList(db.tblCustomers, "ID", "LastName", tblLesseDetail.CustomerID);
            ViewBag.LenderID = new SelectList(db.tblLenders, "ID", "Name", tblLesseDetail.LenderID);
            return View(tblLesseDetail);
        }

        // POST: LesseDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Lesse,LesseDate,FundDate,FirstPaymentDate,FirstPaymentToBankDate,LesseMaturityDate,Description,LeaseID,DealOrigin,LeaseType,TotalLeaase,MonthlyPayment,Term,Lender,LeaseFee,VIN,Residual,EstimatedPropertyTax,MMA,AccountRep1,AccountRep2,AccountRep3,InsuranceProvider,InsuranceExpiry,VendorCost,LeaseDocumentForm,TitleHolder,IsSaleTaxExept,LeaseTerminationDate,TitleStatus,UCCStatus,UCCDate,PropertyTaxStatus,CollateralStreet,CollateralCity,CollateralState,CollateralZipCode,CollateralCountry,MailingStreet,MailingCity,MailingState,MailingZipCode,Comments1,Comments2,LeaseYear,LeaseMonthAndYear,FundedMonthAndYear,LeaseCount,BANKCHK_LESSEEBANK_TAB,BANKCHK_BANKYEAR_TAB,BANKCHK_BANKMONTH_TAB,LESSEECHK_LESSEEBANK_TAB,LESSEECHK_LESSEEYEAR_TAB,LESSEECHK_LESSEEMONTH_TAB,CustomerID,LenderID,LeaseRate,LicensePlate")] tblLesseDetail tblLesseDetail)
        {
            if (ModelState.IsValid)
            {
                if (tblLesseDetail.CustomerID == null || tblLesseDetail.CustomerID == 0)
                {
                    tblCustomer customerObj = new tblCustomer();
                    customerObj.LastName = tblLesseDetail.Lesse;
                    db.tblCustomers.Add(customerObj);
                    db.SaveChanges();
                    tblLesseDetail.CustomerID = customerObj.ID;
                }
                else
                {
                    tblCustomer customerObj = db.tblCustomers.Find(tblLesseDetail.CustomerID);
                    tblLesseDetail.Lesse = customerObj.LastName;
                }
                if (tblLesseDetail.LenderID == null || tblLesseDetail.LenderID == 0)
                {
                    tblLender customerObj = new tblLender();
                    customerObj.Name = tblLesseDetail.Lesse;
                    db.tblLenders.Add(customerObj);
                    db.SaveChanges();
                    tblLesseDetail.LenderID = customerObj.ID;
                }
                else
                {
                    tblLender customerObj = db.tblLenders.Find(tblLesseDetail.CustomerID);
                    tblLesseDetail.Lender = customerObj.Name;
                }
                db.Entry(tblLesseDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.tblCustomers, "ID", "LastName", tblLesseDetail.CustomerID);
            ViewBag.LenderID = new SelectList(db.tblLenders, "ID", "Name", tblLesseDetail.LenderID);
            return View(tblLesseDetail);
        }

        // GET: LesseDetails/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLesseDetail tblLesseDetail = await db.tblLesseDetails.FindAsync(id);
            if (tblLesseDetail == null)
            {
                return HttpNotFound();
            }
            return View(tblLesseDetail);
        }

        // POST: LesseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tblLesseDetail tblLesseDetail = await db.tblLesseDetails.FindAsync(id);
            db.tblLesseDetails.Remove(tblLesseDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public ActionResult UploadExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> UploadExcel(HttpPostedFileBase FileUpload)
        {

            List<string> data = new List<string>();

            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Detail$]", connectionString);
                    var ds = new DataSet();
                    adapter.Fill(ds, "ExcelTable");
                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Detail";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    excelFile.AddMapping<LesseDetailVM>(x => x.LesseDate, "LEASE DATE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.FundDate, "FUND DATE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.FirstPaymentDate, "1ST PAYMENT DATE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.FirstPaymentToBankDate, "1ST PAYMENT TO BANK DATE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LesseMaturityDate, "LESSE MATURITY DATE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.Description, "DESCRIPTION");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LeaseID, "LEASE ID");
                    excelFile.AddMapping<LesseDetailVM>(x => x.DealOrigin, "DEAL ORIGIN");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LeaseType, "LEASE TYPE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.TotalLeaase, "TOTAL LEASE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.MonthlyPayment, "MTHLY PMT");
                    excelFile.AddMapping<LesseDetailVM>(x => x.Term, "TERM");
                    excelFile.AddMapping<LesseDetailVM>(x => x.Lender, "LENDER");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LeaseFee, "LEASE FEE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.VIN, "VIN");
                    excelFile.AddMapping<LesseDetailVM>(x => x.Residual, "RESIDUAL");
                    excelFile.AddMapping<LesseDetailVM>(x => x.EstimatedPropertyTax, "ESTIMATED PROPERTY TAX");
                    excelFile.AddMapping<LesseDetailVM>(x => x.MMA, "MMA");
                    excelFile.AddMapping<LesseDetailVM>(x => x.AccountRep1, "ACCOUNT REP1");
                    excelFile.AddMapping<LesseDetailVM>(x => x.AccountRep2, "ACCOUNT REP2");
                    excelFile.AddMapping<LesseDetailVM>(x => x.AccountRep3, "ACCOUNT REP3");
                    excelFile.AddMapping<LesseDetailVM>(x => x.InsuranceProvider, "INSURANCE PROVIDER");
                    excelFile.AddMapping<LesseDetailVM>(x => x.InsuranceExpiry, "INSURANCE EXPIRY");
                    excelFile.AddMapping<LesseDetailVM>(x => x.VendorCost, "VENDOR COST");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LeaseDocumentForm, "LEASE DOCUMENT FORM");
                    excelFile.AddMapping<LesseDetailVM>(x => x.TitleHolder, "TITLEHOLDER");
                    excelFile.AddMapping<LesseDetailVM>(x => x.IsSaleTaxExept, "SALES-TAX EXEMPT?");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LeaseTerminationDate, "LEASE TERMINATION DATE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.TitleStatus, "TITLE STATUS");
                    excelFile.AddMapping<LesseDetailVM>(x => x.UCCStatus, "UCC STATUS");
                    excelFile.AddMapping<LesseDetailVM>(x => x.UCCDate, "UCC DATE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.PropertyTaxStatus, "PROPERTY TAX STATUS");
                    excelFile.AddMapping<LesseDetailVM>(x => x.CollateralStreet, "STREET");
                    excelFile.AddMapping<LesseDetailVM>(x => x.CollateralCity, "CITY");
                    excelFile.AddMapping<LesseDetailVM>(x => x.CollateralState, "STATE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.CollateralZipCode, "ZIP CODE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.CollateralCountry, "COUNTY");
                    excelFile.AddMapping<LesseDetailVM>(x => x.MailingStreet, "STREET");
                    excelFile.AddMapping<LesseDetailVM>(x => x.MailingCity, "CITY");
                    excelFile.AddMapping<LesseDetailVM>(x => x.MailingState, "STATE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.MailingZipCode, "ZIP CODE");
                    excelFile.AddMapping<LesseDetailVM>(x => x.Comments1, "COMMENTS1");
                    excelFile.AddMapping<LesseDetailVM>(x => x.Comments2, "COMMENTS2");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LeaseYear, "LEASE YEAR");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LeaseMonthAndYear, "LEASE MO/YR");
                    excelFile.AddMapping<LesseDetailVM>(x => x.FundedMonthAndYear, "FUNDED MO/YR");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LeaseCount, "LEASE COUNT");
                    excelFile.AddMapping<LesseDetailVM>(x => x.BANKCHK_LESSEEBANK_TAB, "BANK CHK - LESSEE-BANK TAB");
                    excelFile.AddMapping<LesseDetailVM>(x => x.BANKCHK_BANKYEAR_TAB, "BANK CHK - BANK-YEAR TAB");
                    excelFile.AddMapping<LesseDetailVM>(x => x.BANKCHK_BANKMONTH_TAB, "BANK CHK - BANK-MONTH TAB");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LESSEECHK_LESSEEBANK_TAB, "LESSEE CHK - LESSEE-BANK TAB");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LESSEECHK_LESSEEYEAR_TAB, "LESSEE CHK - LESSEE-YEAR TAB");
                    excelFile.AddMapping<LesseDetailVM>(x => x.LESSEECHK_LESSEEMONTH_TAB, "LESSEE CHK - LESSEE-MONTH TAB");

                    var lesseDetails = from a in excelFile.Worksheet<LesseDetailVM>(sheetName) select a;
                    try
                    {
                        foreach (var a in lesseDetails)
                        {
                                if (a.LESSEE != "" && a.LESSEE != null)
                                {
                                    tblLesseDetail TU = new tblLesseDetail();
                                    TU.Lesse = a.LESSEE;
                                    TU.LesseDate = a.LesseDate;
                                    TU.FundDate = a.FundDate;
                                    TU.FirstPaymentDate = a.FirstPaymentDate;
                                    TU.FirstPaymentToBankDate = a.FirstPaymentToBankDate;
                                    TU.LesseMaturityDate = a.LesseMaturityDate;
                                    TU.Description = a.Description;
                                    TU.LeaseID = a.LeaseID;
                                    TU.DealOrigin = a.DealOrigin;
                                    TU.LeaseType = a.LeaseType;
                                    TU.TotalLeaase = a.TotalLeaase;
                                    TU.MonthlyPayment = a.MonthlyPayment;
                                    TU.Term = a.Term;
                                    TU.Lender = a.Lender;
                                    TU.LeaseFee = a.LeaseFee;
                                    TU.VIN = a.VIN;
                                    TU.Residual = a.Residual;
                                    TU.EstimatedPropertyTax = a.EstimatedPropertyTax;
                                    TU.MMA = a.MMA;
                                    TU.AccountRep1 = a.AccountRep1;
                                    TU.AccountRep2 = a.AccountRep2;
                                    TU.AccountRep3 = a.AccountRep3;
                                    TU.InsuranceProvider = a.InsuranceProvider;
                                    TU.InsuranceExpiry = a.InsuranceExpiry;
                                    TU.VendorCost = a.VendorCost;
                                    TU.LeaseDocumentForm = a.LeaseDocumentForm;
                                    TU.TitleHolder = a.TitleHolder;
                                    if (!string.IsNullOrEmpty(a.IsSaleTaxExept))
                                    {
                                        if (a.IsSaleTaxExept == "YES")
                                        {
                                            TU.IsSaleTaxExept = true;
                                        }
                                        else
                                        {
                                            TU.IsSaleTaxExept = false;
                                        }
                                    }
                                    TU.LeaseTerminationDate = a.LeaseTerminationDate;
                                    TU.TitleStatus = a.TitleStatus;
                                    TU.UCCStatus = a.UCCStatus;
                                    TU.UCCDate = a.UCCDate;
                                    TU.PropertyTaxStatus = a.PropertyTaxStatus;
                                    TU.CollateralStreet = a.CollateralStreet;
                                    TU.CollateralCity = a.CollateralCity;
                                    TU.CollateralState = a.CollateralState;
                                    TU.CollateralZipCode = a.CollateralZipCode;
                                    TU.CollateralCountry = a.CollateralCountry;
                                    TU.MailingStreet = a.MailingStreet;
                                    TU.MailingCity = a.MailingCity;
                                    TU.MailingState = a.MailingState;
                                    TU.MailingZipCode = a.MailingZipCode;
                                    TU.Comments1 = a.Comments1;
                                    TU.Comments2 = a.Comments2;
                                    TU.LeaseYear = a.LeaseYear;
                                    TU.LeaseMonthAndYear = a.LeaseMonthAndYear;
                                    TU.FundedMonthAndYear = a.FundedMonthAndYear;
                                    TU.LeaseCount = a.LeaseCount;
                                    TU.BANKCHK_LESSEEBANK_TAB = a.BANKCHK_LESSEEBANK_TAB;
                                    TU.BANKCHK_BANKYEAR_TAB = a.BANKCHK_BANKYEAR_TAB;
                                    TU.BANKCHK_BANKMONTH_TAB = a.BANKCHK_BANKMONTH_TAB;
                                    TU.LESSEECHK_LESSEEBANK_TAB = a.LESSEECHK_LESSEEBANK_TAB;
                                    TU.LESSEECHK_LESSEEYEAR_TAB = a.LESSEECHK_LESSEEYEAR_TAB;
                                    TU.LESSEECHK_LESSEEMONTH_TAB = a.LESSEECHK_LESSEEMONTH_TAB;
                                if(TU.Lesse != null && TU.Lesse != string.Empty)
                                {
                                    tblCustomer cust = db.tblCustomers.Where(p => p.LastName == TU.Lesse).FirstOrDefault();
                                    if (cust != null)
                                    {
                                        TU.CustomerID = cust.ID;
                                    }
                                    else
                                    {
                                        tblCustomer customerObj = new tblCustomer();
                                        customerObj.LastName = TU.Lesse;
                                        db.tblCustomers.Add(customerObj);
                                        db.SaveChanges();
                                        TU.CustomerID = customerObj.ID;
                                    }
                                }
                                if (TU.Lender != null && TU.Lender != string.Empty)
                                {
                                    tblLender cust = db.tblLenders.Where(p => p.Name == TU.Lender).FirstOrDefault();
                                    if (cust != null)
                                    {
                                        TU.LenderID = cust.ID;
                                    }
                                    else
                                    {
                                        tblLender customerObj = new tblLender();
                                        customerObj.Name = TU.Lender;
                                        db.tblLenders.Add(customerObj);
                                        db.SaveChanges();
                                        TU.LenderID = customerObj.ID;
                                    }
                                    
                                }
                                db.tblLesseDetails.Add(TU);

                                }
                                //}
                                //else
                                //{
                                //    data.Add("<ul>");
                                //    if (a.Name == "" || a.Name == null) data.Add("<li> name is required</li>");
                                //    if (a.Address == "" || a.Address == null) data.Add("<li> Address is required</li>");
                                //    if (a.ContactNo == "" || a.ContactNo == null) data.Add("<li>ContactNo is required</li>");
                                //    data.Add("</ul>");
                                //    data.ToArray();
                                //    return Json(data, JsonRequestBehavior.AllowGet);
                                //}
                            
                        }
                        //deleting excel file from folder
                        db.SaveChanges();
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    //alert message for invalid file format
                    data.Add("<ul>");
                    data.Add("<li>Only Excel file format is allowed</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
