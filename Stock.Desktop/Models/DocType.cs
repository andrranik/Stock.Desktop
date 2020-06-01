using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Desktop.Models
{
    public class DocType: BaseModel, IBaseModel
    {
        public DocType()
        {
            
        }

        public DocType(SqlDataReader reader)
        {
            FillFromReader(reader);
        }

        public void FillFromReader(SqlDataReader reader)
        {
            if (!reader.HasRows)
                throw new ArgumentException("Результат запроса не содержит строк");

            Id = Convert.ToInt32(reader["id"]);
            Name = Convert.ToString(reader["name"]);
        }

        public List<string> GetFieldValues(bool withIdentity = true)
        {
            var res = new List<string>();
            if (Id != 0 && withIdentity)
                res.Add(Convert.ToString(Id));
            res.Add($"N'{Name}'");
            return res;
        }
    }
}
