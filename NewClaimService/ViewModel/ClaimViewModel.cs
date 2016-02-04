using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewClaimService.Models;

namespace NewClaimService.ViewModel
{
    public class ClaimViewModel
    {
        public string ClaimNumber { get; set; }
        public string ClaimantFirstName { get; set; }
        public string ClaimantLastName { get; set; }

        public StatusCode Status { get; set; }
        public DateTime LossDate { get; set; }
        public virtual LossInfoViewModel LossInfo { get; set; }
        public long AssignedAdjusterID { get; set; }

        public IEnumerable<VehicleViewModel> Vehicles { get; set; }
        
    }
}
