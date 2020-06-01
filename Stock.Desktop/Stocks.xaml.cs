using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Stock.Desktop.EditForms;
using Stock.Desktop.Helpers;

namespace Stock.Desktop
{
    /// <summary>
    /// Interaction logic for Stocks.xaml
    /// </summary>
    public partial class Stocks : UserControl
    {
        public ObservableCollection<Models.Stock> StockList { get; set; } = new ObservableCollection<Models.Stock>();

        public Models.Stock SelectedStock { get; set; }

        public Stocks()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private async void StockAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new StocksEdit();
            editWindow.ShowDialog();
            await Restore();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Restore();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
        }

        private async Task Restore()
        {
            using (DataBaseHelper dbh = new DataBaseHelper())
            {
                if (this.IsVisible)
                {
                    var res = await dbh.GetStocks();
                    StockList.Clear();
                    foreach (var stock in res)
                    {
                        StockList.Add(stock);
                    }
                }
            }
        }

        private async void StockEdit_Click(object sender, RoutedEventArgs e)
        {
            var se = new StocksEdit();
            se.StockItem = SelectedStock;
            se.ShowDialog();
            await Restore();
        }

        private async void StockDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dbh = new DataBaseHelper())
                {
                    var added = await dbh.DeleteStock(SelectedStock);
                    await Restore();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            
        }
    }
}
