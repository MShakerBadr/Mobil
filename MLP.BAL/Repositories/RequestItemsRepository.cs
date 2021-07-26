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
    public class RequestItemsRepository : BaseRepository<RequestItem>
    {
        private MLPDB01Entities DataContext { get; set; }
        public RequestItemsRepository(DbContext context)
            : base(context)
        {
            DataContext = (MLPDB01Entities)context;
        }

        public List<RequestItemsViewModel>  GetAllRequestItemsList(int? ID)
        {
            var res = (from ri in DataContext.RequestItems
                       join ro in DataContext.RequestOrders
                       on ri.FK_RequestNo equals ro.FK_RequestNo
                       where ro.ID == ID
                        orderby ri.ID descending
                       select new RequestItemsViewModel{
                           ID=  ri.ID,
                           SalesItemID= ri.FK_SalesItemID ,
                           Quantity= ri.Quantity  ,
                           ItemName=ri.SalesItem.ItemName + " - " + ri.SalesItem.ItemCode,
                           FK_RequestNo =ri.FK_RequestNo
                       }).ToList();
            return res;
        }

        public bool RemoveRange(List<RequestItem> removedList)
        {
            DataContext.RequestItems.RemoveRange(removedList);
            DataContext.SaveChanges();
            return true;
        }
    }
}
