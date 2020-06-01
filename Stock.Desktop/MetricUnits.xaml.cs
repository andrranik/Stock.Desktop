using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Stock.Desktop.EditForms;
using Stock.Desktop.Helpers;
using Stock.Desktop.Models;

namespace Stock.Desktop
{
    /// <summary>
    /// Interaction logic for Stocks.xaml
    /// </summary>
    public partial class MetricUnits : UserControl
    {
        public ObservableCollection<MetricUnit> MetricUnitList { get; set; } = new ObservableCollection<MetricUnit>();

        public MetricUnit SelectedMetricUnit { get; set; }

        public MetricUnits()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async void MetricUnitAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new MetricUnitEdit();
            editWindow.ShowDialog();
            await Restore();
        }

        private async void MetricUnitEdit_Click(object sender, RoutedEventArgs e)
        {
            var se = new MetricUnitEdit();
            se.MetricUnitItem = SelectedMetricUnit;
            se.ShowDialog();
            await Restore();
        }

        private async void MetricUnitDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dbh = new DataBaseHelper())
                {
                    var added = await dbh.Delete(SelectedMetricUnit, Tables.MetricUnitTable);
                    await Restore();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.IsVisible)
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
                var res = await dbh.GetElements<MetricUnit>(Tables.MetricUnitTable, Tables.MetricUnitFields);
                MetricUnitList.Clear();
                foreach (var mu in res)
                {
                    MetricUnitList.Add(mu);
                }
            }
        }
    }
}
