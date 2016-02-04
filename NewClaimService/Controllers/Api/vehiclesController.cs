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


namespace NewClaimService.Controllers.Api
{
    [Route("api/claims/{claimNumber}/vehicles")]
    public class vehiclesController : Controller
    {
        private IClaimRepository _claimRepository;
        private ILogger<vehiclesController> _logger;

        public vehiclesController(IClaimRepository repository, ILogger<vehiclesController> logger)
        {
            _claimRepository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public JsonResult Get(string claimNumber)
        {
            try
            {
                var result = _claimRepository.GetClaimByClaimNumber(claimNumber);
                if( result == null)
                {
                    return Json(null);
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(Mapper.Map<IEnumerable<VehicleViewModel>>(result.Vehicles));
            }
            catch(Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }
        }

        [HttpGet("getVehicle/{vin}")]
        public JsonResult GetVehicle(string claimNumber, string vin)
        {
            try
            {
                var claim = _claimRepository.GetClaimByClaimNumber(claimNumber);
                if (claim == null)
                {
                    return Json(null);
                }
                var vehicle = claim.Vehicles.Where(v => v.Vin == vin).SingleOrDefault();
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(Mapper.Map<VehicleViewModel>(vehicle));
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }
        }

        [HttpPost("")]
        public JsonResult Post(string claimNumber, [FromBody]VehicleViewModel vm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newVehicle = Mapper.Map<VehicleInfoType>(vm);
                    _claimRepository.AddVehicle(claimNumber, newVehicle);

                    if(_claimRepository.SaveToDB())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<VehicleViewModel>(newVehicle));
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }

            return Json(new { Message = "Failed", ModelState = ModelState });
        }

        [HttpPut("")]
        public JsonResult Put(string claimNumber, [FromBody]VehicleViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newVehicle = Mapper.Map<VehicleInfoType>(vm);
                    var updated = _claimRepository.UpdateVehicle(claimNumber, newVehicle);

                    if (updated && _claimRepository.SaveToDB())
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(Mapper.Map<VehicleViewModel>(newVehicle));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }

            return Json(new { Message = "Failed", ModelState = ModelState });
        }

        
    }
}
