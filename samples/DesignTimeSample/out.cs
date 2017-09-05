using System;
using TextTemplating;
using TextTemplating.Infrastructure;
using TextTemplating.T4.Parsing;
using TextTemplating.T4.Preprocessing;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace GeneratedNamespace
{
    public partial class GeneratedClass : TextTransformationBase
    {
        public override string TransformText()
        {
            Write("\n");
            Write("\n");
            Write("\n");
            Write("\n");
            Write("\n");
            Write("\n");
            Write("\n");
            Write("\n\n");


            string Namespace = "Together.Model";
            string RepoName = "DBase";
            string ConnectionStr = "cgtConnection";

            var builder = new T4Builder("Data Source=zsj439453290.zicp.net,5562;Initial Catalog=Together;MultipleActiveResultSets=True;user id=sa;password=!dd19881221;");


            Write("\n\n#pragma warning disable 1591\nusing BaseCore;\nusing PetaPoco.NetCore;\nusing System;\nusing System.Collections.Generic;\nusing System.Data.SqlClient;\n\nnamespace ");
            Write((Namespace).ToString());
            Write("\n{\n\n     public partial class ");
            Write((RepoName).ToString());
            Write(" : Database\n     {\n        private static SqlConnection con;\n        /// <summary>\n        /// open the connection\n        /// </summary>\n        /// <returns></returns>\n        private static SqlConnection OpenConnection()\n        {\n            if (con == null)\n            {\n                con = new SqlConnection(GetConn());\n            }\n            else\n            {\n                if (con.State == System.Data.ConnectionState.Closed)\n                {\n                    con.ConnectionString = JsonConfig.JsonRead(\"TogetherConnectionString\");\n                    con.Open();\n                }\n                    \n            }\n            return con;\n        }\n\n        private static string GetConn()\n        {\n            return JsonConfig.JsonRead(\"TogetherConnectionString\");\n        }\n\n        private static SqlConnection OpenConnection(string name)\n        {\n            if (con == null)\n            {\n                con = new SqlConnection(JsonConfig.JsonRead(name));\n            }\n            else\n            {\n                if (con.State == System.Data.ConnectionState.Closed)\n                    con.Open();\n            }\n            return con;\n        }\n\n\n        public ");
            Write((RepoName).ToString());
            Write("() : base(OpenConnection())\n        {\n            CommonConstruct();\n        }\n\n        public ");
            Write((RepoName).ToString());
            Write("(string connectionStringName) : base(OpenConnection(connectionStringName))\n        {\n            CommonConstruct();\n        }\n\n        partial void CommonConstruct();\n\n        public interface IFactory\n        {\n            ");
            Write((RepoName).ToString());
            Write(" GetInstance();\n        }\n\n        public static IFactory Factory { get; set; }\n        public static ");
            Write((RepoName).ToString());
            Write(" GetInstance()\n        {\n            if (_instance != null)\n                return _instance;\n\n            if (Factory != null)\n                return Factory.GetInstance();\n            else\n                return new ");
            Write((RepoName).ToString());
            Write("();\n        }\n\n        [ThreadStatic] static ");
            Write((RepoName).ToString());
            Write(" _instance;\n\n        public override void OnBeginTransaction()\n        {\n            if (_instance == null)\n                _instance = this;\n        }\n\n        public override void OnEndTransaction()\n        {\n            if (_instance == this)\n                _instance = null;\n        }\n\n        public static int BulkUpdate<T>(string tableName, List<T> data, Func<T, string> funColumns) \n        {\n            try\n            {\n                using (SqlConnection conn = OpenConnection())\n                {\n                    conn.Open();\n\n                    String sql = \"\";\n\n                    foreach (var item in data)\n                    {\n                        sql += string.Format(\"UPDATE dbo.[{0}] SET {1} ;\", tableName, funColumns(item));\n                    }\n\n                    SqlCommand comm = new SqlCommand()\n                    {\n                        CommandText = sql,\n                        Connection = conn\n                    };\n\n                    return comm.ExecuteNonQuery();\n                }\n            }\n            catch (Exception x)\n            {\n                throw x;\n            }\n        }\n\n        public class Record<T> where T : new()\n        {\n            public static ");
            Write((RepoName).ToString());
            Write(" repo { get { return ");
            Write((RepoName).ToString());
            Write(".GetInstance(); } }\n            public bool IsNew() { return repo.IsNew(this); }\n            public object Insert() { return repo.Insert(this); }\n\n            public void Save() { repo.Save(this); }\n            public int Update() { return repo.Update(this); }\n\n            public int Update(IEnumerable<string> columns) { return repo.Update(this, columns); }\n            public static int Update(string sql, params object[] args) { return repo.Update<T>(sql, args); }\n            public static int Update(Sql sql) { return repo.Update<T>(sql); }\n            public int Delete() { return repo.Delete(this); }\n            public static int Delete(string sql, params object[] args) { return repo.Delete<T>(sql, args); }\n            public static int Delete(Sql sql) { return repo.Delete<T>(sql); }\n            public static int Delete(object primaryKey) { return repo.Delete<T>(primaryKey); }\n            public static bool Exists(object primaryKey) { return repo.Exists<T>(primaryKey); }\n            public static T SingleOrDefault(object primaryKey) { return repo.SingleOrDefault<T>(primaryKey); }\n            public static T SingleOrDefault(string sql, params object[] args) { return repo.SingleOrDefault<T>(sql, args); }\n            public static T SingleOrDefault(Sql sql) { return repo.SingleOrDefault<T>(sql); }\n            public static T FirstOrDefault(string sql, params object[] args) { return repo.FirstOrDefault<T>(sql, args); }\n            public static T FirstOrDefault(Sql sql) { return repo.FirstOrDefault<T>(sql); }\n            public static T Single(object primaryKey) { return repo.Single<T>(primaryKey); }\n            public static T Single(string sql, params object[] args) { return repo.Single<T>(sql, args); }\n            public static T Single(Sql sql) { return repo.Single<T>(sql); }\n            public static T First(string sql, params object[] args) { return repo.First<T>(sql, args); }\n            public static T First(Sql sql) { return repo.First<T>(sql); }\n            public static List<T> Fetch(string sql, params object[] args) { return repo.Fetch<T>(sql, args); }\n            public static List<T> Fetch(Sql sql) { return repo.Fetch<T>(sql); }\n\n            public static List<T> Fetch(int page, int itemsPerPage, string sql, params object[] args) { return repo.Fetch<T>(page, itemsPerPage, sql, args); }\n\n            public static List<T> SkipTake(int skip, int take, string sql, params object[] args) { return repo.SkipTake<T>(skip, take, sql, args); }\n            public static List<T> SkipTake(int skip, int take, Sql sql) { return repo.SkipTake<T>(skip, take, sql); }\n            public static Page<T> Page(int page, int itemsPerPage, string sql, params object[] args) { return repo.Page<T>(page, itemsPerPage, sql, args); }\n            public static Page<T> Page(int page, int itemsPerPage, Sql sql) { return repo.Page<T>(page, itemsPerPage, sql); }\n            public static IEnumerable<T> Query(string sql, params object[] args) { return repo.Query<T>(sql, args); }\n            public static IEnumerable<T> Query(Sql sql) { return repo.Query<T>(sql); }\n\n        }\n\n    }\n\n\n");

            foreach (var item in builder.Table)
            {

                Write("\n    \n     [TableName(\"");
                Write((item.TableName).ToString());
                Write("\")]\n     ");

                if (!String.IsNullOrWhiteSpace(item.Primkey))
                {

                    Write("\n[PrimaryKey(\"");
                    Write((item.Primkey).ToString());
                    Write("\"");
                    Write((item.IsIdentity == true ? ")" : ", autoIncrement = false)").ToString());
                    Write("]\n     ");

                }

                Write("\n[ExplicitColumns]\n     public partial class ");
                Write((item.TableNameStr).ToString());
                Write(":");
                Write((RepoName).ToString());
                Write(".Record<");
                Write((item.TableNameStr).ToString());
                Write(">\n     {\n        \n        ");

                foreach (var col in item.Column)
                {

                    Write("\n[Column");
                    Write((col.ColumnAlias != null ? "(\"" + col.ColumnName + "\")" : "").ToString());
                    Write("] public ");
                    Write((col.ColumnType).ToString());
                    Write(" ");
                    Write((col.ColumnAlias != null ? col.ColumnAlias : col.ColumnName).ToString());
                    Write(" {get;set;}\n        ");

                }

                Write("\n\n     }\n");

            }

            Write("\n\n}\n\n\n");

            return GenerationEnvironment.ToString();
        }

        public class T4Builder
        {
            public List<String> TableList = new List<String>();

            public List<TableFrame> Table = new List<TableFrame>();

            public T4Builder(string conectionstring)
            {
                SqlConnection con = new SqlConnection(conectionstring);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("select name from sysobjects where xtype='u'", con);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tablename = reader.GetValue(0).ToString();
                    TableList.Add(tablename);
                }
                con.Close();
                foreach (var tablename in TableList)
                {
                    var TableFrame = new TableFrame();
                    TableFrame.TableName = "dbo." + tablename;

                    #region 获取主键
                    con.Open();
                    SqlCommand cmd_prikey = new SqlCommand("EXEC sp_pkeys @table_name='" + tablename + "' ", con);
                    var key_result = cmd_prikey.ExecuteReader();
                    while (key_result.Read())
                    {
                        TableFrame.Primkey = key_result.GetValue(3) != null ? key_result.GetValue(3).ToString() : null;
                    }
                    con.Close();

                    con.Open();
                    SqlCommand cmd_isIdentity = new SqlCommand(string.Format("select  is_identity as 'identity' from sys.columns where object_ID = object_ID('{0}') and name = '{1}'", tablename, TableFrame.Primkey), con);
                    var identity_result = cmd_isIdentity.ExecuteReader();
                    while (identity_result.Read())
                    {
                        if (identity_result.GetValue(0) != null)
                        {
                            TableFrame.IsIdentity = identity_result.GetValue(0).ToString().ToLower() == "true" ? true : false;
                        }
                    }
                    con.Close();
                    cmd_prikey.Dispose();
                    #endregion

                    #region 获取列名
                    con.Open();
                    SqlCommand cmd_table = new SqlCommand("select COLUMN_NAME,DATA_TYPE,NUMERIC_SCALE,IS_NULLABLE from information_schema.columns where TABLE_NAME='" + tablename + "'", con);
                    var table_result = cmd_table.ExecuteReader();
                    List<Colum> Column = new List<Colum>();

                    while (table_result.Read())
                    {
                        Colum Columindex = new Colum();
                        Columindex.ColumnName = table_result.GetValue(0) != null ? table_result.GetValue(0).ToString() : null;
                        if (!String.IsNullOrEmpty(Columindex.ColumnName))
                        {
                            Columindex.ColumnAlias = tablename.ToString() == Columindex.ColumnName ? "_" + Columindex.ColumnName : null;
                            var ColumTypeStr = GetPropertyType(table_result.GetValue(1) != null ? table_result.GetValue(1).ToString() : null, table_result.GetValue(2) != null ? table_result.GetValue(2).ToString() : null);
                            if (table_result.GetValue(3).ToString() == "YES" && ColumTypeStr != "string" && ColumTypeStr != "Guid")
                            {
                                ColumTypeStr = ColumTypeStr + "?";
                            }

                            Columindex.ColumnType = ColumTypeStr;
                            Column.Add(Columindex);
                        }
                    }
                    con.Close();
                    #endregion
                    table_result.Dispose();
                    TableFrame.Column = Column;
                    Table.Add(TableFrame);
                }
                con.Close();
                con.Dispose();
            }


            private string GetPropertyType(string sqlType, string dataScale)
            {
                string sysType = "string";
                sqlType = sqlType.ToLower();
                switch (sqlType)
                {
                    case "bigint":
                        sysType = "long";
                        break;
                    case "smallint":
                        sysType = "short";
                        break;
                    case "int":
                        sysType = "int";
                        break;
                    case "uniqueidentifier":
                        sysType = "Guid";
                        break;
                    case "smalldatetime":
                    case "datetime":
                    case "date":
                        sysType = "DateTime";
                        break;
                    case "float":
                        sysType = "double";
                        break;
                    case "real":
                    case "numeric":
                    case "smallmoney":
                    case "decimal":
                    case "money":
                    case "number":
                        sysType = "decimal";
                        break;
                    case "tinyint":
                        sysType = "byte";
                        break;
                    case "bit":
                        sysType = "bool";
                        break;
                    case "image":
                    case "binary":
                    case "varbinary":
                    case "timestamp":
                        sysType = "byte[]";
                        break;
                }
                if (sqlType == "number" && dataScale == "0")
                    return "long";

                return sysType;
            }
        }

        public class TableFrame
        {
            public string TableName { get; set; }

            public string Primkey { get; set; }

            public bool IsIdentity { get; set; }

            public List<Colum> Column { get; set; }

            public string TableNameStr
            {
                get
                {
                    return TableName != null ? TableName.Replace("dbo.", "").Trim() : null;
                }
            }
        }

        public class Colum
        {
            public string ColumnName { get; set; }

            public string ColumnType { get; set; }

            public string ColumnAlias { get; set; }
        }

    }
}
