using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Desktop.Models
{
    interface IBaseModel
    {
        List<string> GetFieldValues(bool withIdentity = true);

        int Id { get; set; }
    }
}
