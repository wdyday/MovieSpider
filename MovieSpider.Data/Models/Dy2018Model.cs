using MovieSpider.Data.DbEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Data.Models
{
    public class Dy2018Model
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public RegionEnum Country { get; set; }

        public MediaTypeEnum MediaType { get; set; }
    }
}
