using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace NewClaimService.Models
{
    public class ClaimContext : DbContext
    {
        public ClaimContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<MitchellClaimType> MitchellClaim { get; set; }
        public DbSet<VehicleInfoType> VehicleInfo { get; set; }
        public DbSet<LossInfoType> LossInfo { get; set; }

        protected override void OnConfiguring(EntityOptionsBuilder optionsBuilder)
        {
            //var connString = Startup.Configuration["Data:ClaimContextConnection"];
            //var connString = "Server=(localdb)\\ProjectsV12;Database=ClaimDB;Trusted_Connection=;MultipleActiveResultSets=true;";

            var connString = "Server=(localdb)\\v11.0;Database=ClaimDB2;Trusted_Connection=true;MultipleActiveResultSets=true;";
            optionsBuilder.UseSqlServer(connString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
