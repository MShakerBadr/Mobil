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
    public class CustomerRepository : BaseRepository<Customer>
    {
        private MLPDB01Entities DataContext { get; set; }
        public CustomerRepository(DbContext context)
            : base(context)
        {
            DataContext = (MLPDB01Entities)context;
        }

        public Customer Checklogincredentials(string mobil, string password)
        {
            //string modifiedMobile = mobil.Substring(3, (mobil.Length - 3));
            //Customer UserObj = DataContext.Customers.Where(c => c.Mobile.Contains(modifiedMobile) && c.Password == password && c.IsMobileUser == true).FirstOrDefault();
            Customer UserObj = DataContext.Customers.Where(c => c.Mobile==mobil && c.Password == password && c.IsMobileUser == true).FirstOrDefault();

            return UserObj;
        }


        public LoginResponse GetCustomizeUserData(string mobil, string password)
        {
            LoginResponse resp = new LoginResponse();
            string modifiedMobile = mobil.Substring(3, (mobil.Length - 3));
            Customer Userdata = DataContext.Customers.Where(c => c.Mobile.Contains(modifiedMobile) && c.Password == password).FirstOrDefault();

            resp.data = new UserData();

            var LevelObj = DataContext.LoyaltyLevels.Where(l => l.FromPoints <= Userdata.CurrentPoints && ((l.ToPoints == null) || (l.ToPoints >= Userdata.CurrentPoints))).FirstOrDefault();
            resp.data.usercode = Userdata.CustomerCode ?? string.Empty;
            resp.data.fname = Userdata.FirstName ?? string.Empty;
            resp.data.lname = Userdata.LastName ?? string.Empty;
            resp.data.points = Userdata.CurrentPoints ?? 0;
            resp.data.mail = Userdata.Email ?? string.Empty;
            resp.data.mobile = Userdata.Mobile;
            resp.data.city = Userdata.City.CityName ?? string.Empty;
            resp.data.cityar = Userdata.City.CityNameAR ?? string.Empty;
            //resp.data.address = string.Empty;
            resp.data.CityId = Userdata.FK_CityID;
            resp.data.AreaId = Userdata.FK_AreaID;
            resp.data.area = Userdata.Area ?? string.Empty;
            if (Userdata.FK_AreaID != null)
                resp.data.areaar = Userdata.Area1.AreaNameAR ?? string.Empty;
            else
                resp.data.areaar = "";
            //resp.data.levelname = DataContext.LoyaltyLevels.Where(l => l.ID == Userdata.FK_LatestLevelID).Select(l => l.LevelEng).FirstOrDefault() ?? string.Empty;
            resp.data.levelname = DataContext.LoyaltyLevels.Where(l => l.FromPoints <= Userdata.CurrentPoints && ((l.ToPoints == null) || (l.ToPoints >= Userdata.CurrentPoints))).Select(l => l.LevelEng).FirstOrDefault() ?? string.Empty;
            resp.data.brithdate = Userdata.BirthDate == null ? string.Empty : ((DateTime)Userdata.BirthDate).ToString("dd/MM/yyyy");
            resp.data.gender = Userdata.Gender ?? string.Empty;
            resp.data.IsFristLogin = (bool)Userdata.IsFristlogin;
            if (LevelObj != null)
            {
                if (LevelObj.ID == 1)
                    resp.data.BarLevel = Convert.ToInt32(((double)Userdata.CurrentPoints.Value / ((double)LevelObj.ToPoints - LevelObj.FromPoints)).Value * 10);
                else
                {
                    int PointsToAdded = 0;
                    for (int i = LevelObj.ID; i > 1; i--)
                    {
                        var Level = DataContext.LoyaltyLevels.Where(l => l.ID == i).FirstOrDefault();
                        var PrevLevel = DataContext.LoyaltyLevels.Where(l => l.ID == (i - 1)).FirstOrDefault();
                        if (i == LevelObj.ID)
                            PointsToAdded = Level.ToPoints.Value;

                        PointsToAdded += (PrevLevel.ToPoints - PrevLevel.FromPoints).Value;
                    }

                    resp.data.BarLevel = Convert.ToInt32(((double)Userdata.CurrentPoints.Value / (double)PointsToAdded) * 10);
                }
            }
            resp.data.AlfursanNo = Userdata.AlfursanNo ?? string.Empty;
            return resp;

        }

        public CustomerData GetCustomizeUserData(int CustomerID)
        {
            CustomerData resp = new CustomerData();
            Customer Userdata = DataContext.Customers.Where(c => c.ID == CustomerID).FirstOrDefault();
            var LevelObj = DataContext.LoyaltyLevels.Where(l => l.FromPoints <= Userdata.CurrentPoints && ((l.ToPoints == null) || (l.ToPoints >= Userdata.CurrentPoints))).FirstOrDefault();

            resp.usercode = Userdata.CustomerCode ?? string.Empty;
            resp.fname = Userdata.FirstName ?? string.Empty;
            resp.lname = Userdata.LastName ?? string.Empty;
            resp.points = Userdata.CurrentPoints ?? 0;
            resp.mail = Userdata.Email ?? string.Empty;
            resp.mobile = Userdata.Mobile;
            resp.city = Userdata.City.CityName ?? string.Empty;
            resp.cityar = Userdata.City.CityNameAR ?? string.Empty;
            resp.CityId = Userdata.FK_CityID ?? 0;
            //resp.data.address = string.Empty;
            resp.area = Userdata.Area ?? string.Empty;
            if (Userdata.FK_AreaID != null)
                resp.areaar = Userdata.Area1.AreaNameAR ?? string.Empty;
            else
                resp.areaar = "";
            resp.AreaId = Userdata.FK_AreaID ?? 0;
            //resp.levelname = DataContext.LoyaltyLevels.Where(l => l.ID == Userdata.FK_LatestLevelID).Select(l => l.LevelEng).FirstOrDefault() ?? string.Empty;
            //resp.levelname = (Userdata.CurrentPoints >= 0 && Userdata.CurrentPoints <= 3500) ? "Silver" : ((Userdata.CurrentPoints >= 3500 && Userdata.CurrentPoints <= 10000) ? "Gold" : "Platinum");
            resp.levelname = DataContext.LoyaltyLevels.Where(l => l.FromPoints <= Userdata.CurrentPoints && ((l.ToPoints == null) || (l.ToPoints >= Userdata.CurrentPoints))).Select(l => l.LevelEng).FirstOrDefault() ?? string.Empty;
            resp.brithdate = Userdata.BirthDate == null ? string.Empty : ((DateTime)Userdata.BirthDate).ToString("dd/MM/yyyy");
            resp.gender = Userdata.Gender ?? string.Empty;

            if (LevelObj != null)
            {
                if (LevelObj.ID == 1)
                    resp.BarLevel = Convert.ToInt32(((double)Userdata.CurrentPoints.Value / ((double)LevelObj.ToPoints - LevelObj.FromPoints)).Value * 10);
                else
                {
                    int PointsToAdded = 0;
                    for (int i = LevelObj.ID; i > 1; i--)
                    {
                        var Level = DataContext.LoyaltyLevels.Where(l => l.ID == i).FirstOrDefault();
                        var PrevLevel = DataContext.LoyaltyLevels.Where(l => l.ID == (i - 1)).FirstOrDefault();
                        if (i == LevelObj.ID)
                            PointsToAdded = Level.ToPoints.Value;

                        PointsToAdded += (PrevLevel.ToPoints - PrevLevel.FromPoints).Value;
                    }

                    resp.BarLevel = Convert.ToInt32(((double)Userdata.CurrentPoints.Value / (double)PointsToAdded) * 10);
                }
            }

            resp.AlfursanNo = Userdata.AlfursanNo;

            return resp;

        }

        public int CheckRegistrationbymobil(string mobile, string password)
        {
            //string modifiedMobile = mobile.Substring(3, (mobile.Length - 3));
            //Customer UserObj = DataContext.Customers.Where(c => c.Mobile.Contains(modifiedMobile)).FirstOrDefault();
            Customer UserObj = DataContext.Customers.Where(c => c.Mobile == mobile).FirstOrDefault();
            if (UserObj == null)
            {
                return 1;
            }
            else if ((UserObj.IsMobileUser == false) || UserObj.IsMobileUser == null)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        public void UpdateConfirmationCode(string Mobile, string Code, string password)
        {
            //string modifiedMobile = Mobile.Substring(3, (Mobile.Length - 3));
            //Customer UserObj = DataContext.Customers.Where(c => c.Mobile.Contains(modifiedMobile)).FirstOrDefault();
            Customer UserObj = DataContext.Customers.Where(c => c.Mobile==Mobile).FirstOrDefault();
            UserObj.Password = password;

            UserObj.ConfirmationCode = Code;
            UserObj.Mobile = Mobile;
            DataContext.SaveChanges();
        }

        public int ValidateConfirmationCode(string mobile, string code)
        {
            //string modifiedMobile = mobile.Substring(3, (mobile.Length - 3));
            //Customer validate = DataContext.Customers.Where(c => c.Mobile.Contains(mobile) && c.confirmationCode == code).FirstOrDefault();
            Customer validate = DataContext.Customers.Where(c => c.Mobile==mobile && c.ConfirmationCode == code).FirstOrDefault();
            if (validate != null)
            {
                validate.IsMobileUser = true;
                validate.IsFristlogin = true;
                validate.FirstLoginDate = DateTime.Now;
                //Current Points to be 50 
                //if (validate.CurrentPoints ==null)
                //{
                    validate.CurrentPoints = 50;
                //}
                //else
                //{
                //    validate.CurrentPoints += 50;
                //}
                
                DataContext.SaveChanges();
                return 1;

            }
            else
            {
                return 0;
            }
        }

    }
}
