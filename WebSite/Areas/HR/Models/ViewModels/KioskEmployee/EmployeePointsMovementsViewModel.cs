using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.HR.Models.ViewModels.KioskEmployee
{
    public class EmployeePointsMovementsViewModel
    {
        public List<KioskUserPendingPointsExchange> InterchangedTicketsList;
        public List<KioskUserPointsMovement> UserPointsMovementList;
        public List<KioskExchangeableItem> ItemsList;
        public bool EnableExchangePetitions;
        public int AvailablePoints;
        public int PetitionPoints;

        public EmployeePointsMovementsViewModel()
        {
            InterchangedTicketsList = new List<KioskUserPendingPointsExchange>();
            UserPointsMovementList = new List<KioskUserPointsMovement>();
            ItemsList = new List<KioskExchangeableItem>();
            EnableExchangePetitions = false;
            AvailablePoints = 0;
            PetitionPoints = 0;
        }
    }
}