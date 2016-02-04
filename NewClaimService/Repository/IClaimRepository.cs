using System;
using System.Collections.Generic;
using NewClaimService.Models;

namespace NewClaimService.Repository
{
    public interface IClaimRepository
    {
        bool AddNewClaim(MitchellClaimType claim);
        void Delete(string claimNumber);
        MitchellClaimType GetClaimByClaimNumber(string claimNumber);
        IList<MitchellClaimType> GetClaimsByDate(DateTime lossDate1, DateTime lossDate2);
        VehicleInfoType GetVehicleFromClaim(string claimNumber, string vin);
        IList<MitchellClaimType> Retrieve();
        bool UpdateClaim(MitchellClaimType claimPassIn);

        void AddVehicle(string claimNumber, VehicleInfoType veh);
        bool UpdateVehicle(string claimNumber, VehicleInfoType newVehicle);

        bool SaveToDB();
    }
}