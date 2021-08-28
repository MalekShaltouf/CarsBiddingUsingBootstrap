using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarsBiddingUsingBootstrap.Models.ViewModelClasses
{
    public class GalleryViewModel
    {
        public Nullable<int> ManufactureYearTextSearch { get; set; }
        public string CarColorTextSearch { get; set; }
        public Nullable<int> PriceTextSearch { get; set; }
        public string TypeOfTransmissionGearTextSearch { get; set; }
        public string CarTypeTextSearch { get; set; }
    }
}