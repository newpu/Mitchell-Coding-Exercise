using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewClaimService.Models;

namespace NewClaimService.ViewModel
{
    public class LossInfoViewModel
    {
        public CauseOfLossCode CauseOfLoss { get; set; }
        public DateTime ReportedDate { get; set; }
        public string LossDescription { get; set; }
    }
}
