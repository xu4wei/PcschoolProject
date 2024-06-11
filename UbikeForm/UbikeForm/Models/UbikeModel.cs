using System.Net.Http;

namespace UbikeForm.Models
{
    public class UbikeModel
    {
        //靜態成員 串接Ubike Service
        public static List<UbikeData> getUbikeData()
        {
            //todo 採用Http Client進行遠端服務串接
            HttpClient client = new HttpClient();
            //設定服務位址
            client.BaseAddress = new Uri(Properties.Settings.Default.ubikeservice);

            //呼叫服務 採用非同步呼叫 採用Http Request Method:GET
            String jsonString = client.GetStringAsync("").GetAwaiter().GetResult();
            //將取得的json字串轉換成物件
            List<UbikeData> ubikeData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UbikeData>>(jsonString);

            return ubikeData;
        }
    }
}
