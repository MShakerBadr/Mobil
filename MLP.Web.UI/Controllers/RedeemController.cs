using MLP.BAL;
using MLP.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLP.Web.UI.Controllers
{
    public class RedeemController : Controller
    {
        UnitOfWork unitofwork = new UnitOfWork();
        //
        // GET: /Redeem/
        [Authorize]
        public ActionResult Index()
        {

            List<RedeemViewModel> RedeemListVM = LoadRedemptionList();
            return View(RedeemListVM);
        }
        [Authorize]
        public ActionResult Edit(int? id)
        {
            ViewBag.ServiceCenters = unitofwork.ServiceCenter.GetAll();

            var RedeemObj = unitofwork.RedemptionRequests.GetWhere(x => x.ID == id).FirstOrDefault();
            RedeemViewModel Redeem = new RedeemViewModel
            {
                ID = RedeemObj.ID,
                Customer = RedeemObj.Customer.FirstName + " " + RedeemObj.Customer.LastName,
                Award = unitofwork.SalesItem.GetWhere(x => x.ID == RedeemObj.Award.FK_SalesItemID).Select(x => x.ItemName).FirstOrDefault(),
                RedeemDate = RedeemObj.RedeemDate.ToString(),
                RedeemPoints = RedeemObj.RedeemPoints.Value
            };
            return View(Redeem);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(RedeemViewModel RedemRequets)
        {
            return RedirectToAction("Index");
        }
        [Authorize]
        private List<RedeemViewModel> LoadRedemptionList()
        {
            List<RedeemViewModel> RedeemListVM = new List<RedeemViewModel>();
            var RedeemList = unitofwork.RedemptionRequests.GetWhere(x => x.FK_RedeemStatusID == null).ToList();

            foreach (var item in RedeemList)
            {
                RedeemViewModel Redeem = new RedeemViewModel();
                Redeem.ID = item.ID;
                Redeem.Customer = item.Customer.FirstName + " " + item.Customer.LastName;
                Redeem.Award = unitofwork.SalesItem.GetWhere(x => x.ID == item.Award.FK_SalesItemID).Select(x => x.ItemName).FirstOrDefault();
                Redeem.RedeemDate = item.RedeemDate.ToString();
                Redeem.RedeemPoints = item.RedeemPoints.Value;
                Redeem.Qty = item.Award.Qty.Value;
                Redeem.ServiceCenter = item.FK_ServiceCenterID == null ? " " : item.ServiceCenter.CenterName;
                Redeem.FK_ServiceCenterID = item.FK_ServiceCenterID ?? 0;
                RedeemListVM.Add(Redeem);
            }

            return RedeemListVM;
        }
    }
}