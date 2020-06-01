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
using System.Windows.Shapes;
using Stock.Desktop.Helpers;
using Stock.Desktop.Models;

namespace Stock.Desktop.EditForms
{
    /// <summary>
    /// Interaction logic for MaterialEdit.xaml
    /// </summary>
    public partial class DocEdit : Window
    {

        public Doc DocItem { get; set; }

        public ObservableCollection<Models.Stock> StockList { get; set; } = new ObservableCollection<Models.Stock>();
        public Models.Stock SelectedStockItem { get; set; }

        public ObservableCollection<MetricUnit> MetricUnitList { get; set; } = new ObservableCollection<MetricUnit>();
        public MetricUnit SelectedMetricUnitItem { get; set; }

        public ObservableCollection<Material> MaterialList { get; set; } = new ObservableCollection<Material>();
        public Material SelectedMaterialItem { get; set; }

        public ObservableCollection<DocType> DocTypeList { get; set; } = new ObservableCollection<DocType>();
        public DocType SelectedDocType { get; set; }

        public DocEdit()
        {
            InitializeComponent();
            DocItem = new Doc();
            DataContext = this;
            Initialize();
        }

        public async void Initialize()
        {
            DocItem = new Doc() { Date = DateTime.Today };
            using (var dbh = new DataBaseHelper())
            {
                StockList.AddList(await dbh.GetElements<Models.Stock>(Tables.StockTable, Tables.StockFields));
                MetricUnitList.AddList(await dbh.GetElements<MetricUnit>(Tables.MetricUnitTable, Tables.MetricUnitFields));
                MaterialList.AddList(await dbh.GetElements<Material>(Tables.MaterialTable, Tables.MaterialFields));
                DocTypeList.AddList(await dbh.GetElements<DocType>(Tables.DocTypeTable, Tables.DocTypeFields));
            }
        }

        private async void DocSave_Click(object sender, RoutedEventArgs e)
        {

            DocItem.StockId = SelectedStockItem.Id;
            DocItem.MaterialId = SelectedMaterialItem.Id;
            DocItem.DocTypeId = SelectedDocType.Id;
            DocItem.MetricUnitId = SelectedMetricUnitItem.Id;

            using (var dbh = new DataBaseHelper())
            {
                var rowAdded = DocItem.Id == 0
                    ? await dbh.Create(DocItem, Tables.DocTable, Tables.DocFields)
                    : await dbh.Update(DocItem, Tables.DocTable, Tables.DocFields);
                Close();
            }
        }

        private void DocCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
