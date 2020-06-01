using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.Desktop.Helpers;

namespace Stock.Desktop.Models
{
    public class Doc: BaseModel, IBaseModel
    {
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
        public int DocTypeId { get; set; }
        public string TypeName { get; set; }
        public int StockId { get; set; }
        public string StockName { get; set; }
        public int MetricUnitId { get; set; }
        public string MetricUnitName { get; set; }
        public int MaterialId { get;set; }
        public string MaterialName { get; set; }

        public Doc()
        {

        }

        public Doc(SqlDataReader reader)
        {
            FillFromReader(reader);
        }

        public void FillFromReader(SqlDataReader reader)
        {
            if (!reader.HasRows)
                throw new ArgumentException("Результат запроса не содержит строк");

            Id = Convert.ToInt32(reader["id"]);
            Name = Convert.ToString(reader["name"]);
            Date = Convert.ToDateTime(reader["date"]);
            Quantity = Convert.ToDouble(reader["quantity"]);
            DocTypeId = Convert.ToInt32(reader["doc_type_id"]);
            StockId = Convert.ToInt32(reader["stock_id"]);
            MetricUnitId = Convert.ToInt32(reader["metric_unit_id"]);
            MaterialId = Convert.ToInt32(reader["material_id"]);


            //FillChildModels();
        }

        public async Task<Doc> FillChildModels()
        {
            using (var dbh = new DataBaseHelper())
            {
                if (DocTypeId != 0)
                    TypeName = (await dbh.GetById<DocType>(DocTypeId, Tables.DocTypeTable, Tables.DocTypeFields)).Name;
                if (StockId != 0)
                    StockName = (await dbh.GetById<Models.Stock>(StockId, Tables.StockTable, Tables.StockFields)).Name;
                if (MetricUnitId != 0)
                    MetricUnitName = (await dbh.GetById<MetricUnit>(MetricUnitId, Tables.MetricUnitTable, Tables.MetricUnitFields)).Name;
                if (MaterialId != 0)
                    MaterialName = (await dbh.GetById<Material>(MaterialId, Tables.MaterialTable, Tables.MaterialFields)).Name;
                return this;
            }
            
        }

        public List<string> GetFieldValues(bool withIdentity = true)
        {
            var res = new List<string>();
            if (Id != 0 && withIdentity)
                res.Add(Convert.ToString(Id));
            res.Add($"N'{Name}'");
            res.Add($"'{Date.ToString(CultureInfo.InvariantCulture)}'");
            res.Add($"{Quantity}");
            res.Add($"{DocTypeId}");
            res.Add($"{StockId}");
            res.Add($"{MetricUnitId}");
            res.Add($"{MaterialId}");
            return res;
        }
    }
}
