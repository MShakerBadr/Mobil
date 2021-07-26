using Microsoft.AspNet.Identity.EntityFramework;

namespace MLP.Web.UI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //public int UserID { get; set; }
        //public int ServiceCenterID { get; set; }
        //public string UserName { get; set; }
        //public string Password { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MLPDB01Entities")
        {
        }

        public System.Data.Entity.DbSet<MLP.Web.UI.Models.RedeemViewModel> RedeemViewModels { get; set; }

        public System.Data.Entity.DbSet<MLP.Web.UI.Models.BookingViewModel> BookingViewModels { get; set; }
        
        public System.Data.Entity.DbSet<MLP.DAL.ContractUser> ContractUsers { get; set; }
        public System.Data.Entity.DbSet<MLP.DAL.Contract> Contracts{ get; set; }

        public System.Data.Entity.DbSet<MLP.DAL.ContractCar> ContractCars { get; set; }
    }
}