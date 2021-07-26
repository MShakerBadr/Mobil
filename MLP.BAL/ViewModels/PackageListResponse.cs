using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class PackageListResponse
    {
        public int error { get; set; }

        public string message { get; set; }


        public List<PackagesList> data { get; set; }
    }
    public class PackagesList
    {
        public int ID { get; set; }
        public string PackageTitle { get; set; }
        public string PackageAbtract { get; set; }
        public string PackageHTML { get; set; }
        public string PackageImage { get; set; }
        public string PackageDate { get; set; }
    }
}
