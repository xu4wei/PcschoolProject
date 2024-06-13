using System.Data;

namespace MyWeb.DbModels
{
    //介面規劃 CRUD的方法規範
    public interface IDao<T,K> //使用Generic泛型
    {
        //抽象屬性Property 注入依賴的連接物件(保持彈性)
        IDbConnection Connection { get; set; }
        //規範新增
        Boolean insert(T entity);
        //規範刪除
        Boolean delete(K key);
        //規範修改
        Boolean update(T entity);
        //規範查詢
        T query(K key);
        //多筆查詢
        List<T> queryAll();
        //配合Key進行多筆查詢
        List<T> queryByKey(K key);
    }
}
