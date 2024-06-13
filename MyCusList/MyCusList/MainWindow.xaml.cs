using MyCusList.Models;
using System.Windows;
using System.Windows.Controls;

namespace MyCusList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //定義Property
        public List<String> countries { set; get; }
        //Handler 查詢結果-多筆客戶資料
        public List<Customers> customers { set; get; }
        //宣告dbContext物件
        public Northwnd _dbContext;
        public MainWindow()
        {
            InitializeComponent();
            //建構DbContext物件(Persistence 連接上資料庫)
            this._dbContext = new Northwnd();
            //查詢客戶基本資料 進一步整理出國家別清單(Distinct)
            //使用LINQ查詢語法
            countries = (from c in this._dbContext.Customers
                         where c.Country != null & c.Country != ""
                         select c.Country).Distinct().OrderBy(p => p).ToList();

            this.DataContext = this;
            //委派事件程序(進行聆聽) 而非運用事件
            this.lstCountry.SelectionChanged += lstCountry_SelectionChanged;
        }

        private void lstCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //多選項 使用
            //IList items = e.AddedItems;
            //單一選項 e.Source回應根類別Object,進行Explicit轉型
            String value = ((ListBox)e.Source).SelectedValue.ToString();
            //使用LINQ查詢語法 查詢相對國家別客戶資料
            customers = (from c in this._dbContext.Customers
                         where c.Country == value
                         select c).ToList(); //Lazy Loading

            //將查詢結果資料繫結至DataGrid
            this.gridCustomers.ItemsSource = customers;
        }
    }
}