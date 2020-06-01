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
using Stock.Desktop.Models;

namespace Stock.Desktop.EditForms
{
    /// <summary>
    /// Interaction logic for MetricUnitEdit.xaml
    /// </summary>
    public partial class MetricUnitEdit : Window
    {

        public MetricUnit MetricUnitItem { get; set; }

        public MetricUnitEdit()
        {
            InitializeComponent();
            MetricUnitItem = new MetricUnit();
            DataContext = this;
        }

        private async void MaterialSave_Click(object sender, RoutedEventArgs e)
        {
            using (var dbh = new DataBaseHelper())
            {
                var rowAdded = MetricUnitItem.Id == 0
                    ? await dbh.Create(MetricUnitItem, Tables.MetricUnitTable, Tables.MetricUnitFields)
                    : await dbh.Update(MetricUnitItem, Tables.MetricUnitTable, Tables.MetricUnitFields);
                Close();
            }
            
        }

        private void MaterialCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
