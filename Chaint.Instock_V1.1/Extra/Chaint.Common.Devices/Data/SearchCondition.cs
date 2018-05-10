using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Data;

namespace Chaint.Common.Devices.Data
{

    public class SearchCondition
    {
        private IList<SearchInfo> lstCodition = new List<SearchInfo>();

        public IList<SearchInfo> ConditionInfo
        {
            get { return this.lstCodition; }
        }
        /// </example>
        /// </summary>
        /// <param name="fielName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="sqlOperator">SqlOperator枚举类型</param>
        /// <returns>增加条件后的Hashtable</returns>
        public SearchCondition AddCondition(string fielName, object fieldValue, SqlOperator sqlOperator)
        {
            // this.conditionTable.Add(fielName, new SearchInfo(fielName, fieldValue, sqlOperator));
            this.lstCodition.Add(new SearchInfo(fielName, fieldValue, sqlOperator));
            return this;
        }

        /// <summary>
        /// 为查询添加条件
        /// <example>
        /// 用法一：
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual, false);
        /// searchObj.AddCondition("Test2", "Test2Value", SqlOperator.Like, true);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// 
        /// 用法二：AddCondition函数可以串起来添加多个条件
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual, false).AddCondition("Test2", "Test2Value", SqlOperator.Like, true);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// </example>
        /// </summary>
        /// <param name="fielName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="sqlOperator">SqlOperator枚举类型</param>
        /// <param name="excludeIfEmpty">如果字段为空或者Null则不作为查询条件</param>
        /// <returns></returns>
        public SearchCondition AddCondition(string fielName, object fieldValue, SqlOperator sqlOperator, bool excludeIfEmpty)
        {
            //this.conditionTable.Add(fielName, new SearchInfo(fielName, fieldValue, sqlOperator, excludeIfEmpty));
            this.lstCodition.Add(new SearchInfo(fielName, fieldValue, sqlOperator, excludeIfEmpty));
            return this;
        }

        /// <summary>
        /// 根据对象构造相关的条件语句（不使用参数），如返回的语句是:
        /// <![CDATA[
        /// Where (1=1)  AND Test4  <  'Value4' AND Test6  >=  'Value6' AND Test7  <=  'value7' AND Test  <>  '1' AND Test5  >  'Value5' AND Test2  Like  '%Value2%' AND Test3  =  'Value3'
        /// ]]>
        /// </summary>
        /// <returns></returns> 
        public string BuildConditionSql()
        {
            string sql = " Where (1=1) ";
            string fieldName = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (SearchInfo searchInfo in lstCodition)
            {
                //如果选择ExcludeIfEmpty为True,并且该字段为空值的话,跳过
                if (searchInfo.ExcludeIfEmpty && string.IsNullOrEmpty((string)searchInfo.FieldValue))
                {
                    continue;
                }

                if (searchInfo.SqlOperator == SqlOperator.LIKE)
                {
                    sb.AppendFormat(" AND {0} {1} '{2}'", searchInfo.FieldName,
                        this.ConvertSqlOperator(searchInfo.SqlOperator), string.Format("%{0}%", searchInfo.FieldValue));
                }
                else
                {
                    sb.AppendFormat(" AND {0} {1} '{2}'", searchInfo.FieldName,
                        this.ConvertSqlOperator(searchInfo.SqlOperator), searchInfo.FieldValue);
                }
            }
            sql += sb.ToString();
            return sql;
        }

        /// <summary>
        /// 排除Where 关键字
        /// </summary>
        /// <returns></returns>
        public string BuildConditionSqlExcludeWhere()
        {
            string sql = "(1=1) ";
            string fieldName = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (SearchInfo searchInfo in lstCodition)
            {
                //如果选择ExcludeIfEmpty为True,并且该字段为空值的话,跳过
                if (searchInfo.ExcludeIfEmpty && string.IsNullOrEmpty((string)searchInfo.FieldValue))
                {
                    continue;
                }

                if (searchInfo.SqlOperator == SqlOperator.LIKE)
                {
                    sb.AppendFormat(" AND {0} {1} '{2}'", searchInfo.FieldName,
                        this.ConvertSqlOperator(searchInfo.SqlOperator), string.Format("%{0}%", searchInfo.FieldValue));
                }
                else
                {
                    sb.AppendFormat(" AND {0} {1} '{2}'", searchInfo.FieldName,
                        this.ConvertSqlOperator(searchInfo.SqlOperator), searchInfo.FieldValue);
                }
            }
            sql += sb.ToString();
            return sql;
        }


        #region 辅助函数

        /// <summary>
        /// 转换枚举类型为对应的Sql语句操作符号
        /// </summary>
        /// <param name="sqlOperator">SqlOperator枚举对象</param>
        /// <returns><![CDATA[对应的Sql语句操作符号（如 ">" "<>" ">=")]]></returns>
        private string ConvertSqlOperator(SqlOperator sqlOperator)
        {
            string stringOperator = " = ";
            switch (sqlOperator)
            {
                case SqlOperator.EQUAL:
                    stringOperator = " = ";
                    break;
                case SqlOperator.SMALLER:
                    stringOperator = " < ";
                    break;
                case SqlOperator.SMALLER_EQUAL:
                    stringOperator = " <= ";
                    break;
                case SqlOperator.LIKE:
                    stringOperator = " Like ";
                    break;
                case SqlOperator.GREATER:
                    stringOperator = " > ";
                    break;
                case SqlOperator.GREATER_EQUAL:
                    stringOperator = " >= ";
                    break;
                case SqlOperator.NOTEQUAL:
                    stringOperator = " <> ";
                    break;
                default:
                    break;
            }
            return stringOperator;
        }

        /// <summary>
        /// 根据传入对象的值类型获取其对应的DbType类型
        /// </summary>
        /// <param name="fieldValue">对象的值</param>
        /// <returns>DbType类型</returns>
        private DbType GetFieldDbType(object fieldValue)
        {
            DbType type = DbType.String;

            switch (fieldValue.GetType().ToString())
            {
                case "System.Int16":
                    type = DbType.Int16;
                    break;
                case "System.UInt16":
                    type = DbType.UInt16;
                    break;
                case "System.Single":
                    type = DbType.Single;
                    break;
                case "System.UInt32":
                    type = DbType.UInt32;
                    break;
                case "System.Int32":
                    type = DbType.Int32;
                    break;
                case "System.UInt64":
                    type = DbType.UInt64;
                    break;
                case "System.Int64":
                    type = DbType.Int64;
                    break;
                case "System.String":
                    type = DbType.String;
                    break;
                case "System.Double":
                    type = DbType.Double;
                    break;
                case "System.Decimal":
                    type = DbType.Decimal;
                    break;
                case "System.Byte":
                    type = DbType.Byte;
                    break;
                case "System.Boolean":
                    type = DbType.Boolean;
                    break;
                case "System.DateTime":
                    type = DbType.DateTime;
                    break;
                case "System.Guid":
                    type = DbType.Guid;
                    break;
                default:
                    break;
            }
            return type;
        }
        #endregion
    }

    public enum SqlOperator
    {
        LIKE,
        GREATER,
        GREATER_EQUAL,
        SMALLER,
        SMALLER_EQUAL,
        EQUAL,
        NOTEQUAL,
        IS
    }

    /// <summary>
    /// 查询信息实体类
    /// </summary>
    public class SearchInfo
    {
        private string fieldName;
        private object fieldValue;
        private SqlOperator sqlOperator;
        private bool excludeIfEmpty = false;

        public SearchInfo() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段的值</param>
        /// <param name="sqlOperator">字段的Sql操作符号</param>
        public SearchInfo(string fieldName, object fieldValue, SqlOperator sqlOperator)
            : this(fieldName, fieldValue, sqlOperator, false)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段的值</param>
        /// <param name="sqlOperator">字段的Sql操作符号</param>
        /// <param name="excludeIfEmpty">如果字段为空或者Null则不作为查询条件</param>
        public SearchInfo(string fieldName, object fieldValue, SqlOperator sqlOperator, bool excludeIfEmpty)
        {
            this.fieldName = fieldName;
            this.fieldValue = fieldValue;
            this.sqlOperator = sqlOperator;
            this.excludeIfEmpty = excludeIfEmpty;
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// 字段的值
        /// </summary>
        public object FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }

        /// <summary>
        /// 字段的Sql操作符号
        /// </summary>
        public SqlOperator SqlOperator
        {
            get { return sqlOperator; }
            set { sqlOperator = value; }
        }

        /// <summary>
        /// 如果字段为空或者Null则不作为查询条件
        /// </summary>
        public bool ExcludeIfEmpty
        {
            get { return excludeIfEmpty; }
            set { excludeIfEmpty = value; }
        }
    }




}
