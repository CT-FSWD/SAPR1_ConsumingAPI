using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//Consuming API - Step 2 - Create the ResourcesController - Empty MVC5 Controller
//Consuming API - Step 3 - usings
using ConsumingWebServices.UI.MVC.Models;//Access to DTO's
using System.Net.Http;//added for HTTPClient - which allows us to send requests to API
using System.Net.Http.Headers;//access to MediaTypeWithQualityHeaderValue object
using Newtonsoft.Json;//Added for deserialization functionality

namespace ConsumingWebServices.UI.MVC.Controllers
{
    public class ResourcesController : Controller
    {
        // GET: Resources - Index template, model: ResourceViewModel, no data context
        public ActionResult Index()
        {
            //Consuming API - Step 4 - Create Region below
            #region Get all resources via the API
            List<ResourceViewModel> resources = new List<ResourceViewModel>();

            using (var client = new HttpClient())
            {
                //configure the HttpClient object so we can make the appropriate request for data
                client.BaseAddress = new Uri("http://localhost:62689/");
                //address above should match the port number on each individual machine when running the SAPR1_ResourceAPI solution.

                //set up the client to accept JSON as the data being passed
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //deal with the response
                HttpResponseMessage response = client.GetAsync("api/Resources/").Result;
                resources = JsonConvert.DeserializeObject<List<ResourceViewModel>>(response.Content.ReadAsStringAsync().Result);

            }
            #endregion
            return View(resources);
        }//end Index action

        public ActionResult Details(int id)
        {
            #region TEACH-COMMENT: ADDED - Get 1 link by ID, via Web API
            ResourceViewModel link = new ResourceViewModel();
            using (var client = new HttpClient())
            {
                //TEACH-COMMENT: configure httpclient
                //The below defines the URI (Uniform Resource Identifier) upon which
                //to apply the request
                client.BaseAddress = new Uri("http://localhost:62689/");
                //The accept header defines what type of media is acceptable for a 
                //response 
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //We will get the results asynchronously - this means the execution of
                //this operation doesn't block the execution of other operations. 
                HttpResponseMessage response = client.GetAsync("api/Resources/" + id).Result;

                //Once a response is received it must be Deserialized - or converted
                //from JSON to an object we can interact with/manipulate in our code
                link = JsonConvert.DeserializeObject<ResourceViewModel>(response.Content.ReadAsStringAsync().Result);
            }
            #endregion

            //UPDATED to pass data to view
            return View(link);
        }

        //Beyond this region, determine how much time you have left for Web Services. You should be done at the end of the day on Monday with the module. If you are running out of time, copy and paste the remaining actions, then generate a view for each.
        // GET: Hyperlinks/Create
        public ActionResult Create()
        {
            #region TEACH-COMMENT: ADDED - provides ddl for category options (GET CREATE)
            List<CategoryViewModel> cats = new List<CategoryViewModel>();
            using (var client = new HttpClient())
            {
                //configure httpclient
                client.BaseAddress = new Uri("http://localhost:62689/");//port # varies by student, unless deployed, then use appropriate URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/Categories/").Result;
                cats = JsonConvert.DeserializeObject<List<CategoryViewModel>>(response.Content.ReadAsStringAsync().Result);
            }

            ViewBag.CategoryID = new SelectList(cats, "CategoryID", "CategoryName");
            #endregion


            return View();
        }
        // POST: Hyperlinks/Create
        [HttpPost]
        //public ActionResult Create(FormCollection collection)
        public ActionResult Create(ResourceViewModel newResource)
        {
            //TEACH-NOTE: REPLACED Try-Catch w/ all code below

            #region TEACH-COMMENT: ADDED - create new link, via Web API
            using (var client = new HttpClient())
            {
                //configure httpclient
                client.BaseAddress = new Uri("http://localhost:62689/");

                //use PostAsJsonAsync<>() - may require nuget install of Microsoft.AspNet.WebAPI.Client.
                //wait for async result: If fail, throw error message. If OK, redirect to Index
                var postTask = client.PostAsJsonAsync<ResourceViewModel>("api/Resources/", newResource);
                postTask.Wait();

                //Get the result code for the task
                var result = postTask.Result;
                //if it wasn't successful, set the create page back up.
                if (!result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Server error reaching API");

                    //TEACH-NOTE: COPIED THIS REGION FROM GET CREATE, updated region note, removed internal USING and SelectList ctor has 4th param for selected item, removed client.baseaddress line
                    #region TEACH-COMMENT: COPY-ADAPTED - provides ddl for category options (POST CREATE + both EDITS)
                    List<CategoryViewModel> cats = new List<CategoryViewModel>();

                    //TEACH-COMMENT: configure httpclient
                    client.BaseAddress = new Uri("http://localhost:62689/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("api/Categories/").Result;
                    cats = JsonConvert.DeserializeObject<List<CategoryViewModel>>(response.Content.ReadAsStringAsync().Result);

                    ViewBag.CategoryID = new SelectList(cats, "CategoryID", "CategoryName", newResource.CategoryId);
                    #endregion

                    return View(newResource);
                }
            }
            #endregion
            //If it was successful, send user back to Index view
            return RedirectToAction("Index");
        }//end Create

        //-------Edit Functionality-------//
        // GET: Hyperlinks/Edit/5
        public ActionResult Edit(int id)
        {
            #region TEACH-COMMENT: COPIED from details - Get 1 link by ID, via Web API
            ResourceViewModel link = new ResourceViewModel();
            using (var client = new HttpClient())
            {
                //configure httpclient
                client.BaseAddress = new Uri("http://localhost:62689/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/Resources/" + id).Result;
                link = JsonConvert.DeserializeObject<ResourceViewModel>(response.Content.ReadAsStringAsync().Result);

                #region TEACH-COMMENT: COPY-ADAPTED - provides ddl for category options (POST CREATE + both EDITS)
                List<CategoryViewModel> cats = new List<CategoryViewModel>();

                // client.BaseAddress = new Uri("http://localhost:62689/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response2 = client.GetAsync("api/Categories/").Result;
                cats = JsonConvert.DeserializeObject<List<CategoryViewModel>>(response2.Content.ReadAsStringAsync().Result);

                ViewBag.CategoryID = new SelectList(cats, "CategoryID", "CategoryName", link.CategoryId);
                #endregion

            }
            #endregion

            //UPDATED to pass data to view
            return View(link);
        }

        // POST: Hyperlinks/Edit/5
        [HttpPost]
        //updated for our type info
        public ActionResult Edit(ResourceViewModel linkToEdit)
        {
            #region TEACH-COMMENT: ADDED - update category, via Web API
            using (var client = new HttpClient())
            {
                //configure httpclient
                client.BaseAddress = new Uri("http://localhost:62689/");

                //use PutAsJsonAsync<>().
                //wait for async result: If fail, throw error message. If OK, redirect to Index
                var putTask = client.PutAsJsonAsync("api/Resources/", linkToEdit);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error reaching API");

                    //COPIED THIS REGION FROM GET EDIT
                    #region TEACH-COMMENT: COPY-ADAPTED - provides ddl for category options (POST CREATE + both EDITS)
                    List<CategoryViewModel> cats = new List<CategoryViewModel>();

                    //configure httpclient
                    client.BaseAddress = new Uri("http://localhost:62689/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response2 = client.GetAsync("api/Categories/").Result;
                    cats = JsonConvert.DeserializeObject<List<CategoryViewModel>>(response2.Content.ReadAsStringAsync().Result);

                    ViewBag.CategoryID = new SelectList(cats, "CategoryID", "CategoryName", linkToEdit.CategoryId);
                    #endregion

                    return View(linkToEdit);
                }
            }
            #endregion
        }//end Edit

        //--------Delete Functionality--------//
        // GET: Hyperlinks/Delete/5
        public ActionResult Delete(int id)
        {
            #region TEACH-COMMENT: COPIED from details - Get 1 link by ID, via Web API
            ResourceViewModel link = new ResourceViewModel();
            using (var client = new HttpClient())
            {
                //TEACH-COMMENT: configure httpclient
                client.BaseAddress = new Uri("http://localhost:62689/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/Resources/" + id).Result;
                link = JsonConvert.DeserializeObject<ResourceViewModel>(response.Content.ReadAsStringAsync().Result);
            }
            #endregion


            //UPDATED - returns data to view
            return View(link);
        }

        // POST: Hyperlinks/Delete/5
        //UPDATED parameters for ONLY int id 
        //ADDED attribute ActionName(Delete): bypass "2 overloads can't take same int id signature" rule
        //public ActionResult Delete(int id, FormCollection collection)
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            //TEACH-NOTE: REPLACED Try-Catch w/ all code below

            #region TEACH-COMMENT: ADDED - delete category, via Web API
            using (var client = new HttpClient())
            {
                //TEACH-COMMENT: configure httpclient
                client.BaseAddress = new Uri("http://localhost:62689/");

                //TEACH-COMMENT: use DeleteAsync().
                //wait for async result: If fail, throw error message. If OK, redirect to Index
                var deleteTask = client.DeleteAsync("api/Resources/" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error reaching API");
                    return View();
                }
            }
            #endregion

        }//end DeletePost

    }//end class
}//end namespace