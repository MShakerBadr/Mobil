using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class CancelReasonsResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public List<CancelReasonsData> data { get; set; }
    }
    public class CancelReasonsData
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
