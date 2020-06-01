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
    public class HomeController : Controller
    {
        List<string> filmtitlar = new List<string>();
        List<string> salongnamn = new List<string>();


        public ActionResult Index()
        {

            return View();
        }
        [Authorize]
        public ActionResult GetVisningsSchema()
        {
            List<VisningsSchema> visningsLista = new List<VisningsSchema>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://193.10.202.72/BiljettService/VisningsSchema");
            // Add an Accept header for JSON format.    
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // List all Names.    
            HttpResponseMessage response = client.GetAsync("VisningsSchema").Result;  // Blocking call!    
            if (response.IsSuccessStatusCode)
            {
                var products = response.Content.ReadAsStringAsync().Result;
                visningsLista = JsonConvert.DeserializeObject<List<VisningsSchema>>(products);
            }

            return View(visningsLista);
        }
        [Authorize]
        public ActionResult Create()
        {
            SalongLista();
            FilmLista();
            ViewBag.filmtitelLista = filmtitlar;
            ViewBag.salongnamnLista = salongnamn;

            return View();
        }

        [HttpPost]
        public ActionResult Create(VisningsSchema nyVisning) /*Källa till create-metoden. https://www.tutorialsteacher.com/webapi/consume-web-api-post-method-in-aspnet-mvc*/
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.72/BiljettService/VisningsSchema");
                var postJob = client.PostAsJsonAsync<VisningsSchema>("VisningsSchema", nyVisning);
                postJob.Wait();

                var postReslut = postJob.Result;
                if (postReslut.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetVisningsSchema");
                }

                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            }

            return View(nyVisning);
        }
        [Authorize]
        public ActionResult PutVisningsSchema(int id) /*Metoden kommer ifrån denna källa: https://www.tutorialsteacher.com/webapi/implement-put-method-in-web-api*/
        {
            SalongLista();
            FilmLista();
            ViewBag.filmtitelLista = filmtitlar;
            ViewBag.salongnamnLista = salongnamn;
            VisningsSchema visning = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.72/BiljettService/");
                var responseTask = client.GetAsync("VisningsSchema/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<VisningsSchema>();
                    readTask.Wait();
                    visning = readTask.Result;
                }
            }
            return View(visning);
        }

        [HttpPost]
        public ActionResult PutVisningsSchema(VisningsSchema visning) /*Metoden kommer ifrån denna källa: https://www.tutorialsteacher.com/webapi/implement-put-method-in-web-api*/
        {
            

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.72/BiljettService/VisningsSchema");
                var putTask = client.PutAsJsonAsync<VisningsSchema>("VisningsSchema", visning);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetVisningsSchema");
                }
            }

            return View(visning);
        }
        [Authorize]
        public ActionResult DeleteVisningsSchema(int id) /*Metoden kommer ifrån denna källa: https://www.tutorialsteacher.com/webapi/implement-put-method-in-web-api*/
        {
            VisningsSchema visning = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.72/BiljettService/");
                var responseTask = client.GetAsync("VisningsSchema/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<VisningsSchema>();
                    readTask.Wait();
                    visning = readTask.Result;
                }
            }

            return View(visning);
        }

        [HttpPost]
        public ActionResult DeleteVisningsSchema(VisningsSchema visning) /*Metoden kommer ifrån denna källa: https://www.tutorialsteacher.com/webapi/consume-web-api-delete-method-in-aspnet-mvc*/
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.72/BiljettService/");
                var deleteTask = client.DeleteAsync("VisningsSchema/" + visning.Id.ToString());

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("GetVisningsSchema");

            }
            return RedirectToAction("GetVisningsSchema");
        }
















        public void SalongLista()
        {
            List<Salong> salongLista = new List<Salong>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://193.10.202.71/Filmservice/salong");
            // Add an Accept header for JSON format.    
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // List all Names.    
            HttpResponseMessage response = client.GetAsync("salong").Result;  // Blocking call!    
            if (response.IsSuccessStatusCode)
            {
                var products = response.Content.ReadAsStringAsync().Result;
                salongLista = JsonConvert.DeserializeObject<List<Salong>>(products);
            }

            foreach (var titel in salongLista)
            {
                salongnamn.Add(titel.Namn.ToString());
            }

        }

        public void FilmLista()
        {
            List<Film> filmLista = new List<Film>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://193.10.202.71/Filmservice/film");
            // Add an Accept header for JSON format.    
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // List all Names.    
            HttpResponseMessage response = client.GetAsync("film").Result;  // Blocking call!    
            if (response.IsSuccessStatusCode)
            {
                var products = response.Content.ReadAsStringAsync().Result;
                filmLista = JsonConvert.DeserializeObject<List<Film>>(products);
            }

            foreach (var titel in filmLista)
            {
                filmtitlar.Add(titel.Titel.ToString());
            }

        }


    }
}
