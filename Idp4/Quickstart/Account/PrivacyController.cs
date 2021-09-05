using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Idp4.Quickstart.Account
{
    public class PrivacyController : Controller
    {
        public IActionResult Explanation()
        {
            return View();
        }

        public IActionResult TermsAndConditions()
        {
            return View();
        }

        public IActionResult DeletionInfo()
        {
            return View();
        }
    }
}
