using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace NewClaimService.ViewModel
{
    public class VehicleViewModel
    {
        [Required]
        public int ModelYear { get; set; }
        public string MakeDescription { get; set; }
        public string ModelDescription { get; set; }

        public string EngineDescription { get; set; }
        public string ExteriorColor { get; set; }
        public string Vin { get; set; }
        public string LicPlate { get; set; }
        public string LicPlateState { get; set; }
        public DateTime LicPlateExpDate { get; set; }
        public string DamageDescription { get; set; }
        public int Mileage { get; set; }
    }
}
