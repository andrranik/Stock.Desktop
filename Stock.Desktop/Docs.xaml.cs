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
using Stock.Desktop.Models;

namespace Stock.Desktop
{
    /// <summary>
    /// Interaction logic for Stocks.xaml
    /// </summary>
    public partial class Docs : UserControl
    {
        public ObservableCollection<Doc> DocList { get; set; } = new ObservableCollection<Doc>();

        public Doc SelectedDoc { get; set; }

        public Docs()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private async void DocAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new DocEdit();
            editWindow.ShowDialog();
            await Restore();
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
                var res = await dbh.GetElements<Doc>(Tables.DocTable, Tables.DocFields);
                DocList.Clear();
                foreach (var doc in res)
                {
                    var edoc = await doc.FillChildModels();
                    DocList.Add(edoc);
                }
            }
        }

        private async void DocEdit_Click(object sender, RoutedEventArgs e)
        {
            var se = new DocEdit {DocItem = SelectedDoc};
            se.ShowDialog();
            await Restore();
        }

        private async void DocDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dbh = new DataBaseHelper())
                {
                    var added = await dbh.Delete(SelectedDoc, Tables.DocTable);
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
