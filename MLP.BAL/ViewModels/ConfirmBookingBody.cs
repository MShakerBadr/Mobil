using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class ConfirmBookingBody
    {
        public string token { get; set; }
        public string CarCode { get; set; }
        public int ServiceCenterID { get; set; }
        public string ChoosenDate { get; set; }
        public int FromID { get; set; }
        public int Inetrval { get; set; }
        public List<int> Services { get; set; }

    }
    public class EditBookingBody 
    {
        public int Id { get; set; }
        public string token { get; set; }
        public string CarCode { get; set; }
        public int ServiceCenterID { get; set; }
        public string ChoosenDate { get; set; }
        public int FromID { get; set; }
        public int Inetrval { get; set; }
        public List<int> Services { get; set; }
    
    }
}
