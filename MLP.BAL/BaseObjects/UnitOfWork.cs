using MLP.BAL.BaseObjects.ReporistoriesInterfaces;
using MLP.BAL.Repositories;
using MLP.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL
{
    public class UnitOfWork : IUnitOfWork
    {

        private DbContext DataContext { get; set; }

        public MLPDB01Entities Context { get; private set; }
        public UnitOfWork()
        {
            DataContext = new MLPDB01Entities();
            Context = new MLPDB01Entities();
            Context.Database.CommandTimeout = 720;
            // Avoid load navigation properties
            //QuickWinDbContext.Configuration.LazyLoadingEnabled = false;
            DataContext.Database.CommandTimeout = 720;
            //QuickWinDbContext.Configuration.UseDatabaseNullSemantics = true;
            //QuickWinDbContext.Database.Connection.Database.
        }

        /// <summary>
        /// Commit changes of QuickWinDbContext
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            return DataContext.SaveChanges();
        }

        /// <summary>
        /// Dispose the UnitOfWork and QuickWinDbContext objects
        /// </summary>
        public void Dispose()
        {
            DataContext.Dispose();
        }

        /// <summary>
        /// Detect if QuickWinDbContext is traking any change 
        /// </summary>
        /// <returns>True if there is any change, else false</returns>
        public bool HasChanges()
        {
            return DataContext.ChangeTracker.HasChanges();
        }


        public CustomerRepository customer { get { return new CustomerRepository(DataContext); } }

        public CustomerCarsReporsitory customerCars { get { return new CustomerCarsReporsitory(DataContext); } }
        public CarModelReporsitory CarModel { get { return new CarModelReporsitory(DataContext); } }
        public CarBrandRepository CarBrand { get { return new CarBrandRepository(DataContext); } }
        public TempCustomerRepository Tempcustomer { get { return new TempCustomerRepository(DataContext); } }

        public EngineTypeReporsitory EngineType { get { return new EngineTypeReporsitory(DataContext); } }
        public SalesItemsRepository SalesItem { get { return new SalesItemsRepository(DataContext); } }

        public PromotionRepository Promotion { get { return new PromotionRepository(DataContext); } }

        public ServiceCenterRepository ServiceCenter { get { return new ServiceCenterRepository(DataContext); } }

        public MobileCategoryRepository MobileCategory { get { return new MobileCategoryRepository(DataContext); } }

        public AreaRepository cities { get { return new AreaRepository(DataContext); } }
        public CityAreaRepository Areas { get { return new CityAreaRepository(DataContext); } }

        public NewsRepository News { get { return new NewsRepository(DataContext); } }

        public LoginLogRepository LoginLog { get { return new LoginLogRepository(DataContext); } }

        public AwardsRepository Awards { get { return new AwardsRepository(DataContext); } }

        public LevelRepository Levels { get { return new LevelRepository(DataContext); } }

        public VehiclesRepository Vehicles { get { return new VehiclesRepository(DataContext); } }

        public InvoicesRepository Invoices { get { return new InvoicesRepository(DataContext); } }

        public MessageThreadRepository MessageThread { get { return new MessageThreadRepository(DataContext); } }

        public MessageCommentRepository MessageComment { get { return new MessageCommentRepository(DataContext); } }

        public InvoiceDetailsRepository InvoiceDetails { get { return new InvoiceDetailsRepository(DataContext); } }

        public RedemptionRequestsRepository RedemptionRequests { get { return new RedemptionRequestsRepository(DataContext); } }

        public ServiceCenterSalesItemRepository ServiceCenterSalesItems { get { return new ServiceCenterSalesItemRepository(DataContext); } }

        public TimeSlotsRepository TimeSlots { get { return new TimeSlotsRepository(DataContext); } }

        public BookingRepository Booking { get { return new BookingRepository(DataContext); } }

        public BookingStatusRepository BookingStatus { get { return new BookingStatusRepository(DataContext); } }

        public BookingServicesRepository BookingServices { get { return new BookingServicesRepository(DataContext); } }

        public BookingTimeSlotsRepository BookingTimeSlots { get { return new BookingTimeSlotsRepository(DataContext); } }

        public BookingCancelReasonsRepository BookingCancelReasons { get { return new BookingCancelReasonsRepository(DataContext); } }

        public UserRepository Users { get { return new UserRepository(DataContext); } }

        public PackageRepository Packages { get { return new PackageRepository(DataContext); } }

        public NotificationRepository Notifications { get { return new NotificationRepository(DataContext); } }

        public CustomerNotificationRepository CustomerNotification { get { return new CustomerNotificationRepository(DataContext); } }

        public BundlesRepository Bundles { get { return new BundlesRepository(DataContext); } }

        public ReturnInvoiceRepository ReturnInvoices { get { return new ReturnInvoiceRepository(DataContext); } }

        public ReturnInvoiceItemRepository ReturnInvoicesDetails { get { return new ReturnInvoiceItemRepository(DataContext); } }

        public RequestOrderRepository RequesOrder{ get { return new RequestOrderRepository(DataContext); } }
        public RequestItemsRepository RequesItems { get { return new RequestItemsRepository(DataContext); } }
        public NotificationcleardateRepository NotfifcationClearDate { get { return new NotificationcleardateRepository(DataContext); } }
        public MLPTokenRepository MLPTokenlst
        {
            get { return new MLPTokenRepository(DataContext); }
        }
        public CustomerSMSCheckRepository MLPSMSCheck
        {
            get { return new CustomerSMSCheckRepository(DataContext); }
        }
        public AdsRepository Ads
        {
            get { return new AdsRepository(DataContext); }
        }
        public AwardsTypesRepository AwardsTypes { get { return new AwardsTypesRepository(DataContext); } }
        public AlForsanRedemptionRepository AlForsanRedemption { get { return new AlForsanRedemptionRepository(DataContext); } }
        public InvoicePaymentsRepository InvoicePayments { get { return new InvoicePaymentsRepository(DataContext); } }
    }
}
