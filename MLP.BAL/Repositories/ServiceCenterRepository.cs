using MLP.BAL.BaseObjects;
using MLP.BAL.ViewModels;
using MLP.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace MLP.BAL.Repositories
{
    public class ServiceCenterRepository : BaseRepository<ServiceCenter>
    {
        private MLPDB01Entities DataContext { get; set; }
        public ServiceCenterRepository(DbContext context)
            : base(context)
        {
            DataContext = (MLPDB01Entities)context;
        }


        public ServiceCenterListResponse GetServiceCenters(int sort, string lang, string SearchKeyword = null)
        {
            ServiceCenterListResponse resp = new ServiceCenterListResponse();
            resp.error = 0;
            resp.message = "Success";
            resp.data = new List<ServiceCenters>();


            var List = (from s in DataContext.ServiceCenters
                        join c in DataContext.Cities on s.FK_CityID equals c.ID
                        where s.IsActive == true && !s.CenterName.Contains("Warehouse") && s.IsInLoyaltyProgram == true
                        select new
                        {
                            s.ID,
                            s.CenterName,
                            s.CenterNameAr,
                            s.Address,
                            s.AddressAR,
                            c.CityName,
                            c.CityNameAR,
                            s.Area,
                            s.AreaAr,
                            s.px,
                            s.py,
                            s.FK_CityID,
                            s.DescriptionHTML,
                            s.DescriptionHTMLAR,
                            s.POSImage,
                            s.OpeningTime,
                            s.ClosingTime,
                            s.ContactNo,
                            s.OpenningDate
                        }
                          ).ToList();

            if (SearchKeyword != null)
            {
                List = (from s in DataContext.ServiceCenters
                        join c in DataContext.Cities on s.FK_CityID equals c.ID
                        where s.IsActive == true && !s.CenterName.Contains("Warehouse") && s.IsInLoyaltyProgram == true && ((s.CenterName.Contains(SearchKeyword)) || (s.CenterNameAr.Contains(SearchKeyword)) ||
                        (s.Address.Contains(SearchKeyword)) || (s.AddressAR.Contains(SearchKeyword)) || (s.Area.Contains(SearchKeyword)) ||
                        (s.AreaAr.Contains(SearchKeyword)) || (c.CityName.Contains(SearchKeyword)) || (c.CityNameAR.Contains(SearchKeyword)) ||
                        (c.Region.RegionName.Contains(SearchKeyword)) || (c.Region.RegionNameAr.Contains(SearchKeyword)))
                        select new { s.ID, s.CenterName, s.CenterNameAr, s.Address, s.AddressAR, c.CityName, c.CityNameAR, s.Area, s.AreaAr, s.px, s.py, s.FK_CityID, s.DescriptionHTML, s.DescriptionHTMLAR, s.POSImage, s.OpeningTime, s.ClosingTime, s.ContactNo, s.OpenningDate }).ToList();
            }


            if (sort == 1)
            {
                List = List.OrderBy(o => o.CenterName).ToList();
                //if (List.Count > 1)
                //{
                foreach (var item in List)
                {
                    ServiceCenters cen = new ServiceCenters();
                    cen.id = item.ID;
                    if (lang == "ar")
                    {
                        cen.centername = item.CenterNameAr ?? string.Empty;
                        cen.address = item.AddressAR ?? string.Empty;
                        cen.city = item.CityNameAR ?? string.Empty;
                        cen.area = item.AreaAr ?? string.Empty;
                        cen.DescHTML = item.DescriptionHTMLAR ?? string.Empty;
                    }
                    else
                    {
                        cen.centername = item.CenterName;
                        cen.address = item.Address;
                        cen.city = item.CityName;
                        cen.area = item.Area ?? string.Empty;
                        cen.DescHTML = item.DescriptionHTML ?? string.Empty;
                    }

                    cen.cityid = item.FK_CityID ?? 0;
                    cen.branchdetails = string.Empty;
                    cen.latitude = item.px.ToString();
                    cen.longitude = item.py.ToString();

                    cen.image = item.POSImage ?? string.Empty;

                    if (item.OpeningTime != null && item.ClosingTime != null)
                    {
                        TimeSpan Open = new TimeSpan(item.OpeningTime.Value.Hours, 0, 0);
                        TimeSpan Close = new TimeSpan(item.ClosingTime.Value.Hours, 0, 0);
                        TimeSpan result = Open - Close;
                        cen.IsFullday = (result.Hours == 0 && result.Minutes == 0) ? true : false;
                    }
                    else
                        cen.IsFullday = false;
                    if (item.OpenningDate > DateTime.Now)
                        cen.comingSoon = true;
                    else
                        cen.comingSoon = false;
                    cen.OpeningTime = item.OpeningTime == null ? string.Empty : item.OpeningTime.Value.Hours.ToString("00.##") + ":" + item.OpeningTime.Value.Minutes.ToString("00.##");
                    cen.ClosingTime = item.ClosingTime == null ? string.Empty : item.ClosingTime.Value.Hours.ToString("00.##") + ":" + item.ClosingTime.Value.Minutes.ToString("00.##");

                    cen.ContactNo = item.ContactNo ?? string.Empty;
                    resp.data.Add(cen);

                }

                // }
            }
            else if (sort == 2)
            {
                List = List.OrderByDescending(o => o.CenterName).ToList();
                // if (List.Count > 1)
                //{
                foreach (var item in List)
                {
                    ServiceCenters cen = new ServiceCenters();
                    cen.id = item.ID;
                    if (lang == "ar")
                    {
                        cen.centername = item.CenterNameAr ?? string.Empty;
                        cen.address = item.AddressAR ?? string.Empty;
                        cen.city = item.CityNameAR ?? string.Empty;
                        cen.area = item.AreaAr ?? string.Empty;
                        cen.DescHTML = item.DescriptionHTMLAR ?? string.Empty;
                    }
                    else
                    {
                        cen.centername = item.CenterName;
                        cen.address = item.Address;
                        cen.city = item.CityName;
                        cen.area = item.Area ?? string.Empty;
                        cen.DescHTML = item.DescriptionHTML ?? string.Empty;
                    }
                    cen.branchdetails = string.Empty;
                    cen.latitude = item.px.ToString();
                    cen.longitude = item.py.ToString();

                    cen.image = item.POSImage ?? string.Empty;

                    if (item.OpeningTime != null && item.ClosingTime != null)
                    {
                        TimeSpan Open = new TimeSpan(item.OpeningTime.Value.Hours, 0, 0);
                        TimeSpan Close = new TimeSpan(item.ClosingTime.Value.Hours, 0, 0);
                        TimeSpan result = Open - Close;
                        cen.IsFullday = (result.Hours == 0 && result.Minutes == 0) ? true : false;
                    }
                    else
                        cen.IsFullday = false;
                    if (item.OpenningDate > DateTime.Now)
                        cen.comingSoon = true;
                    else
                        cen.comingSoon = false;

                    cen.OpeningTime = item.OpeningTime == null ? string.Empty : item.OpeningTime.Value.Hours.ToString("00.##") + ":" + item.OpeningTime.Value.Minutes.ToString("00.##");
                    cen.ClosingTime = item.ClosingTime == null ? string.Empty : item.ClosingTime.Value.Hours.ToString("00.##") + ":" + item.ClosingTime.Value.Minutes.ToString("00.##");
                    cen.ContactNo = item.ContactNo ?? string.Empty;
                    resp.data.Add(cen);

                }

                // }
            }

            return resp;
        }

        public ServiceCenterListResponse GetServiceCenters(int sort, double lat, double lon, string lang, string SearchKeyword = null)
        {
            ServiceCenterListResponse resp = new ServiceCenterListResponse();
            resp.error = 0;
            resp.message = "Success";
            resp.data = new List<ServiceCenters>();

            double latA = lat;
            double longA = lon;
            double latB;
            //= -31.99212f;
            double longB;
            var locA = new GeoCoordinate(latA, longA);

            var List = (from s in DataContext.ServiceCenters
                        join c in DataContext.Cities on s.FK_CityID equals c.ID
                        where s.IsActive == true && !s.CenterName.Contains("Warehouse") && s.IsInLoyaltyProgram == true
                        select new { s.ID, s.CenterName, s.CenterNameAr, s.Address, s.AddressAR, c.CityName, c.CityNameAR, s.Area, s.AreaAr, s.px, s.py, s.FK_CityID, s.DescriptionHTML, s.DescriptionHTMLAR, s.POSImage, s.OpeningTime, s.ClosingTime, s.ContactNo, s.OpenningDate }
                          ).ToList();

            if (SearchKeyword != null)
            {
                List = (from s in DataContext.ServiceCenters
                        join c in DataContext.Cities on s.FK_CityID equals c.ID
                        where s.IsActive == true && !s.CenterName.Contains("Warehouse") && s.IsInLoyaltyProgram == true && ((s.CenterName.Contains(SearchKeyword)) || (s.CenterNameAr.Contains(SearchKeyword)) ||
                        (s.Address.Contains(SearchKeyword)) || (s.AddressAR.Contains(SearchKeyword)) || (s.Area.Contains(SearchKeyword)) ||
                        (s.AreaAr.Contains(SearchKeyword)) || (c.CityName.Contains(SearchKeyword)) || (c.CityNameAR.Contains(SearchKeyword)) ||
                        (c.Region.RegionName.Contains(SearchKeyword)) || (c.Region.RegionNameAr.Contains(SearchKeyword)))
                        select new { s.ID, s.CenterName, s.CenterNameAr, s.Address, s.AddressAR, c.CityName, c.CityNameAR, s.Area, s.AreaAr, s.px, s.py, s.FK_CityID, s.DescriptionHTML, s.DescriptionHTMLAR, s.POSImage, s.OpeningTime, s.ClosingTime, s.ContactNo, s.OpenningDate }).ToList();
            }

            List<NearBy> lst = new List<NearBy>();

            if (List.Count > 0)
            {
                foreach (var item in List)
                {
                    latB = Convert.ToDouble(item.px);
                    longB = Convert.ToDouble(item.py);
                    var locB = new GeoCoordinate(latB, longB);
                    double distance = locA.GetDistanceTo(locB) / 1000;
                    lst.Add(new NearBy() { CenterId = item.ID, distance = distance });

                }
            }

            var LstafterSort = (from l in List
                                join ol in lst on l.ID equals ol.CenterId
                                select new { l.ID, l.CenterName, l.CenterNameAr, l.Address, l.AddressAR, l.CityName, l.CityNameAR, l.Area, l.AreaAr, l.px, l.py, ol.distance, l.FK_CityID, l.DescriptionHTML, l.DescriptionHTMLAR, l.POSImage, l.OpeningTime, l.ClosingTime, l.ContactNo, l.OpenningDate }).ToList().OrderBy(o => o.distance);
            foreach (var item in LstafterSort)
            {
                ServiceCenters cen = new ServiceCenters();


                cen.id = item.ID;
                if (lang == "ar")
                {
                    cen.centername = item.CenterNameAr ?? string.Empty;
                    cen.address = item.AddressAR ?? string.Empty;
                    cen.city = item.CityNameAR ?? string.Empty;
                    cen.area = item.AreaAr ?? string.Empty;
                    cen.DescHTML = item.DescriptionHTMLAR ?? string.Empty;
                }
                else
                {
                    cen.centername = item.CenterName;
                    cen.address = item.Address;
                    cen.city = item.CityName;
                    cen.area = item.Area ?? string.Empty;
                    cen.DescHTML = item.DescriptionHTML ?? string.Empty;
                }

                cen.cityid = item.FK_CityID ?? 0;

                cen.distance = item.distance;

                cen.branchdetails = string.Empty;
                cen.latitude = item.px.ToString();
                cen.longitude = item.py.ToString();

                cen.image = item.POSImage ?? string.Empty;

                if (item.OpeningTime != null && item.ClosingTime != null)
                {
                    TimeSpan Open = new TimeSpan(item.OpeningTime.Value.Hours, 0, 0);
                    TimeSpan Close = new TimeSpan(item.ClosingTime.Value.Hours, 0, 0);
                    TimeSpan result = Open - Close;
                    cen.IsFullday = (result.Hours == 0 && result.Minutes == 0) ? true : false;
                }
                else
                    cen.IsFullday = false;
                if (item.OpenningDate > DateTime.Now)
                    cen.comingSoon = true;
                else
                    cen.comingSoon = false;
                cen.OpeningTime = item.OpeningTime == null ? string.Empty : item.OpeningTime.Value.Hours.ToString("00.##") + ":" + item.OpeningTime.Value.Minutes.ToString("00.##");
                cen.ClosingTime = item.ClosingTime == null ? string.Empty : item.ClosingTime.Value.Hours.ToString("00.##") + ":" + item.ClosingTime.Value.Minutes.ToString("00.##");
                cen.ContactNo = item.ContactNo ?? string.Empty;
                resp.data.Add(cen);

            }

            return resp;
        }
        // Same as GetServiceCenters with IsBooking Flage
        public ServiceCenterListResponse GetBookingServiceCenters(int sort, string lang, string SearchKeyword = null)
        {
            ServiceCenterListResponse resp = new ServiceCenterListResponse();
            resp.error = 0;
            resp.message = "Success";
            resp.data = new List<ServiceCenters>();


            var List = (from s in DataContext.ServiceCenters
                        join c in DataContext.Cities on s.FK_CityID equals c.ID
                        where s.IsActive == true && !s.CenterName.Contains("Warehouse") && s.IsInLoyaltyProgram == true && s.IsBooking == true
                        select new { s.ID, s.CenterName, s.CenterNameAr, s.Address, s.AddressAR, c.CityName, c.CityNameAR, s.Area, s.AreaAr, s.px, s.py, s.FK_CityID, s.DescriptionHTML, s.DescriptionHTMLAR, s.POSImage, s.OpeningTime, s.ClosingTime, s.ContactNo, s.OpenningDate }
                          ).ToList();

            if (SearchKeyword != null)
            {
                List = (from s in DataContext.ServiceCenters
                        join c in DataContext.Cities on s.FK_CityID equals c.ID
                        where s.IsActive == true && !s.CenterName.Contains("Warehouse") && s.IsInLoyaltyProgram == true && s.IsBooking == true
                        && ((s.CenterName.Contains(SearchKeyword)) || (s.CenterNameAr.Contains(SearchKeyword)) ||
                        (s.Address.Contains(SearchKeyword)) || (s.AddressAR.Contains(SearchKeyword)) || (s.Area.Contains(SearchKeyword)) ||
                        (s.AreaAr.Contains(SearchKeyword)) || (c.CityName.Contains(SearchKeyword)) || (c.CityNameAR.Contains(SearchKeyword)) ||
                        (c.Region.RegionName.Contains(SearchKeyword)) || (c.Region.RegionNameAr.Contains(SearchKeyword)))
                        select new { s.ID, s.CenterName, s.CenterNameAr, s.Address, s.AddressAR, c.CityName, c.CityNameAR, s.Area, s.AreaAr, s.px, s.py, s.FK_CityID, s.DescriptionHTML, s.DescriptionHTMLAR, s.POSImage, s.OpeningTime, s.ClosingTime, s.ContactNo, s.OpenningDate }).ToList();
            }


            if (sort == 1)
            {
                List = List.OrderBy(o => o.CenterName).ToList();
                //if (List.Count > 1)
                //{
                foreach (var item in List)
                {
                    ServiceCenters cen = new ServiceCenters();
                    cen.id = item.ID;
                    if (lang == "ar")
                    {
                        cen.centername = item.CenterNameAr ?? string.Empty;
                        cen.address = item.AddressAR ?? string.Empty;
                        cen.city = item.CityNameAR ?? string.Empty;
                        cen.area = item.AreaAr ?? string.Empty;
                        cen.DescHTML = item.DescriptionHTMLAR ?? string.Empty;
                    }
                    else
                    {
                        cen.centername = item.CenterName;
                        cen.address = item.Address;
                        cen.city = item.CityName;
                        cen.area = item.Area ?? string.Empty;
                        cen.DescHTML = item.DescriptionHTML ?? string.Empty;
                    }

                    cen.cityid = item.FK_CityID ?? 0;
                    cen.branchdetails = string.Empty;
                    cen.latitude = item.px.ToString();
                    cen.longitude = item.py.ToString();

                    cen.image = item.POSImage ?? string.Empty;

                    if (item.OpeningTime != null && item.ClosingTime != null)
                    {
                        TimeSpan Open = new TimeSpan(item.OpeningTime.Value.Hours, 0, 0);
                        TimeSpan Close = new TimeSpan(item.ClosingTime.Value.Hours, 0, 0);
                        TimeSpan result = Open - Close;
                        cen.IsFullday = (result.Hours == 0 && result.Minutes == 0) ? true : false;
                    }
                    else
                        cen.IsFullday = false;
                    if (item.OpenningDate > DateTime.Now)
                        cen.comingSoon = true;
                    else
                        cen.comingSoon = false;
                    cen.OpeningTime = item.OpeningTime == null ? string.Empty : item.OpeningTime.Value.Hours.ToString("00.##") + ":" + item.OpeningTime.Value.Minutes.ToString("00.##");
                    cen.ClosingTime = item.ClosingTime == null ? string.Empty : item.ClosingTime.Value.Hours.ToString("00.##") + ":" + item.ClosingTime.Value.Minutes.ToString("00.##");

                    cen.ContactNo = item.ContactNo ?? string.Empty;
                    resp.data.Add(cen);

                }

                // }
            }
            else if (sort == 2)
            {
                List = List.OrderByDescending(o => o.CenterName).ToList();
                // if (List.Count > 1)
                //{
                foreach (var item in List)
                {
                    ServiceCenters cen = new ServiceCenters();
                    cen.id = item.ID;
                    if (lang == "ar")
                    {
                        cen.centername = item.CenterNameAr ?? string.Empty;
                        cen.address = item.AddressAR ?? string.Empty;
                        cen.city = item.CityNameAR ?? string.Empty;
                        cen.area = item.AreaAr ?? string.Empty;
                        cen.DescHTML = item.DescriptionHTMLAR ?? string.Empty;
                    }
                    else
                    {
                        cen.centername = item.CenterName;
                        cen.address = item.Address;
                        cen.city = item.CityName;
                        cen.area = item.Area ?? string.Empty;
                        cen.DescHTML = item.DescriptionHTML ?? string.Empty;
                    }
                    cen.branchdetails = string.Empty;
                    cen.latitude = item.px.ToString();
                    cen.longitude = item.py.ToString();

                    cen.image = item.POSImage ?? string.Empty;

                    if (item.OpeningTime != null && item.ClosingTime != null)
                    {
                        TimeSpan Open = new TimeSpan(item.OpeningTime.Value.Hours, 0, 0);
                        TimeSpan Close = new TimeSpan(item.ClosingTime.Value.Hours, 0, 0);
                        TimeSpan result = Open - Close;
                        cen.IsFullday = (result.Hours == 0 && result.Minutes == 0) ? true : false;
                    }
                    else
                        cen.IsFullday = false;
                    if (item.OpenningDate > DateTime.Now)
                        cen.comingSoon = true;
                    else
                        cen.comingSoon = false;
                    cen.OpeningTime = item.OpeningTime == null ? string.Empty : item.OpeningTime.Value.Hours.ToString("00.##") + ":" + item.OpeningTime.Value.Minutes.ToString("00.##");
                    cen.ClosingTime = item.ClosingTime == null ? string.Empty : item.ClosingTime.Value.Hours.ToString("00.##") + ":" + item.ClosingTime.Value.Minutes.ToString("00.##");
                    cen.ContactNo = item.ContactNo ?? string.Empty;
                    resp.data.Add(cen);

                }

                // }
            }

            return resp;
        }
        // Same as GetServiceCenters with IsBooking Flage
        public ServiceCenterListResponse GetBookingServiceCenters(int sort, double lat, double lon, string lang, string SearchKeyword = null)
        {
            ServiceCenterListResponse resp = new ServiceCenterListResponse();
            resp.error = 0;
            resp.message = "Success";
            resp.data = new List<ServiceCenters>();

            double latA = lat;
            double longA = lon;
            double latB;
            //= -31.99212f;
            double longB;
            var locA = new GeoCoordinate(latA, longA);

            var List = (from s in DataContext.ServiceCenters
                        join c in DataContext.Cities on s.FK_CityID equals c.ID
                        where s.IsActive == true && !s.CenterName.Contains("Warehouse") && s.IsInLoyaltyProgram == true && s.IsBooking == true
                        select new { s.ID, s.CenterName, s.CenterNameAr, s.Address, s.AddressAR, c.CityName, c.CityNameAR, s.Area, s.AreaAr, s.px, s.py, s.FK_CityID, s.DescriptionHTML, s.DescriptionHTMLAR, s.POSImage, s.OpeningTime, s.ClosingTime, s.ContactNo, s.OpenningDate }
                          ).ToList();

            if (SearchKeyword != null)
            {
                List = (from s in DataContext.ServiceCenters
                        join c in DataContext.Cities on s.FK_CityID equals c.ID
                        where s.IsActive == true && !s.CenterName.Contains("Warehouse") && s.IsInLoyaltyProgram == true && s.IsBooking == true &&
                        ((s.CenterName.Contains(SearchKeyword)) || (s.CenterNameAr.Contains(SearchKeyword)) ||
                        (s.Address.Contains(SearchKeyword)) || (s.AddressAR.Contains(SearchKeyword)) || (s.Area.Contains(SearchKeyword)) ||
                        (s.AreaAr.Contains(SearchKeyword)) || (c.CityName.Contains(SearchKeyword)) || (c.CityNameAR.Contains(SearchKeyword)) ||
                        (c.Region.RegionName.Contains(SearchKeyword)) || (c.Region.RegionNameAr.Contains(SearchKeyword)))
                        select new { s.ID, s.CenterName, s.CenterNameAr, s.Address, s.AddressAR, c.CityName, c.CityNameAR, s.Area, s.AreaAr, s.px, s.py, s.FK_CityID, s.DescriptionHTML, s.DescriptionHTMLAR, s.POSImage, s.OpeningTime, s.ClosingTime, s.ContactNo, s.OpenningDate }).ToList();
            }

            List<NearBy> lst = new List<NearBy>();

            if (List.Count > 0)
            {
                foreach (var item in List)
                {
                    latB = Convert.ToDouble(item.px);
                    longB = Convert.ToDouble(item.py);
                    var locB = new GeoCoordinate(latB, longB);
                    double distance = locA.GetDistanceTo(locB) / 1000;
                    lst.Add(new NearBy() { CenterId = item.ID, distance = distance });

                }
            }

            var LstafterSort = (from l in List
                                join ol in lst on l.ID equals ol.CenterId
                                select new { l.ID, l.CenterName, l.CenterNameAr, l.Address, l.AddressAR, l.CityName, l.CityNameAR, l.Area, l.AreaAr, l.px, l.py, ol.distance, l.FK_CityID, l.DescriptionHTML, l.DescriptionHTMLAR, l.POSImage, l.OpeningTime, l.ClosingTime, l.ContactNo ,l.OpenningDate}).ToList().OrderBy(o => o.distance);
            foreach (var item in LstafterSort)
            {
                ServiceCenters cen = new ServiceCenters();


                cen.id = item.ID;
                if (lang == "ar")
                {
                    cen.centername = item.CenterNameAr ?? string.Empty;
                    cen.address = item.AddressAR ?? string.Empty;
                    cen.city = item.CityNameAR ?? string.Empty;
                    cen.area = item.AreaAr ?? string.Empty;
                    cen.DescHTML = item.DescriptionHTMLAR ?? string.Empty;
                }
                else
                {
                    cen.centername = item.CenterName;
                    cen.address = item.Address;
                    cen.city = item.CityName;
                    cen.area = item.Area ?? string.Empty;
                    cen.DescHTML = item.DescriptionHTML ?? string.Empty;
                }

                cen.cityid = item.FK_CityID ?? 0;

                cen.distance = item.distance;

                cen.branchdetails = string.Empty;
                cen.latitude = item.px.ToString();
                cen.longitude = item.py.ToString();

                cen.image = item.POSImage ?? string.Empty;

                if (item.OpeningTime != null && item.ClosingTime != null)
                {
                    TimeSpan Open = new TimeSpan(item.OpeningTime.Value.Hours, 0, 0);
                    TimeSpan Close = new TimeSpan(item.ClosingTime.Value.Hours, 0, 0);
                    TimeSpan result = Open - Close;
                    cen.IsFullday = (result.Hours == 0 && result.Minutes == 0) ? true : false;
                }
                else
                    cen.IsFullday = false;
                if (item.OpenningDate > DateTime.Now)
                    cen.comingSoon = true;
                else
                    cen.comingSoon = false;
                cen.OpeningTime = item.OpeningTime == null ? string.Empty : item.OpeningTime.Value.Hours.ToString("00.##") + ":" + item.OpeningTime.Value.Minutes.ToString("00.##");
                cen.ClosingTime = item.ClosingTime == null ? string.Empty : item.ClosingTime.Value.Hours.ToString("00.##") + ":" + item.ClosingTime.Value.Minutes.ToString("00.##");
                cen.ContactNo = item.ContactNo ?? string.Empty;
                resp.data.Add(cen);

            }

            return resp;
        }
    }
}
