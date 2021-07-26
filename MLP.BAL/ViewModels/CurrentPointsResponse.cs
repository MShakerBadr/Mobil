using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class CurrentPointsResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public int CurrentPoints { get; set; }
    }
}
