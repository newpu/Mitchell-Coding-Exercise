using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace ClaimService.Models.Claim
{
    [Serializable]
    [CollectionDataContract(Name = "Vehicles", ItemName = "VehicleDetails", Namespace = "http://mitchell.com/examples/claim")]
    public class VehicleListType
    {

        public VehicleListType ()
	    {
            VehicleDetails = new List<VehicleInfoType>();
	    }

        [Required]
        public virtual ICollection<VehicleInfoType> VehicleDetails { get; set; } 
    }

   
}