using FlexGridExplorer.Resources;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FlexGridExplorer
{
    public class Car
    {
        [Display(Name = nameof(AppResources.BrandLabel), ResourceType = typeof(AppResources))]
        public string Brand { get; set; }

        [Display(Name = nameof(AppResources.ModelLabel), ResourceType = typeof(AppResources))]
        public string Model { get; set; }

        [Display(Name = nameof(AppResources.PriceLabel), ResourceType = typeof(AppResources))]
        public double Price { get; set; }

        [Display(Name = nameof(AppResources.CategoryLabel), ResourceType = typeof(AppResources))]
        public string Category { get; set; }

        [Display(Name = nameof(AppResources.TransmissSpeedCountLabel), ResourceType = typeof(AppResources))]
        public string TransmissSpeedCount { get; set; }

        [Display(Name = nameof(AppResources.TransmissAutomaticLabel), ResourceType = typeof(AppResources))]
        public bool? TransmissAutomatic { get; set; }

        [Browsable(false)]
        public int ID { get; set; }
        [Browsable(false)]
        public Int16 HP { get; set; }
        [Browsable(false)]
        public double Liter { get; set; }
        [Browsable(false)]
        public Int16 Cyl { get; set; }
        [Browsable(false)]
        public Int16 MPG_City { get; set; }
        [Browsable(false)]
        public Int16 MPG_Highway { get; set; }
        [Browsable(false)]
        public string Description { get; set; }
        [Browsable(false)]
        public string Hyperlink { get; set; }
        [Browsable(false)]
        public byte[] Picture { get; set; }
    }
}