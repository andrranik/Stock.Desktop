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
    public partial class Materials : UserControl
    {
        public ObservableCollection<Material> MaterialList { get; set; } = new ObservableCollection<Material>();

        public Material SelectedMaterial { get; set; }

        public Materials()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async void MaterialAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new MaterialEdit();
            editWindow.ShowDialog();
            await Restore();
        }

        private async void MaterialEdit_Click(object sender, RoutedEventArgs e)
        {
            var se = new MaterialEdit();
            se.MaterialItem = SelectedMaterial;
            se.ShowDialog();
            await Restore();
        }

        private async void MaterialDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dbh = new DataBaseHelper())
                {
                    var added = await dbh.Delete(SelectedMaterial, Tables.MaterialTable);
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
                var res = await dbh.GetElements<Material>(Tables.MaterialTable, Tables.MaterialFields);
                MaterialList.Clear();
                foreach (var stock in res)
                {
                    MaterialList.Add(stock);
                }
            }
        }
    }
}
