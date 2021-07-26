namespace MLP.BAL.ViewModels
{
    public class RequestItemsViewModel
    {
        public int ID { get; set; }
        public int? SalesItemID { get; set; }
        public int? Quantity { get; set; }
        public string ItemName { get; set; }
        public string FK_RequestNo { get; set; }
    }
}