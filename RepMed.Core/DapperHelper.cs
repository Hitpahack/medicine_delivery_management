using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;

namespace RepMed.Core
{
    public static class DapperHelper
    {
        #region Select Query

        public static string SelectAll(this string tableName, string whereQuery = "")
        {
            var query = string.Concat($@"SELECT * FROM {tableName}");
            if (!string.IsNullOrEmpty(whereQuery))
                query =  string.Concat(query, $@" where {whereQuery}");

            return query;
        }
        public static string Select(this string tableName, string[] columns)
        {
            return SelectQuery(tableName, columns: columns);
        }
        public static string Select(this string tableName, string[] columns, string whereQuery = "")
        {
            return SelectQuery(tableName, whereQuery: whereQuery, columns: columns);
        }
        public static string Select(this string tableName, string[] columns, string tblPrefix, string whereQuery = "")
        {
            return SelectQuery(tableName, tblPrefix: tblPrefix, whereQuery: whereQuery, columns: columns);
        }
        private static string SelectQuery(this string tableName, string[] columns, string tblPrefix = "", string whereQuery = "")
        {

            if (!string.IsNullOrEmpty(tblPrefix))
            {
                columns = columns.Select(r => string.Concat(tblPrefix, ".", '"', r, '"')).ToArray();
                tableName = string.Concat(tableName, " ", tblPrefix);
            }
            else
            {
                columns = columns.Select(r => string.Concat('"', r, '"')).ToArray();
            }


            var query = string.Concat($@"SELECT {string.Join(",", columns)} FROM {tableName}");
            if (!string.IsNullOrEmpty(whereQuery))
                query = string.Concat(query, $@" where {whereQuery}");

            return query;
        }
        #endregion

        #region Insert Query
        public static TResult Insert<TResult>(this IDbConnection con, IDbTransaction tran,
            string tableName, string tblcolumns,
            string dataColumn,
            object data,
            string returningData = "") where TResult : class
        {
            string sql = $@"INSERT INTO {tableName} ({tblcolumns}) VALUES ({dataColumn}) {returningData}";

            return con.QueryFirstOrDefault<TResult>(sql, data, tran);
        }

        public static TResult Insert<TResult>(this IDbConnection con, IDbTransaction tran,
           string tableName, Dictionary<string, string> updateData,
           string returningData = "") where TResult : class
        {
            var tblcolumns = string.Join(",", updateData.Select(r => string.Concat(@$" ""{r.Key}"" ")));
            var dataValues = string.Join(",", updateData.Select(r => string.Concat(@$" '{r.Value}' ")));

            string sql = $@"INSERT INTO {tableName} ({tblcolumns}) VALUES ({dataValues}) {returningData}";

            return con.QueryFirstOrDefault<TResult>(sql, transaction: tran);
        }

        public static TResult Insert<TResult>(this IDbConnection con, IDbTransaction tran,
          string tableName, Dictionary<string, object> updateData,
          string returningData = "") where TResult : class
        {
            var tblcolumns = string.Join(",", updateData.Select(r => string.Concat(@$" ""{r.Key}"" ")));
            var dataColumn = string.Join(",", updateData.Select(r => string.Concat(@$" @{r.Key} ")));

            string sql = $@"INSERT INTO {tableName} ({tblcolumns}) VALUES ({dataColumn}) {returningData}";

            return con.QueryFirstOrDefault<TResult>(sql, updateData, tran);
        }

        public static int Insert(this IDbConnection con, IDbTransaction tran,
           string tableName, Dictionary<string, string> updateData,
           string returningData = "") 
        {
            var tblcolumns = string.Join(",", updateData.Select(r => string.Concat(@$" ""{r.Key}"" ")));
            var dataValues = string.Join(",", updateData.Select(r => string.Concat(@$" '{r.Value}' ")));

            string sql = $@"INSERT INTO {tableName} ({tblcolumns}) VALUES ({dataValues}) {returningData}";

            return con.Execute(sql, transaction: tran);
        }
        #endregion

        #region Update Query
        public static TResult Update<TResult>(this IDbConnection con, IDbTransaction tran,
            string tableName, Dictionary<string,string> updateData,
            string wherQuery = "", string returningData = "") where TResult : class
        {
            var updatecolumQury = string.Join(",", updateData.Select(r => string.Concat(@$" ""{r.Key}""='{r.Value}'")));
            string sql = $@"UPDATE {tableName} SET {updatecolumQury} ";

            if (!string.IsNullOrEmpty(wherQuery))
                sql = string.Concat(sql, $" WHERE {wherQuery} ");

            sql = string.Concat(sql, returningData);

            return con.QueryFirstOrDefault<TResult>(sql, transaction: tran);
        }

        public static TResult Update<TResult>(this IDbConnection con, IDbTransaction tran,
          string tableName, Dictionary<string, object> updateData,
          string wherQuery = "", string returningData = "") where TResult : class
        {
            var updatecolumQury = string.Join(",", updateData.Select(r => string.Concat(@$" ""{r.Key}""='{r.Value}'")));
            
            string sql = $@"UPDATE {tableName} SET {updatecolumQury} ";

            if (!string.IsNullOrEmpty(wherQuery))
                sql = string.Concat(sql, $" WHERE {wherQuery} ");

            sql = string.Concat(sql, returningData);

            return con.QueryFirstOrDefault<TResult>(sql, transaction: tran);
        }

        public static TResult Update<TResult>(this IDbConnection con, IDbTransaction tran,
          string tableName, Dictionary<string, string> updateKeyValueColumns, object data,
          string wherQuery = "", string returningData = "") where TResult : class
        {
            var updatecolumQury = string.Join(",", updateKeyValueColumns.Select(r => string.Concat(@$" ""{r.Key}""=@{r.Value}")));
            
            string sql = $@"UPDATE {tableName} SET {updatecolumQury} ";

            if (!string.IsNullOrEmpty(wherQuery))
                sql = string.Concat(sql, $" WHERE {wherQuery} ");

            sql = string.Concat(sql, returningData);

            return con.QueryFirstOrDefault<TResult>(sql, data, transaction: tran);
        }

        #endregion

       
        public static string QueryAsValuesParma<T>(string prefix = "", params string[] excludesProperties) where T : class
        {
            var query_ = AsQueryParma(typeof(T), prefix, excludesProperties);
            return string.Join(",", query_.Select(r => r));

        }
        public static string QueryAsColumnsParma<T>(string prefix = "", params string[] excludesProperties) where T : class
        {
            var query_ = AsQueryParma(typeof(T), "", excludesProperties);
            return string.Join(",", query_.Select(r => string.Concat(prefix, '"', r, '"')));

        }

        public static string QueryAsColumnsParma<T, T2>(string prefix = "", params string[] excludesProperties) where T : class
        {
            var query_ = AsQueryParma(typeof(T), typeof(T2), "", excludesProperties);
            return string.Join(",", query_.Select(r => string.Concat(prefix, '"', r, '"')));

        }
        public static string QueryAsValuesParma<T, T2>(params string[] excludesProperties) where T : class
        {
            var query_ = AsQueryParma(typeof(T), typeof(T2), "@", excludesProperties);
            return string.Join(",", query_.Select(r => string.Concat(r)));

        }
       
        public static string QueryAsValuesParma<T>(this T classs, string prefix = "", params string[] excludesProperties) where T : class
        {
            var query_ = AsQueryParma(classs, prefix, excludesProperties);
            return string.Join(",", query_.Select(r => r));

        }
        public static string QueryAsColumnsParma<T>(this T classs, string prefix = "", params string[] excludesProperties) where T : class
        {
            var query_ = AsQueryParma(classs, "", excludesProperties);
            return string.Join(",", query_.Select(r => string.Concat(prefix, '"', r, '"')));

        }
        public static IEnumerable<string> AsQueryParma<T>(this T classs, string prefix = "", params string[] excludesProperties) where T : class
        {

            excludesProperties = excludesProperties.Select(r => r.ToLower()).ToArray();

            if (!string.IsNullOrEmpty(prefix))
                return typeof(T).GetProperties().Where(p =>
                !p.IsDefined(typeof(IgnoreDapperAttribute), true) && !excludesProperties.Contains(p.Name.ToLower()))
                    .Select(r => prefix + r.Name);
            else
                return typeof(T).GetProperties().Where(p =>
                !p.IsDefined(typeof(IgnoreDapperAttribute), true) && !excludesProperties.Contains(p.Name.ToLower())).Select(r =>
                r.Name);

        }

        public static IEnumerable<string> AsQueryParma(this Type classs, string prefix = "", params string[] excludesProperties)
        {

            excludesProperties = excludesProperties.Select(r => r.ToLower()).ToArray();

            if (!string.IsNullOrEmpty(prefix))
            {
                return classs.GetProperties().Where(p =>
                !p.IsDefined(typeof(IgnoreDapperAttribute), true) && !excludesProperties.Contains(p.Name.ToLower()))
                    .Select(r => (prefix + r.Name));
            }
            else
            {
                return classs.GetProperties().Where(p =>
                !p.IsDefined(typeof(IgnoreDapperAttribute), true) && !excludesProperties.Contains(p.Name.ToLower())).Select(r =>
                 (r.Name));
            }

        }
        public static IEnumerable<string> AsQueryParma(this Type entity, Type dto, string prefix = "", params string[] excludesProperties)
        {

            excludesProperties = excludesProperties.Select(r => r.ToLower()).ToArray();

            if (!string.IsNullOrEmpty(prefix))
            {
                var entityColumns = entity.GetProperties().Select(r => r.Name.ToLower());
                var dtoColumns = dto.GetProperties().Where(p => !p.IsDefined(typeof(IgnoreDapperAttribute), true) &&
                !excludesProperties.Contains(p.Name.ToLower()))
                    .Select(r => r.Name);

                return dtoColumns.Where(p => entityColumns.Contains(p.ToLower())).Select(r => (prefix + r));
            }
            else
            {
                var entityColumns = entity.GetProperties().Select(r => r.Name.ToLower());
                var dtoColumns = dto.GetProperties().Where(p => !p.IsDefined(typeof(IgnoreDapperAttribute), true) &&
                !excludesProperties.Contains(p.Name.ToLower())).Select(r =>
                (r.Name));

                return dtoColumns.Where(p => entityColumns.Contains(p.ToLower()));
            }

        }


    }


    public class DbTables
    {
        public const string tblUser = @" dbo.""User"" ";
        public const string tblPersons = @" dbo.""Persons"" ";
        public const string tblUserContacts = @" dbo.""UserContacts"" ";
        public const string tblUserRolePermission = @" dbo.""UserRolePermission"" ";
        public const string tblUserRoles = @" dbo.""UserRoles"" ";
        public const string tblUserTokenLog = @" dbo.""UserTokenLog"" ";
        public const string tblRole = @" dbo.""Role"" ";
        public const string tblCountry = @" dbo.""Country"" ";
        public const string tblStates = @" dbo.""State"" ";
        public const string tblCity = @" dbo.""City"" ";
        public const string tblProviders = @" dbo.""Providers"" ";
        public const string tblCodeRequest = @" dbo.""CodeRequest"" ";
        public const string tblProviderCategory = @" dbo.""ProviderCategories"" ";
        public const string tblAssessments = @" dbo.""Assessments"" ";
        public const string tblCustomFields = @" dbo.""CustomFields"" ";
        public const string tblUserCustomFields = @" dbo.""UserCustomFields"" ";
        public const string tblUserAssessments = @" dbo.""UserAssessments"" ";

    }

}
