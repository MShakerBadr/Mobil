using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class CategoryListResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public List<Categ> data { get; set; }

    }
    public class Categ
    {
        public int id { get; set; }

        public string categoryname { get; set; }

        public string categorynamear { get; set; }

    }
}
