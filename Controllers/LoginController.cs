using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using AdminMVC.Models;
using Newtonsoft.Json;

namespace AdminMVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(PersonalModel login)
        {
            if (login.Email == null || login.Losenord == null)
            {
                ModelState.AddModelError("", "Du måste fylla i både användarnamn och lösenord");
                return View();
            }
            bool validUser = false;

            List<PersonalModel> ResponsAnv = new List<PersonalModel>();

            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri("http://193.10.202.74/inlogg/anvandares");
                var response = client.PostAsJsonAsync("LoggaIn", login).Result;
                if (response.IsSuccessStatusCode)

                {
                    var AnvSvar = response.Content.ReadAsStringAsync().Result;
                    validUser = true;
                    Personal objektFrånWS = JsonConvert.DeserializeObject<Personal>(AnvSvar);
                    if (objektFrånWS != null)
                    {
                        if (objektFrånWS.Behorighetsniva == 0) 
                        {
                            validUser = true;
                        }else 
                        {
                            validUser = false;
                        } 
                    }
                }
                else
                    validUser = false;
                }



            if (validUser == true)
            {
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(login.Email, false);
            }
            ModelState.AddModelError("", "Inloggningen ej godkänd");
            return RedirectToAction("Index", "Login");

        }



        //[HttpPost]
        //public ActionResult Index(PersonalModel test)
        //{
        //    //PersonalModel test = new PersonalModel();
        //    List<PersonalModel> ResponsAnv = new List<PersonalModel>();

        //    //test.Email = anvNamn;
        //    //test.Losenord = losord;

        //    using (var client = new HttpClient())

        //    {
        //        client.BaseAddress = new Uri("http://193.10.202.74/inlogg/anvandares");
        //        var response = client.PostAsJsonAsync("LoggaIn", test).Result;
        //        if (response.IsSuccessStatusCode)

        //        {
        //            var AnvSvar = response.Content.ReadAsStringAsync().Result;
        //            //ResponseAnv = JsonConvert.DeserializeObject<List<PersonalModel>>(AnvSvar);
        //            Console.WriteLine("Success");
        //            return RedirectToAction("GetVisningsSchema", "Home");

        //        }
        //        else

        //            Console.WriteLine("Error");

        //    }
        //    return View();
        //}
    }
}