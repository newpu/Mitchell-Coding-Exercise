using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using NewClaimService.Models;
using NewClaimService.Repository;
using System.Net;
using AutoMapper;
using NewClaimService.ViewModel;
using Microsoft.Framework.Logging;
using Microsoft.AspNet.JsonPatch;


namespace NewClaimService.Controllers.Api
{
    [Route("api/claims")]
    public class claimsController : Controller
    {
        private IClaimRepository _claimRepository;
        private ILogger<claimsController> _logger;

        public claimsController(IClaimRepository repository, ILogger<claimsController> logger)
        {
            _claimRepository = repository;
            _logger = logger;
        }

        
        [HttpGet("")]
        public JsonResult Get()
        {
            try
            {
                var list = _claimRepository.Retrieve();
                // var result = Mapper.Map<IEnumerable<ClaimViewModel>>(list);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(list);
            }
            catch(Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }
        }

        [HttpGet("detail/{claimNumber}")]
        public JsonResult Detail(string claimNumber)
        {
            try
            {
                var claim = _claimRepository.GetClaimByClaimNumber(claimNumber);
                // var result = Mapper.Map<ClaimViewModel>(claim);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(claim);
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }
        }

        [HttpGet("getRange/{startDate}/{endDate}")]
        public JsonResult GetRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var claim = _claimRepository.GetClaimsByDate(startDate, endDate);
                // var result = Mapper.Map<ClaimViewModel>(claim);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(claim);
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }
        }

        [HttpPut("")]
        public JsonResult Put([FromBody]MitchellClaimType viewM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var newClaim = Mapper.Map<MitchellClaimType>(viewM);
                    var newClaim = viewM;

                    if(  _claimRepository.UpdateClaim(newClaim) && _claimRepository.SaveToDB() )
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(newClaim);
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
        public JsonResult Post([FromBody]ClaimViewModel viewM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newClaim = Mapper.Map<MitchellClaimType>(viewM);

                    if( _claimRepository.AddNewClaim(newClaim) && _claimRepository.SaveToDB() )
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(newClaim);
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

        [HttpPatch("{claimNumber}")]
        public JsonResult Patch(string claimNumber, [FromBody]JsonPatchDocument<MitchellClaimType> claimPatchDocument)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var claim = _claimRepository.GetClaimByClaimNumber(claimNumber); ;

                    if (claim == null)
                        return Json(new { Message = "Claim to update not found" });

                    // apply patch document
                    claimPatchDocument.ApplyTo(claim);
                    if (_claimRepository.UpdateClaim(claim) && _claimRepository.SaveToDB())
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(claim);
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
