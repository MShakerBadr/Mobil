using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class VehiclesListResponse
    {
        public int error { get; set; }

        public string message { get; set; }
        //  [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<VehiclesData> Vehicles { get; set; }
    }
    public class AddVehiclesResponse
    {
        public int error { get; set; }
        public string message { get; set; }
    }
    public class VehiclesData
    {
        public int ID { get; set; }
        public string CarNumber { get; set; }
        public string CarCode { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Vendor { get; set; }
        public string Motor { get; set; }
        public string EngineType { get; set; }

    }
}
