using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class MessageObject
    {
        public int error { get; set; }

        public string message { get; set; }
        public  DAL.Booking data { get; set; }
    }
}
