﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntoSport.Models
{
    public class Cart
    {
        public List<int> ProductIds = new List<int>();
        public List<int> ProductQuantity = new List<int>();
    }
}