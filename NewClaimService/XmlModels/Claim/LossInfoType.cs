using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ClaimService.Models.Claim
{

    public enum CauseOfLossCode
    {
        Collision,
        Explosion,
        Fire,
        Hail,
        Mechanical_Breakdown,
        Other
    }

    [Serializable]
    [DataContract(Name = "LossInfo", Namespace = "http://mitchell.com/examples/claim")]
    public class LossInfoType
    {
       [DataMember(Name = "CauseOfLoss")]
        public CauseOfLossCode CauseOfLoss { get; set; }

        [DataMember(Name = "ReportedDate")]
        public DateTime ReportedDate { get; set; }

        [DataMember(Name = "LossDescription")]
        public string LossDescription { get; set; }
    }
}