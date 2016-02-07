using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimClient.Models
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
    public class LossInfoType
    {
        public int Id { get; set; }

        public CauseOfLossCode CauseOfLoss { get; set; }
        public DateTime ReportedDate { get; set; }
        public string LossDescription { get; set; }
    }
}
