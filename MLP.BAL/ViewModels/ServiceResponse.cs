using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class ServiceResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public List<Services> data { get; set; }
    }

    public class Services
    {

        public int ID { get; set; }

        public string SerivceName { get; set; }

        public decimal Price { get; set; }

        public string SerivceImage { get; set; }

        public string Description { get; set; }

        public string DescHTML { get; set; }
    }
}
