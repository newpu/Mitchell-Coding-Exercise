using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewClaimService.Models
{
    public class ClaimContextSeeder
    {
        private ClaimContext _context;

        public ClaimContextSeeder(ClaimContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            var mitchellClaim = CreateOne();

            if (_context.MitchellClaim.Any(c => c.ClaimNumber == mitchellClaim.ClaimNumber) == false)
            {
                _context.VehicleInfo.AddRange(mitchellClaim.Vehicles);
                _context.LossInfo.Add(mitchellClaim.LossInfo);
                _context.MitchellClaim.Add(mitchellClaim);
                _context.SaveChanges();
            }
        }

        //private MitchellClaimType readXml()
        //{
        //    XmlSerializer reader = new XmlSerializer(typeof(MitchellClaimType));
        //    System.IO.StreamReader file = new System.IO.StreamReader($"\\Assets\\create-claim.xml");
        //    MitchellClaimType MitchellClaim = (MitchellClaimType)reader.Deserialize(file);
        //    file.Close();

        //    return MitchellClaim;
        //}

        private MitchellClaimType CreateOne()
        {
            var claim = new MitchellClaimType()
            {
                Id = 1,
                ClaimNumber = "22c9c23bac142856018ce14a26b6c299",
                ClaimantFirstName = "George",
                ClaimantLastName = "Washington",
                Status = StatusCode.OPEN,
                LossDate = Convert.ToDateTime("2014-07-09T17:19:13.631-07:00"),
                AssignedAdjusterID = 12345,
            };

            claim.LossInfo = new LossInfoType()
            {
                Id = 1,
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
    }
}
