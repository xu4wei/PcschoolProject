
using System.Data;

namespace MyWeb.DbModels
{
    public class CustomersDao : IDao<Customers, String>
    {
        //Data Field Connection物件
        private IDbConnection _connection;
        //Property  Injection 屬性注入依賴的連接物件
        public IDbConnection Connection
        {
            set
            {
                _connection = value;
            }
            get
            {
                return _connection;
            }
        }

        public bool delete(string key)
        {
            throw new NotImplementedException();
        }

        public bool insert(Customers entity)
        {
            throw new NotImplementedException();
        }

        public Customers query(string key)
        {
            throw new NotImplementedException();
        }

        public List<Customers> queryAll()
        {
            //建構一個集合物件
            List<Customers> list = new List<Customers>();
            //判斷是否注入連接物件
            if(_connection == null)
            {
                throw new Exception("連接物件尚未注入!!!");
            }
            //透過連接物件產生命令物件Command
            IDbCommand comm = _connection.CreateCommand();
            //開啟連接
            _connection.Open();
            //設定查詢命令敘述Native SQL
            comm.CommandText = "SELECT CustomerID,CompanyName,Address,Phone,Country FROM Customers";
            //命令類型
            comm.CommandType = CommandType.Text; //預設值--採用SQL Pass Through-SPT傳遞Native SQL
            //執行查詢命令 將SQL送至資料庫伺服器執行在資料庫伺服器產生ResultSet逐筆Fetching下來處理
            IDataReader reader = comm.ExecuteReader();
            //保持連線 逐筆讀取 重新整理集合物件進行封裝
            while(reader.Read()) 
            {
                //相對記錄讀取欄位 注入到Customers物件的屬性去
                Customers customers = new Customers()
                {
                    CustomerID = reader["CustomerID"].ToString(),
                    CompanyName = reader["CompanyName"].ToString(),
                    Address = reader["Address"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Country = reader["Country"].ToString()
                };
                //將Customers物件加入到集合物件
                list.Add(customers);

            }
            //關閉資料讀取器
            reader.Close();
            //關閉連接
            _connection.Close();
            //回傳集合物件
            return list;
        }

        public List<Customers> queryByKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool update(Customers entity)
        {
            throw new NotImplementedException();
        }
    }
}
