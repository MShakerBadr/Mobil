using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class LookupResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public List<BookingStatusData> Data { get; set; }
    }

    public class BookingStatusData
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}
