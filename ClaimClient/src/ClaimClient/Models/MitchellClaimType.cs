using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaimClient.Models
{
    public enum StatusCode
    {
        OPEN,
        CLOSED
    }
    public class MitchellClaimType
    {
        public int Id { get; set; }

        [Required]
        public string ClaimNumber { get; set; }
        public string ClaimantFirstName { get; set; }
        public string ClaimantLastName { get; set; }
        public StatusCode Status { get; set; }
        public DateTime LossDate { get; set; }
        public virtual LossInfoType LossInfo { get; set; }
        public long AssignedAdjusterID { get; set; }
       
        [Required]
       public virtual ICollection<VehicleInfoType> Vehicles { get; set; } 

    }
}
