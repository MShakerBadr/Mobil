using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class AccountResponse
    {
    }
    public class RegistrtionParams
    {
        public string fname { get; set; }

        public string lname { get; set; }

        public string dateofbrith { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public int Area { get; set; }

        public int city { get; set; }

        public string password { get; set; }

        public string gender { get; set; }

        public string AlfursanNo { get; set; }
    }

    public class VerficationParams
    {
        public string mobile { get; set; }

        public string code { get; set; }
    }
}
