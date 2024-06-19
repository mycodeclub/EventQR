﻿using System.ComponentModel.DataAnnotations.Schema;

namespace EventQR.ViewModels.Reports
{
    public class GuestVM
    {
         
        public Guid GuestId { get; set; }
        public string Name { get; set; }

        public List<SubEventVM> MySubEvents { get; set; }

    }
}
