using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using AdminMVC.Models;
using Newtonsoft.Json;

namespace AdminMVC.Controllers
{
    public class KundController : Controller
    {
        // GET: Kund
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult GetKund()
        {
            List<Kund> KundLista = new List<Kund>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://193.10.202.72/Kundservice/kunder");
            // Add an Accept header for JSON format.    
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // List all Names.    
            HttpResponseMessage response = client.GetAsync("kunder").Result;  // Blocking call!    
            if (response.IsSuccessStatusCode)
            {
                var products = response.Content.ReadAsStringAsync().Result;
                KundLista = JsonConvert.DeserializeObject<List<Kund>>(products);
            }

            return View(KundLista);
        }
        [Authorize]
        public ActionResult PutKund(int id) /*Metoden kommer ifrån denna källa: https://www.tutorialsteacher.com/webapi/implement-put-method-in-web-api*/
        {
            Kund Kunden = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.72/Kundservice/");
                var responseTask = client.GetAsync("Kunder/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Kund>();
                    readTask.Wait();
                    Kunden = readTask.Result;
                }
            }
            return View(Kunden);
        }

        [HttpPost]
        public ActionResult PutKund(Kund Kunden) /*Metoden kommer ifrån denna källa: https://www.tutorialsteacher.com/webapi/implement-put-method-in-web-api*/
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.72/Kundservice/");
                var putTask = client.PutAsJsonAsync<Kund>("Kunder/" + Kunden.InloggningsId, Kunden);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetKund");
                }
            }

            return View(Kunden);
        }
        [Authorize]
        public ActionResult DeleteKund(int id) /*Metoden kommer ifrån denna källa: https://www.tutorialsteacher.com/webapi/implement-put-method-in-web-api*/
        {
            Kund Kunden = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.72/Kundservice/");
                var responseTask = client.GetAsync("Kunder/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Kund>();
                    readTask.Wait();
                    Kunden = readTask.Result;
                }
            }

            return View(Kunden);
        }

        [HttpPost]
        public ActionResult DeleteKund(Kund kund) /*Metoden kommer ifrån denna källa: https://www.tutorialsteacher.com/webapi/consume-web-api-delete-method-in-aspnet-mvc*/
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.72/Kundservice/");
                var deleteTask = client.DeleteAsync("Kunder/" + kund.InloggningsId.ToString());

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("GetKund");

            }
            return RedirectToAction("GetKund");
        }
    }
}