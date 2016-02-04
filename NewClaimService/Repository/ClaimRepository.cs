using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewClaimService.Models;
using System.Xml.Serialization;
using Microsoft.Data.Entity;
using Microsoft.Framework.Logging;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace NewClaimService.Repository
{
    public class ClaimRepository : IClaimRepository
    {
        private ClaimContext _context;
        private ILogger<ClaimRepository> _logger;
        public ClaimRepository(ClaimContext context, ILogger<ClaimRepository> logger)
        {
            _context = context;
            _logger = logger;

            //var claim = ReadInput();
            //var claim = ReadXmlInput();
            SeedData();
        }

        private void SeedData()
        {
            var mitchellClaim = CreateOne();

            try
            {
                if (_context.MitchellClaim.Any(c => c.ClaimNumber == mitchellClaim.ClaimNumber) == false)
                {
                    _context.VehicleInfo.AddRange(mitchellClaim.Vehicles);
                    _context.LossInfo.Add(mitchellClaim.LossInfo);
                    _context.MitchellClaim.Add(mitchellClaim);
                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("seed data error", ex);
            }
        }

        private MitchellClaimType CreateOne()
        {
            var claim = new MitchellClaimType()
            {
                ClaimNumber = "22c9c23bac142856018ce14a26b6c299",
                ClaimantFirstName = "George",
                ClaimantLastName = "Washington",
                Status = StatusCode.OPEN,
                LossDate = Convert.ToDateTime("2014-07-09T17:19:13.631-07:00"),
                AssignedAdjusterID = 12345,
            };

            claim.LossInfo = new LossInfoType()
            {
                CauseOfLoss = (CauseOfLossCode)Enum.Parse(typeof(CauseOfLossCode), "Collision"),
                ReportedDate = Convert.ToDateTime("2014-07-10T17:19:13.676-07:00"),
                LossDescription = "Crashed into an apple tree"
            };

            claim.Vehicles = new List<VehicleInfoType>()
            {
               new VehicleInfoType() {
                    Vin = "1M8GDM9AXKP042788",
                    ModelYear = 2015,
                    MakeDescription = "Ford",
                    ModelDescription = "Mustang",
                    EngineDescription = "EcoBoost",
                    ExteriorColor = "Deep Impact Blue",
                    LicPlate = "NO1PRES",
                    LicPlateState = "VA",
                    LicPlateExpDate = Convert.ToDateTime("2015-04-15T07:00"),
                    DamageDescription = "Front end smashed in. Apple dents in roof.",
                    Mileage = 1776
                }
            };

            return claim;
        }

        private MitchellClaimType ReadInput()
        {
            MitchellClaimType mitchellClaim;
            try
            { 
                XmlSerializer reader = new XmlSerializer(typeof(MitchellClaimType));
                System.IO.StreamReader file = new System.IO.StreamReader(
                    @"C:\NewClaimService\src\NewClaimService\Assets\create-claim.xml");
                mitchellClaim = (MitchellClaimType)reader.Deserialize(file);
                file.Close();
            }
            catch(Exception ex)
            {
                _logger.LogError("Could not get claim by claim number", ex);
                return null;
            }

            return mitchellClaim;
        }

        //MitchellClaimType defined for Xml inout is placed in "XmlModel" folder. To avoid conflict with models in folder "Models"
        //( which are used for create a database(ClaimDB2), they are commented out. Xml Deserialize is tested with Postman tool
        private MitchellClaimType ReadXmlInput()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(MitchellClaimType));
            MitchellClaimType claim = null;

            FileStream fs = new FileStream(@"C:\NewClaimService\src\NewClaimService\Assets\create-claim.xml", FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

            try
            {
                claim = (MitchellClaimType)serializer.ReadObject(reader);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            finally
            {
                reader.Close();
                fs.Close();
            }

            return claim;
        }

        public IList<MitchellClaimType> Retrieve()
        {
            try
            {
                return _context.MitchellClaim.Include(c=> c.Vehicles ).Include(c=> c.LossInfo).Select(c => c).ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError("Could not retrieve data from database", ex);
                return null;
            }
        }

        public MitchellClaimType GetClaimByClaimNumber(string claimNumber)
        {
            try
            {
                return _context.MitchellClaim.Include( cl => cl.Vehicles ).Where(c => c.ClaimNumber == claimNumber).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get claim by claim number", ex);
                return null;
            }
        }

        public IList<MitchellClaimType> GetClaimsByDate(DateTime lossDate1, DateTime lossDate2)
        {
            var startDate = lossDate1;
            var endDate = lossDate2;

            try
            {
                return _context.MitchellClaim.Include(c => c.Vehicles).Where(c => c.LossDate >= startDate && c.LossDate <= endDate).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get claim by loss date", ex);
                return null;
            }
        }

        private MitchellClaimType GetModifiedClaim()
        {
            var claim = new MitchellClaimType()
            {
                ClaimNumber = "22c9c23bac142856018ce14a26b6c299",
                AssignedAdjusterID = 67890,
            };
  
            claim.Vehicles = new List<VehicleInfoType>()
            {
                new VehicleInfoType()
                {
                    Vin = "1M8GDM9AXKP042788",
                    ExteriorColor = "Competition Orange",
                    LicPlateExpDate = Convert.ToDateTime("2015-04-15T07:00")
                }
            };
            
            return claim;
        }

        public bool UpdateClaim(MitchellClaimType claimPassIn)
        {
            bool isChanged = false;

            //to test Xml file input
            // var claim = ReadUpdatefile();
            // var claim = GetModifiedClaim();

            var claim = claimPassIn;
            try
            {
                var claimToUpdate = _context.MitchellClaim.Include(c => c.Vehicles).Where(c => c.ClaimNumber == claim.ClaimNumber).SingleOrDefault();

                if (claimToUpdate != null)
                {
                    if ( claim.AssignedAdjusterID != 0 && claim.AssignedAdjusterID != claimToUpdate.AssignedAdjusterID)
                    {
                        claimToUpdate.AssignedAdjusterID = claim.AssignedAdjusterID;
                        isChanged = true;
                    }
                    //TODO  more fields to update

                    var vNew = claim.Vehicles.FirstOrDefault();
                    var vehicleToUpdate = claimToUpdate.Vehicles.Where(v => v.Vin == vNew.Vin).SingleOrDefault();
                    if (vehicleToUpdate != null)
                    {
                        if (vNew.ExteriorColor != null && vNew.ExteriorColor != "" && vNew.ExteriorColor != vehicleToUpdate.ExteriorColor)
                        {
                            vehicleToUpdate.ExteriorColor = vNew.ExteriorColor;
                            isChanged = true;
                        }

                        if (vNew.LicPlateExpDate != null && vNew.LicPlateExpDate != vehicleToUpdate.LicPlateExpDate)
                        {
                            vehicleToUpdate.LicPlateExpDate = vNew.LicPlateExpDate;
                            isChanged = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("claim update error", ex);
                return false;
            }

            return isChanged;
        }


        public bool UpdateVehicle(string claimNumber, VehicleInfoType newVehicle)
        {
            bool isChanged = false;
            try
            {
                var claimToUpdate = _context.MitchellClaim.Include( c => c.Vehicles).Where(c => c.ClaimNumber == claimNumber).SingleOrDefault();

                if (claimToUpdate != null)
                {
                    var vehicleToUpdate = claimToUpdate.Vehicles.Where(v => v.Vin == newVehicle.Vin).SingleOrDefault();
                    if (vehicleToUpdate != null)
                    {
                        if (newVehicle.ExteriorColor != null && newVehicle.ExteriorColor != "" && newVehicle.ExteriorColor != vehicleToUpdate.ExteriorColor)
                        {
                            vehicleToUpdate.ExteriorColor = newVehicle.ExteriorColor;
                            isChanged = true;
                        }

                        if (newVehicle.LicPlateExpDate != null && newVehicle.LicPlateExpDate != vehicleToUpdate.LicPlateExpDate)
                        {
                            vehicleToUpdate.LicPlateExpDate = newVehicle.LicPlateExpDate;
                            isChanged = true;
                        }

                        //TODO more fields to update
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("vehicle update error", ex);
                return false;
            }
            return isChanged;
        }

        public bool AddNewClaim(MitchellClaimType claim)
        {
            try
            {
                if (_context.MitchellClaim.Any(c => c.ClaimNumber == claim.ClaimNumber) == false)
                {
                    foreach (var vehicle in claim.Vehicles)
                    {
                        _context.VehicleInfo.Add(vehicle);
                    }
                    _context.LossInfo.Add(claim.LossInfo);
                    _context.MitchellClaim.Add(claim);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not add new claim.", ex);
                return false;
            }

            return false;
        }

        public void AddVehicle(string claimNumber,VehicleInfoType veh)
        {
            var claim = _context.MitchellClaim.Include(c => c.Vehicles).Where(c => c.ClaimNumber == claimNumber).FirstOrDefault();
            if( claim.Vehicles.Count > 0)
            {
                claim.Vehicles.Add(veh);
            }
            else
            {
                var list = new List<VehicleInfoType>();
                list.Add(veh);
                claim.Vehicles = list;
            }

            _context.VehicleInfo.Add(veh);
            _context.SaveChanges();

        }

        private MitchellClaimType ReadUpdatefile()
        {
            XmlSerializer reader = new XmlSerializer(typeof(MitchellClaimType));
            System.IO.StreamReader file = new System.IO.StreamReader(
                @"\Assets\update-claim.xml");
            MitchellClaimType mitchellClaim = (MitchellClaimType)reader.Deserialize(file);
            file.Close();

            return mitchellClaim;
        }

        public VehicleInfoType GetVehicleFromClaim(string claimNumber, string vin)
        {
           try
            {
                var claim = _context.MitchellClaim.Include( c=> c.Vehicles).Where(c => c.ClaimNumber == claimNumber).SingleOrDefault();
                return claim.Vehicles.Where(v => v.Vin == vin).SingleOrDefault();
            }
            catch(Exception ex)
            {
                _logger.LogError("error", ex);
                return null;
            }
        }

        public void Delete(string claimNumber)
        {
            try
            {
                var claimToDelete = _context.MitchellClaim.Include(c => c.Vehicles).Include(c => c.LossInfo).Where(c => c.ClaimNumber == claimNumber).SingleOrDefault();
                var lossInfo = claimToDelete.LossInfo;
                var vehicles = claimToDelete.Vehicles;

                _context.LossInfo.Remove(lossInfo);

                foreach (var vehicle in vehicles)
                {
                    _context.VehicleInfo.Remove(vehicle);
                }

                _context.MitchellClaim.Remove(claimToDelete);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in delete claim ", ex);
            }
        }

        public bool SaveToDB()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}
