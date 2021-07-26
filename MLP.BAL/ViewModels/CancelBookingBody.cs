using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class CancelBookingBody
    {
        public string token { get; set; }
        public int BookingID { get; set; }
        public int ReasonID { get; set; }
    }
}
