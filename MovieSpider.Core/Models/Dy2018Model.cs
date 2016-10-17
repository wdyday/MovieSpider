﻿using MovieSpider.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Models
{
    public class Dy2018Model
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public CountryEnum Country { get; set; }
    }
}