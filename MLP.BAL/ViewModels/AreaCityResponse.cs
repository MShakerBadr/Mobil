using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class AreaCityResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public List<cities> data { get; set; }
    }
    public class CityArea
    {
        public int id { get; set; }
        public string AreaName { get; set; }
    }
    public class cities
    {
        public int id { get; set; }

        public string cityname { get; set; }
        public List<CityArea> Areas { get; set; }
    }


}
