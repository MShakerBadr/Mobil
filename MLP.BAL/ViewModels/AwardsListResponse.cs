using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class AwardsListResponse
    {
        public int error { get; set; }

        public string message { get; set; }
        public List<AwardsData> Awards { get; set; }
    }
    public class AwardsData
    {
        public int ID { get; set; }
        public string AwardImage { get; set; }
        public string Product { get; set; }
        public int Qty { get; set; }
        public int AwardDuePoints { get; set; }
        public string Level { get; set; }
        public bool Enabled { get; set; }
        public int AwardTypeID { get; set; }
        public string AwardTypeName { get; set; }

    }
}
