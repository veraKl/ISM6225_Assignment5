using DataGov_API_Intro_6.Models;
using Microsoft.AspNetCore.Mvc;
using DataGov_API_Intro_6.DataAccess;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataGov_API_Intro_6.Controllers
{
    public class HomeController : Controller
    {
        HttpClient? httpClient;

        static string BASE_URL = "https://data.wa.gov/resource/f6w7-q2d2.json";
        static string API_KEY = "hcEy0sdQxZG53NCsiQQIsBGH8tmwt2o3gbPmmwcG"; //Add your API key here inside ""

        // Obtaining the API key is easy. The same key should be usable across the entire
        // data.gov developer network, i.e. all data sources on data.gov.
        // https://www.nps.gov/subjects/developer/get-started.htm

        public ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string NATIONAL_PARK_API_PATH = BASE_URL + "?limit=20";
            string parksData = "";

            Parks? parks = null;

            //httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);
            httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);

            try
            {
                //HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                //                                        .GetAwaiter().GetResult();
                HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                                                        .GetAwaiter().GetResult();



                if (response.IsSuccessStatusCode)
                {
                    parksData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!parksData.Equals(""))
                {
                    //JsonConvert is part of the NewtonSoft.Json Nuget package
                    parks = JsonConvert.DeserializeObject<Parks>(parksData);
                }

                dbContext.Parks.Add(parks);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }

            return View(parks);
            //return View();
        }
    }
}













