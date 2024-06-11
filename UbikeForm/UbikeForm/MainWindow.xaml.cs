using System.Windows;

namespace UbikeForm
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        //Data Field
        private List<Models.UbikeData> ubikeData;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //呼叫自訂模組 取回Ubike資料
            ubikeData = Models.UbikeModel.getUbikeData();
            MessageBox.Show("資料筆數:" + ubikeData.Count.ToString());
            //將取回的資料繫結到DataGrid
            this.gridData.ItemsSource = ubikeData;
        }

        //查詢相對區域的Ubike即時資訊
        private void btnQry_Click(object sender, RoutedEventArgs e)
        {
            var result = (from u in ubikeData
                          where u.sarea == this.comArea.Text
                          select u).ToList();
            //將查詢結果繫結到DataGrid
            this.gridData.ItemsSource = result;
            MessageBox.Show($"區域: {this.comArea.Text} 資料筆數:" + result.Count().ToString());
        }
    }
}