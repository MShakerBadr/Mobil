using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace MLP.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Login",
                routeTemplate: "account/Login/{mobile}/{password}",
                defaults: new { controller = "Account", action = "Login" }
            );

            config.Routes.MapHttpRoute(
                name: "CheckRegisteration",
                routeTemplate: "account/Registration/Check/{mobile}/{password}",
                defaults: new { controller = "Account", action = "CheckmobileNO" }
            );

            config.Routes.MapHttpRoute(
                name: "Registeration",
                routeTemplate: "account/Registration/",
                defaults: new { controller = "Account", action = "Register" }
            );

            config.Routes.MapHttpRoute(
             name: "Verifiction",
             routeTemplate: "account/Registration/confirm/",
             defaults: new { controller = "Account", action = "ConfirmUserCode" }
            );

            config.Routes.MapHttpRoute(
            name: "ForgetPassword",
            routeTemplate: "account/ForgetPassword/{mobile}",
            defaults: new { controller = "Account", action = "ForgetPassword" }
           );

            config.Routes.MapHttpRoute(
            name: "ConfirmForgetPassword",
            routeTemplate: "account/ConfirmForgetPassword/{token}/{NewPassword}",
            defaults: new { controller = "Account", action = "ConfirmForgetPassword" }
           );

            config.Routes.MapHttpRoute(
          name: "CurrentPoints",
          routeTemplate: "account/CurrentPoints/{token}",
          defaults: new { controller = "Account", action = "CurrentPoints" }
         );

            config.Routes.MapHttpRoute(
              name: "ProdList",
              routeTemplate: "Products/Allitems/{lang}",
              defaults: new { controller = "Products", action = "Getallproducts" }
             );

            config.Routes.MapHttpRoute(
              name: "CatList",
              routeTemplate: "Category/All/",
              defaults: new { controller = "Products", action = "GetAllCategories" }
             );

            config.Routes.MapHttpRoute(
              name: "CentersList",
              routeTemplate: "ServiceCenters/All/{Sortby}/{lang}",
              defaults: new { controller = "ServiceCenters", action = "GetBySorttype" }
            );


            config.Routes.MapHttpRoute(
              name: "PromotionList",
              routeTemplate: "Promotion/All/{lang}",
              defaults: new { controller = "Promotions", action = "Getallpromtions" }
            );

            config.Routes.MapHttpRoute(
              name: "CitiesList",
              routeTemplate: "Cities/{lang}",
              defaults: new { controller = "AreaandCity", action = "Getall" }
            );

            config.Routes.MapHttpRoute(
              name: "NewsList",
              routeTemplate: "News/{lang}",
              defaults: new { controller = "News", action = "Get" }
            );

            config.Routes.MapHttpRoute(
              name: "UpdateUser",
              routeTemplate: "account/UpdateUser/{token}",
              defaults: new { controller = "Account", action = "EditCustomer" }
            );

            config.Routes.MapHttpRoute(
                name: "AwardsList",
                routeTemplate: "Awards/{lang}/{token}",
                defaults: new { controller = "Awards", action = "Get" }
            );
            config.Routes.MapHttpRoute(
                name: "VehiclesList",
                routeTemplate: "Vehicles/{token}/{lang}",
                defaults: new { controller = "Vehicles", action = "Get" }
            );

            config.Routes.MapHttpRoute(
               name: "InvoicesList",
               routeTemplate: "Invoices",
               defaults: new { controller = "Invoices", action = "Post" }
           );

            config.Routes.MapHttpRoute(
              name: "Search",
              routeTemplate: "Search/{token}/{lang}/{SearchType}/{Sortby}",
              defaults: new { controller = "Search", action = "Post" }
           );

            config.Routes.MapHttpRoute(
              name: "MobilContact",
              routeTemplate: "MobilContact",
              defaults: new { controller = "MobilContact", action = "Post" }
           );

            config.Routes.MapHttpRoute(
              name: "RedeemRequest",
              routeTemplate: "RedeemRequest/{token}/{AwardID}",
              defaults: new { controller = "Redemption", action = "RedeemRequest" }
            );

            config.Routes.MapHttpRoute(
              name: "RedeemCancel",
              routeTemplate: "RedeemCancel/{token}/{Code}",
              defaults: new { controller = "Redemption", action = "RedeemCancel" }
            );

            config.Routes.MapHttpRoute(
              name: "RedeemConfirm",
              routeTemplate: "RedeemConfirm/{token}/{lang}/{CustomerCode}/{POSCode}",
              defaults: new { controller = "Redemption", action = "RedeemConfirm" }
            );

            config.Routes.MapHttpRoute(
              name: "DeviceTokenSet",
              routeTemplate: "DeviceToken",
              defaults: new { controller = "DeviceToken", action = "Post" }
            );

            config.Routes.MapHttpRoute(
              name: "ServicesByServiceCenter",
              routeTemplate: "ServiceCenters/GetServices/{lang}/{ServiceCenterID}",
              defaults: new { controller = "ServiceCenters", action = "GetServices" }
            );

            config.Routes.MapHttpRoute(
             name: "AvailableTimes",
             routeTemplate: "Booking/AvailableTimes",
             defaults: new { controller = "Booking", action = "AvailableTimes" }
           );

            config.Routes.MapHttpRoute(
            name: "ConfirmBooking",
            routeTemplate: "Booking/ConfirmBooking",
            defaults: new { controller = "Booking", action = "ConfirmBooking" }
           );

            config.Routes.MapHttpRoute(
           name: "EditBooking",
           routeTemplate: "Booking/EditBooking",
           defaults: new { controller = "Booking", action = "EditBooking" }
          );

            config.Routes.MapHttpRoute(
             name: "GetBookingHistory",
             routeTemplate: "Booking/GetHistory/{token}/{lang}",
             defaults: new { controller = "Booking", action = "GetHistory" }
            );

            config.Routes.MapHttpRoute(
               name: "CancelBooking",
               routeTemplate: "Booking/CancelBooking",
               defaults: new { controller = "Booking", action = "CancelBooking" }
             );

            config.Routes.MapHttpRoute(
               name: "BookingCancelingReasons",
               routeTemplate: "Booking/CancelReasons/{lang}",
               defaults: new { controller = "Booking", action = "CancelReasons" }
             );

            config.Routes.MapHttpRoute(
             name: "PackageList",
             routeTemplate: "Package/{lang}",
             defaults: new { controller = "Package", action = "Get" }
             );

            config.Routes.MapHttpRoute(
             name: "RedemptionList",
             routeTemplate: "Redemption/{token}/{lang}",
             defaults: new { controller = "Redemption", action = "Get" }
             );

            config.Routes.MapHttpRoute(
            name: "NotificationList",
            routeTemplate: "Notification/{token}",
            defaults: new { controller = "Notification", action = "Get" }
            );

            config.Routes.MapHttpRoute(
          name: "NotificationClearData",
          routeTemplate: "ClearNotification/{token}",
          defaults: new { controller = "Notification", action = "ClearNotification" }
          );


            config.Routes.MapHttpRoute(
           name: "BookingStatus",
           routeTemplate: "Booking/GetBookingStatus",
           defaults: new { controller = "Booking", action = "GetBookingStatus" }
           );

            config.Routes.MapHttpRoute(
            name: "Advertisements",
            routeTemplate: "Ads/Get",
            defaults: new { controller = "Ads", action = "Get" }
            );

            config.Routes.MapHttpRoute(
             name: "CarLookUps",
             routeTemplate: "CustomerCars/GetCarLookups/{lang}",
             defaults: new { controller = "CustomerCars", action = "Get" }
             );
            config.Routes.MapHttpRoute(
             name: "CreateVehicle",
             routeTemplate: "CustomerCars/CreateVehicle",
             defaults: new { controller = "CustomerCars", action = "Post" }
             );

            config.Routes.MapHttpRoute(
             name: "GetBookingServiceCenter",
             routeTemplate: "ServiceCenters/AllBookingCenters/{Sortby}/{lang}",
             defaults: new { controller = "ServiceCenters", action = "GetBookingBranch" }
             );

            config.Routes.MapHttpRoute(
             name: "AlForsanRedeemRequest",
             routeTemplate: "Redemption/AlForsanRedeemRequest",
             defaults: new { controller = "Redemption", action = "AlForsanRedeemRequest" }
             );

            //   config.EnableSystemDiagnosticsTracing();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
