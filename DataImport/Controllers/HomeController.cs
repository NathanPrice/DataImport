using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DataImport.Models;

namespace DataImport.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new List<AccountModel>());
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            List<AccountModel> customers = new List<AccountModel>();

            if (postedFile == null) return View(customers);
            string path = Server.MapPath("~/Uploads/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = path + Path.GetFileName(postedFile.FileName);
            postedFile.SaveAs(filePath);

            string csvData = ReadCsvFile(filePath);

            return SplitCsvFile(customers, csvData);
        }

        private ActionResult SplitCsvFile(List<AccountModel> accounts, string csvData)
        {
            accounts.AddRange(from row in csvData.Split('\n')
                               where !string.IsNullOrEmpty(row)
                               select new AccountModel
                               {
                                   AccountNumber = row.Split(',')[0],
                                   LastName = row.Split(',')[1],
                                   FirstName = row.Split(',')[2],
                                   MiddleInitial = row.Split(',')[3],
                                   CompanyName = row.Split(',')[4],
                                   Address = row.Split(',')[5],
                                   City = row.Split(',')[6],
                                   State = row.Split(',')[7],
                                   Zip = row.Split(',')[8],
                                   PhoneNumber = row.Split(',')[9],
                                   AccountCreationDate = row.Split(',')[10],
                                   CurrentBalance = row.Split(',')[11],
                                   AccountType = row.Split(',')[12],
                               });

            return View(accounts);
        }

        private static string ReadCsvFile(string filePath)
        {
            return System.IO.File.ReadAllText(filePath);
        }
    }
}