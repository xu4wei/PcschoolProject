namespace MyApi.Models
{
    //Entity Class
    public class Order_Details
    {
        public Orders orders { set; get; }
        public List<OrderDetails> orderDetails { set; get; }

    }
}
