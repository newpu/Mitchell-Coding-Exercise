using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ClaimClient.Models;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.JsonPatch;
using ClaimClient.Helpers;
using System.Net.Http;
using Newtonsoft.Json;


namespace ClaimClient.Controllers
{
    [Route("api/claims")]
    public class claimsController : Controller
    {
        
        private ILogger<claimsController> _logger;

        public claimsController(ILogger<claimsController> logger)
        {
            _logger = logger;
        }

        
        [HttpGet("")]
        public async Task<JsonResult> Get()
        {
            try
            {
                var client = ClaimHttpClient.GetClient();
                HttpResponseMessage response = await client.GetAsync("api/claims");

                if( response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync(); 

                    var model = JsonConvert.DeserializeObject<IEnumerable<MitchellClaimType>>(content);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    // return View(model);
                    return Json(model);
                 }
            }
            catch(Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }

        [HttpGet("detail/{claimNumber}")]
        public async Task<JsonResult> Detail(string claimNumber)
        {
            try
            {
                var client = ClaimHttpClient.GetClient();
                HttpResponseMessage response = await client.GetAsync("api/claims/detail/" + claimNumber);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<MitchellClaimType>(content);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    // return View(model);
                    return Json(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }

        [HttpGet("getRange/{startDate}/{endDate}")]
        public async Task<JsonResult> GetRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var client = ClaimHttpClient.GetClient();
                HttpResponseMessage response = await client.GetAsync("api/claims/getRange/" + startDate + "/" + endDate);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<IEnumerable<MitchellClaimType>>(content);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    // return View(model);
                    return Json(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }

        [HttpPut("")]
        public async Task<JsonResult> Put([FromBody]MitchellClaimType viewM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = ClaimHttpClient.GetClient();
                    var serializedClaimToUpdate = JsonConvert.SerializeObject(viewM);
                    var response = await client.PutAsync("api/claims", new StringContent(serializedClaimToUpdate,
                                 System.Text.Encoding.Unicode, "application/json"));

                    if(response.IsSuccessStatusCode)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(viewM);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }

        [HttpPost("")]
        public async Task<JsonResult> Post([FromBody]MitchellClaimType viewM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = ClaimHttpClient.GetClient();
                    var serializedClaimToAdd = JsonConvert.SerializeObject(viewM);
                    var response = await client.PostAsync("api/claims", new StringContent(serializedClaimToAdd,
                                 System.Text.Encoding.Unicode, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(viewM);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }

        [HttpPatch("{claimNumber}")]
        public async Task<JsonResult> Patch(string claimNumber, [FromBody]MitchellClaimType claimToModify)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = ClaimHttpClient.GetClient();
                    JsonPatchDocument<MitchellClaimType> patchDoc = new JsonPatchDocument<MitchellClaimType>();
                    patchDoc.Replace(pt => pt.AssignedAdjusterID, claimToModify.AssignedAdjusterID);
                    patchDoc.Replace(pt => pt.Vehicles.First().ExteriorColor, claimToModify.Vehicles.First().ExteriorColor);

                    var serializedClaimToModify = JsonConvert.SerializeObject(patchDoc);
                    var response = await client.PatchAsync("api/claims/" + claimNumber, new StringContent(
                        serializedClaimToModify, System.Text.Encoding.Unicode, "application/json"));

                    if(response.IsSuccessStatusCode)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(claimToModify);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }

    }
}
