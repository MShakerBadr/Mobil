using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class AvailableTimesResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public List<TimeSlots> data { get; set; }
    }

    public class TimeSlots
    {
        public int FromID { get; set; }
        public int Inetrval { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }

    }
}
