using MLP.BAL.BaseObjects;
using MLP.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLP.BAL.ViewModels;

namespace MLP.BAL.Repositories
{
    public class RequestOrderRepository : BaseRepository<RequestOrder>
    {
        private MLPDB01Entities DataContext { get; set; }
        public RequestOrderRepository(DbContext context)
            : base(context)
        {
            DataContext = (MLPDB01Entities)context;
        }

        public List<RequestOrderViewModel> GetAllRequesOrder( int ServiceCenterID)
        {
           
            List<RequestOrderViewModel> resList = (from ro in DataContext.RequestOrders
                        join rt in DataContext.RequestTypes on ro.FK_RequestType equals rt.ID
                        join rs in DataContext.RequestStatus on ro.FK_StatusID equals rs.ID
                         join u in DataContext.Users on ro.FK_UserID equals u.ID
                        where ro.FK_RequestType == 1  && ro.FK_CenterID== ServiceCenterID
                         orderby ro.CreationDate descending
                        select new RequestOrderViewModel
                        {
                            ID = ro.ID,
                            FK_RequestNo=ro.FK_RequestNo,
                            FK_CenterID= ro.FK_CenterID,
                            FK_UserID= ro.FK_UserID,
                            Creator=u.UserName,
                            CreationDate= ro.CreationDate,
                            LastModifiedDate=ro.LastModifiedDate,
                            FK_StatusID=ro.FK_StatusID,
                            StatusName= rs.StatusName,
                            ArStatusName=rs.ArStatusName,
                            FK_RequestType=ro.FK_RequestType,
                            RequestTypeName=rt.RequestTypeName,
                            ARRequestTypeName=rt.ARRequestTypeName
                        }
                         ).ToList();

            return resList;



        }
    }
}
