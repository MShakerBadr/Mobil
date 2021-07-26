using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    
    public class AdvertisementResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public List<AdvertisementsData> data { get; set; }

    }
    public class AdvertisementsData
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public Nullable<int> Order { get; set; }
        public string CreationDate { get; set; }
        public string Lastmodifieddate { get; set; }

    }
 
}
