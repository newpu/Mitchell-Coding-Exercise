using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace ClaimService.Models.Claim
{
    [Serializable]
    [DataContract(Name = "VehicleDetails", Namespace = "http://mitchell.com/examples/claim")]
    public class VehicleInfoType
    {
        [DataMember(Name = "Vin")]
        public string Vin { get; set; }

        [DataMember(Name = "ModelYear")]
        public int ModelYear { get; set; }

        [DataMember(Name = "MakeDescription")]
        public string MakeDescription { get; set; }

        [DataMember(Name = "ModelDescription")]
        public string ModelDescription { get; set; }


        [DataMember(Name = "EngineDescription")]
        public string EngineDescription { get; set; }

        [DataMember(Name = "ExteriorColor")]
        public string ExteriorColor { get; set; }

        [DataMember(Name = "LicPlate")]
        public string LicPlate { get; set; }

        [DataMember(Name = "LicPlateState")]
        public string LicPlateState { get; set; }

        [DataMember(Name = "LicPlateExpDate")]
        public DateTime LicPlateExpDate { get; set; }

        [DataMember(Name = "DamageDescription")]
        public string DamageDescription { get; set; }

        [DataMember(Name = "Mileage")]
        public int Mileage { get; set; }

    }
}