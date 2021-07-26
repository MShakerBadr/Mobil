using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class AvailableTimesBody
    {
        public string token { get; set; }
        public string ChoosenDate { get; set; }
        public int ServiceCenterID { get; set; }
        public string lang { get; set; }
        public List<int> Services { get; set; }
    }

    //public class ServicesData 
    //{
    //    public int ID { get; set; }
    //}
}
