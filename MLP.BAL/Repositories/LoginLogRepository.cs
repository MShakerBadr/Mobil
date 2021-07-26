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
    public class LoginLogRepository : BaseRepository<LoginLog>
    {
        private MLPDB01Entities DataContext { get; set; }

        public LoginLogRepository(DbContext Context)
            : base(Context)
        {
            DataContext = (MLPDB01Entities)Context;
        }

        public int GetCustomerIdByToken(string token)
        {
            try
            {
                return DataContext.LoginLogs.Where(l => l.Token == token).FirstOrDefault().FK_CustomerID;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        public bool CheckTokenValidty(string token)
        {
            bool IsValid = true;
            try
            {
                DateTime? TokenTime = (token != string.Empty) ? Convert.ToDateTime(DateTime.Now.ToShortDateString()) : (DateTime?)null;

                var Check = DataContext.LoginLogs.Where(l => l.Token == token && l.LogDate == TokenTime).FirstOrDefault();
                if (Check != null)
                {
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                }

            }
            catch (Exception ex)
            {

                IsValid = false;
            }
            return IsValid;
        }
    }
}
