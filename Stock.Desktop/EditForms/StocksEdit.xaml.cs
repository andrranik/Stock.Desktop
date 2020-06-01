using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Stock.Desktop.Helpers;

namespace Stock.Desktop.EditForms
{
    /// <summary>
    /// Interaction logic for StocksEdit.xaml
    /// </summary>
    public partial class StocksEdit : Window
    {

        public Models.Stock StockItem { get; set; }

        public StocksEdit()
        {
            InitializeComponent();
            StockVolumeTb.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            StockItem = new Models.Stock();
            DataContext = this;
        }

        private async void StocksSave_Click(object sender, RoutedEventArgs e)
        {
            using (var dbh = new DataBaseHelper())
            {
                var rowAdded = StockItem.Id == 0 ? await dbh.CreateStock(StockItem) : await dbh.UpdateStock(StockItem);
                this.Close();
            }
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

    }
}
