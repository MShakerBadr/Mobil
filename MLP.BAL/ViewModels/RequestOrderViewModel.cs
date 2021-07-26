using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{

    public class RequestOrderViewModel
    {

        public int ID { get; set; }
        public string FK_RequestNo { get; set; }
        public int? FK_CenterID { get; set; }
        public int? FK_UserID { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreationDate { get; set; }
        public Nullable<DateTime> LastModifiedDate { get; set; }
        public int? FK_StatusID { get; set; }
        public string StatusName { get; set; }
        public string ArStatusName { get; set; }
        public int? FK_RequestType { get; set; }
        public string RequestTypeName { get; set; }
        public string ARRequestTypeName { get; set; }



    }
}
