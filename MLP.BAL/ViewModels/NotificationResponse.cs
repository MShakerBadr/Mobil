using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.ViewModels
{
    public class NotificationResponse
    {
        public int error { get; set; }

        public string message { get; set; }

        public List<NotificationsData> data { get; set; }

    }
    public class NotificationsData
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public string Date { get; set; }

        public string DateTimeNotification { get; set; }

        public string NotificationType { get; set; }

        public bool IsSeen { get; set; }

    }
}
