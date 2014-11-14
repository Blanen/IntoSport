using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntoSport.Models
{
    public class CartProduct
    {
       
        public int ID { get; set; }
        [Range(1, Int32.MaxValue)]
        public int Quantity { get; set; }
        public List<DetailWaarde> DetailWaardeList { get; set; }

        public CartProduct(){
            DetailWaardeList = new List<DetailWaarde>();
           
    }
    }

  


}