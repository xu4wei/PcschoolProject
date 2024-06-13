namespace MyApi.Models
{
    /// <summary>
    /// 訊息物件--配合服務產生JSON訊息回到前端去
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 訊息代碼-整數
        /// </summary>
        public Int32 code { set; get; }
        /// <summary>
        /// 訊息字串
        /// </summary>
        public String message { set; get; }
    }
}
