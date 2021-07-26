using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class CarLookupViewModel
    {
        public CarLookupViewModel()
        {
            EngineTypes = new List<EngineTypeVM>();
            CarBrands = new List<CarBrandVm>();
        }
        public List<EngineTypeVM> EngineTypes { get; set; }
        public List<CarBrandVm> CarBrands { get; set; }
        public int error { get; set; }
        public string message { get; set; }
    }
    public class EngineTypeVM
    {
        public int Id { get; set; }
        public string EngineType { get; set; }
    }
    public class CarBrandVm
    {
        public CarBrandVm()
        {
            CarModels = new List<CarModelVm>();
        }
        public int Id { get; set; }
        public string CarBrand { get; set; }
        public List<CarModelVm> CarModels { get; set; }
    }
    public class CarModelVm 
    {
        public int Id { get; set; }
        public string CarModel { get; set; }
    }
    public class CarViewModel
    {
        public int Id { get; set; }
        public string CarNumber { get; set; }
        public string token { get; set; }
        public int modelId { get; set; }
        public int BrandId { get; set; }
        public string Year { get; set; }
        public string Motor { get; set; }
        public int EngineTypeId { get; set; }

        public int? error { get; set; }
        public string message { get; set; }

    }
}
