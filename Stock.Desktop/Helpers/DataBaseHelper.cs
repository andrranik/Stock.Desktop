using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Stock.Desktop.Models;

namespace Stock.Desktop.Helpers
{
    internal class DataBaseHelper: IDisposable
    {
        private SqlConnection _connection;


        public DataBaseHelper()
        {
            _connection = new SqlConnection("Server = (LocalDb)\\MSSQLLocalDB; Initial Catalog = Stock; Integrated Security = SSPI; Trusted_Connection = yes;");
            _connection.Open();
        }

        #region Query Builder

        public SqlCommand GetSelectQuery(SelectQuery query)
        {

            return new SqlCommand();
        }


        #endregion



        #region Stock

            #region Get Values

        public async Task<Models.Stock> GetStockById(int id)
        {
            var cnt = new Condition("id", Convert.ToString(id), BinaryOperators.Equal);
            var dict = new Dictionary<LogicOperators, Condition> { { LogicOperators.FirstCondition, cnt } };
            var query = new SelectQuery(Tables.StockTable, null, dict);
            var command = new SqlCommand(query.ToString(), _connection);
            var result = await command.ExecuteReaderAsync();
            return new Models.Stock(result);
        }

        public async Task<List<Models.Stock>> GetStocks(Dictionary<LogicOperators, Condition> conditions = null)
        {
            var query = new SelectQuery(Tables.StockTable, Tables.StockFields);
            var result = new List<Models.Stock>();
            var command = new SqlCommand(query.ToString(), _connection);
            var dr = await command.ExecuteReaderAsync();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    result.Add(new Models.Stock(dr));
                }
            }

            return result;
        }

            #endregion

            #region Create Values

        public async Task<int> CreateStock(Models.Stock stock)
        {
            var query = new InsertQuery(Tables.StockTable, Tables.StockFields.Where(x => !x.Equals("ID")).ToList(), stock.GetFieldValues());
            var command = new SqlCommand(query.ToString(), _connection);
            return await command.ExecuteNonQueryAsync();
        }

        #endregion

            #region Update

        public async Task<int> UpdateStock(Models.Stock stock)
        {
            var getById = new Condition("id", Convert.ToString(stock.Id), BinaryOperators.Equal);
            Dictionary<LogicOperators, Condition> condition = new Dictionary<LogicOperators, Condition>{{ LogicOperators.FirstCondition, getById }};
            var query = new UpdateQuery(Tables.StockTable, Tables.StockFields.Where(x => x != "ID").ToList(), stock.GetFieldValues(false), condition);
            var command = new SqlCommand(query.ToString(), _connection);
            return await command.ExecuteNonQueryAsync();
        }

            #endregion

            #region Delete

        public async Task<int> DeleteStock(Models.Stock stock)
        {
            var cnd = new Condition("ID", Convert.ToString(stock.Id), BinaryOperators.Equal);
            Dictionary<LogicOperators, Condition> conditions = new Dictionary<LogicOperators, Condition>{{LogicOperators.FirstCondition, cnd}};
            var query = new DeleteQuery(Tables.StockTable, conditions);
            var command = new SqlCommand(query.ToString(), _connection);
            return await command.ExecuteNonQueryAsync();
        }

        #endregion



        #endregion

        #region Сommon


            #region Get Values

        public async Task<T> GetById<T>(int id, string tableName, List<string> tableFields)
        {
            var cnt = new Condition("id", Convert.ToString(id), BinaryOperators.Equal);
            var dict = new Dictionary<LogicOperators, Condition> { { LogicOperators.FirstCondition, cnt } };
            var query = new SelectQuery(tableName, tableFields, dict);
            var command = new SqlCommand(query.ToString(), _connection);
            var result = await command.ExecuteReaderAsync();
            result.Read();
            var entity = (T)Activator.CreateInstance(typeof(T), result);
            result.Close();
            return entity;
        }

        public async Task<List<T>> GetElements<T>(string tableName, List<string> fields, Dictionary<LogicOperators, Condition> conditions = null)
        {
            var query = new SelectQuery(tableName, fields);
            var result = new List<T>();
            var command = new SqlCommand(query.ToString(), _connection);
            var dr = await command.ExecuteReaderAsync();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    result.Add((T)Activator.CreateInstance(typeof(T), dr));
                }
                dr.Close();
            }

            return result;
        }

            #endregion

        #region Create Values

        public async Task<int> Create<T>(T element, string tableName, List<string> fields)
        {
            var query = new InsertQuery(tableName, fields.Where(x => !x.Equals("ID")).ToList(), ((IBaseModel)element).GetFieldValues());
            var command = new SqlCommand(query.ToString(), _connection);
            return await command.ExecuteNonQueryAsync();
        }

        #endregion

        #region Update

        public async Task<int> Update<T>(T element, string tableName, List<string> fields)
        {
            var model = (IBaseModel)element;
            var getById = new Condition("id", Convert.ToString(model.Id), BinaryOperators.Equal);
            Dictionary<LogicOperators, Condition> condition = new Dictionary<LogicOperators, Condition> { { LogicOperators.FirstCondition, getById } };
            var query = new UpdateQuery(tableName, fields.Where(x => x != "ID").ToList(), model.GetFieldValues(false), condition);
            var command = new SqlCommand(query.ToString(), _connection);
            return await command.ExecuteNonQueryAsync();
        }

        #endregion

        #region Delete

        public async Task<int> Delete<T>(T element, string tableName)
        {
            var model = (IBaseModel)element;
            var cnd = new Condition("ID", Convert.ToString(model.Id), BinaryOperators.Equal);
            Dictionary<LogicOperators, Condition> conditions = new Dictionary<LogicOperators, Condition> { { LogicOperators.FirstCondition, cnd } };
            var query = new DeleteQuery(tableName, conditions);
            var command = new SqlCommand(query.ToString(), _connection);
            return await command.ExecuteNonQueryAsync();
        }

        #endregion



        #endregion

        public void Dispose()
        {
            _connection.Dispose();
        }

    }

    public class QueryParams
    {
        public string Table { get; set; }

        public List<string> Fields { get; set; }
    }

    public class ConditionQuery : QueryParams
    {
        public Dictionary<LogicOperators, Condition> Conditions { get; set; }
    }

    public class SelectQuery : ConditionQuery
    {
        public SelectQuery(string tableName, List<string> fields)
        {
            Table = tableName;
            Fields = fields;
        }

        public SelectQuery(string tableName, List<string> fields, Dictionary<LogicOperators, Condition> conditions): this(tableName, fields)
        {
            Conditions = conditions;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Table))
                throw new ArgumentException("Не указана таблица для выборки!");
            var result = string.Format(QueryStrings.SelectBody, Fields == null ? "*" : string.Join(", ", Fields), Table);
            if (Conditions != null && Conditions.Count > 0)
                result += Conditions.GetConditionString();
            return result;
        }
    }

    public class InsertQuery: QueryParams
    {
        public InsertQuery(string tableName, List<string> fields, List<string> values)
        {
            if (fields.Count == 0 || values.Count == 0 || fields.Count != values.Count)
                throw new ArgumentException("Количество перечисленных полей и значений не совпадает!");
            Table = tableName;
            Fields = fields;
            Values = values;
        }

        public List<string> Values { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Table))
                throw new ArgumentException("Не указана таблица для вставки!");
            return string.Format(QueryStrings.InsertBody, Table,
                string.Join(", ", Fields), string.Join(", ", Values));
        }
    }

    public class UpdateQuery: ConditionQuery
    {
        public UpdateQuery(string tableName, List<string> fields, List<string> values, Dictionary<LogicOperators, Condition> conditions = null)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Не указана таблица для обновления!");
            if (fields.Count == 0 || values.Count == 0 || (fields.Count != values.Count))
                throw new ArgumentException("Неверно указаны поля или значения!");

            Table = tableName;
            Fields = fields;
            Values = values;
            Conditions = conditions;
        }

        public List<string> Values { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Table))
                throw new ArgumentException("Не указана таблица для обновления!");
            
            StringBuilder setValues = new StringBuilder();
            for (int i = 0; i < Fields.Count; i++)
            {
                setValues.Append($"{(i == 0 ? String.Empty : ",")} {Fields[i]} = {Values[i]} ");
            }

            var result = string.Format(QueryStrings.UpdateBody, Table, setValues);
            if (Conditions != null && Conditions.Count > 0)
                result += Conditions.GetConditionString();
            return result;
        }
    }

    public class DeleteQuery
    {
        public DeleteQuery(string tableName, Dictionary<LogicOperators, Condition> conditions = null)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Не указана таблица для обновления!");
            Table = tableName;
            Conditions = conditions;
        }

        public string Table { get; set; }

        public Dictionary<LogicOperators, Condition> Conditions { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Table))
                throw new ArgumentException("Не указана таблица для обновления!");

            var result = string.Format(QueryStrings.DeleteBody, Table);

            if (Conditions != null && Conditions.Count > 0)
                result += Conditions.GetConditionString();

            return result;
        }
    }

    public class Condition
    {

        public Condition(string left, string right, BinaryOperators opr)
        {
            Fields = new string[2] {left, right};
            Operator = opr;
        } 

        public string[] Fields { get; set; } = new string[2];
        public BinaryOperators Operator;

        public override string ToString()
        {
            if (Fields.Any(string.IsNullOrWhiteSpace))
                throw new ApplicationException("Не указаны поля для сравнения!");

            var result = string.Empty;
            result += Fields[0];
            switch (Operator)
            {
                case BinaryOperators.Equal:
                    result += " = ";
                    break;
                case BinaryOperators.Greater:
                    result += " > ";
                    break;
                case BinaryOperators.Less:
                    result += " < ";
                    break;
                case BinaryOperators.NotEqual:
                    result += " <> ";
                    break;
            }

            result += Fields[1];
            return result;

        }
    }

    public enum LogicOperators
    {
        FirstCondition = 0,
        Or = 1,
        And = 2
    }

    public enum BinaryOperators
    {
        Equal = 0,
        Greater = 1,
        Less = 2,
        NotEqual = 3
    }

    public static class ExtMethods
    {
        public static string GetConditionString(this Dictionary<LogicOperators, Condition> conditions)
        {

            if (conditions == null || conditions.Count == 0)
                return string.Empty;
            var result = QueryStrings.ConditionKeyWord;
            foreach (var condition in conditions)
            {
                if (result == QueryStrings.ConditionKeyWord)
                {
                    result += $"{condition.Value} ";
                }
                else
                {
                    result += $"{GetLogicOperatorString(condition.Key)} {condition.Value}";
                }
            }

            return result;
        }

        public static void AddList<T>(this ObservableCollection<T> collection, List<T> list)
        {
            foreach (var item in list)
            {
                collection.Add(item);
            }
        }

        public static string GetLogicOperatorString(LogicOperators opr)
        {
            switch (opr)
            {
                case LogicOperators.FirstCondition:
                    return String.Empty;
                case LogicOperators.And:
                    return " AND ";
                case LogicOperators.Or:
                    return " OR ";
                default: return "";
            }
        }
    }

    public static class QueryStrings
    {
        public const string SelectBody = "SELECT {0} FROM {1} ";
        public const string InsertBody = "INSERT INTO {0}({1}) VALUES({2})";
        public const string UpdateBody = "UPDATE {0} SET {1} ";
        public const string DeleteBody = "DELETE FROM {0} ";
        public const string ConditionKeyWord = "WHERE ";
        public const string JoinBody = "JOIN {0} t1 on {1} ";

    }

    public static class Tables
    {
        public static string StockTable = "di_stocks";
        public static List<string> StockFields = new List<string>{"ID", "NAME", "VOLUME"};

        public static string MaterialTable = "di_material";
        public static List<string> MaterialFields = new List<string> {"ID", "NAME"};

        public static string MetricUnitTable = "di_metric_units";
        public static List<string> MetricUnitFields = new List<string> { "ID", "NAME" };

        public static string DocTypeTable = "di_doc_types";
        public static List<string> DocTypeFields = new List<string> { "ID", "NAME" };

        public static string DocTable = "di_docs";

        public static List<string> DocFields = new List<string>
            {"ID", "NAME", "DATE", "QUANTITY", "DOC_TYPE_ID", "STOCK_ID", "METRIC_UNIT_ID", "MATERIAL_ID"};
    }
}
