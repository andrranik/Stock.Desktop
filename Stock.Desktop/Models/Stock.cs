using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Desktop.Models
{
    public class Stock: BaseModel, IBaseModel
    {
        public Stock()
        {
            
        }

        public Stock(SqlDataReader reader)
        {
            if (!reader.HasRows)
                throw new ArgumentException("Результат запроса не содержит строк");
            
            Id = Convert.ToInt32(reader["id"]);
            Name = Convert.ToString(reader["name"]);
            Volume = Convert.ToDouble(reader["volume"]);
        }

        public double Volume { get; set; }

        public List<string> GetFieldValues(bool withIdentity = true)
        {
            var res = new List<string>();
            if (Id != 0 && withIdentity)
                res.Add(Convert.ToString(Id));
            res.Add($"N'{Name}'");
            res.Add(Convert.ToString(Volume, CultureInfo.InvariantCulture));
            return res;
        }
    }
}
