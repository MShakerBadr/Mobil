using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class BookingHistoryResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public List<BookingData> data { get; set; }
    }

    public class BookingData
    {
        public int ID { get; set; }

        public string ServiceCenterName { get; set; }
        public int ServiceCenterID { get; set; }

        public string Services { get; set; }
        public List<int> ServicesID { get; set; }

        public string CarNumber { get; set; }
        public int CarId { get; set; }

        public string BookingDate { get; set; }
        //public int FromId { get; set; }
        //public int Interval { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        public string Status { get; set; }

        public int StatusID { get; set; }
        public bool Editable { get; set; }
    }
}
