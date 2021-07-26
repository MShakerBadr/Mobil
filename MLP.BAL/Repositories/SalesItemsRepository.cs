using MLP.BAL.BaseObjects;
using MLP.BAL.ViewModels;
using MLP.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.Repositories
{
    public class SalesItemsRepository : BaseRepository<SalesItem>
    {
         private MLPDB01Entities DataContext { get; set; }
         public SalesItemsRepository(DbContext context)
            : base(context)
        {
            DataContext = (MLPDB01Entities)context;
        }

        public List<SalesItemViewModel> GetAllSalesItem()
        {

            var list = DataContext.SalesItems.Select(s => new SalesItemViewModel {ID=s.ID , ItemName= s.ItemName+" - "+s.ItemCode }).ToList();
            return list;
        }
    }
}
