using MLP.BAL.BaseObjects;
using MLP.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.Repositories
{
    public class TempCustomerRepository : BaseRepository<TempCustomer>
    {
        private MLPDB01Entities DataContext { get; set; }
        UnitOfWork unitofwork = new UnitOfWork();
        public TempCustomerRepository(DbContext context)
            : base(context)
        {
            DataContext = (MLPDB01Entities)context;
        }


        public int ValidateConfirmationCode(string mobile, string code)
        {
            //  string modifiedMobile = mobile.Substring(3, (mobile.Length - 3));
            //var ServiceCenterObj = DataContext.ServiceCenters.Where(s => s.CenterName.Contains("Warehouse")).FirstOrDefault();
            var ServiceCenterObj = DataContext.ServiceCenters.Where(s => s.ID==22221).FirstOrDefault();
            int? NewSCID = DataContext.Customers.Where(c => c.FK_ServiceCenterID == ServiceCenterObj.ID).Select(c => c.CustomerSCID).DefaultIfEmpty().Max();

            var customer = DataContext.Customers.Where(c => c.Mobile==mobile).FirstOrDefault();
            var validate = DataContext.TempCustomers.Where(c => c.Mobile == mobile
                          && c.ConfirmationCode == code).FirstOrDefault();
            if (customer == null)
            {
                if (validate != null)
                {
                    Customer cust = new Customer();
                    cust.FirstName = validate.FirstName;
                    cust.LastName = validate.LastName;
                    cust.Mobile = validate.Mobile;
                    cust.Email = validate.Email;
                    cust.Area = validate.Area;
                    cust.FK_CityID = validate.FK_CityID;
                    cust.ConfirmationCode = validate.ConfirmationCode;
                    cust.Password = validate.Password;
                    cust.IsMobileUser = true;
                    cust.IsFristlogin = true;
                    cust.FK_AreaID = validate.FK_AreaID;
                    cust.FK_UserID = 19;
                    cust.Gender = validate.Gender;
                    cust.BirthDate = validate.BirthDate;
                    //Service Center 
                    cust.FK_ServiceCenterID = ServiceCenterObj.ID;
                    //SCID
                    cust.CustomerSCID = (NewSCID == null ? 0 : NewSCID) + 1;
                    //CustmerCode
                    cust.CustomerCode = mobile;

                    //Current Points to be 50 
                    cust.CurrentPoints = 50;
                    cust.CreationDate = DateTime.Now;
                    cust.LastModifiedDate = DateTime.Now;
                    cust.FirstLoginDate = DateTime.Now;
                    DataContext.Customers.Add(cust);
                    DataContext.SaveChanges();

                    DataContext.TempCustomers.Remove(validate);

                    DataContext.SaveChanges();

                    return 1;

                }
                else
                {
                    return 0;
                }
            }
            else
            {

                if (validate != null)
                {
                    //customer.
                    customer.IsMobileUser = true;
                    customer.FirstLoginDate = DateTime.Now;
                    customer.LastModifiedDate = DateTime.Now;
                    customer.Gender = validate.Gender;
                    customer.BirthDate = validate.BirthDate;
                    customer.Email = validate.Email;
                    customer.Area = validate.Area;
                    customer.FK_CityID = validate.FK_CityID;
                    customer.CurrentPoints += 50;
                    unitofwork.customer.Update(customer);
                    unitofwork.Commit();
                    DataContext.TempCustomers.Remove(validate);
                    DataContext.SaveChanges();
                    return 1;
                }
                return 0;
            }

        }
    }
}
