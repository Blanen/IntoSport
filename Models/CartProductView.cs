using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntoSport.Models
{
    public class CartProductView
    {
        public Product Product { get; set; }
        public int Hoeveelheid { get; set; }
        public List<DetailWaarde> DetailWaardeList { get; set; }

    }
}