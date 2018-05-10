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
        /// <param name="fielName">�ֶ�����</param>
        /// <param name="fieldValue">�ֶ�ֵ</param>
        /// <param name="sqlOperator">SqlOperatorö������</param>
        /// <returns>�����������Hashtable</returns>
        public SearchCondition AddCondition(string fielName, object fieldValue, SqlOperator sqlOperator)
        {
            // this.conditionTable.Add(fielName, new SearchInfo(fielName, fieldValue, sqlOperator));
            this.lstCodition.Add(new SearchInfo(fielName, fieldValue, sqlOperator));
            return this;
        }

        /// <summary>
        /// Ϊ��ѯ�������
        /// <example>
        /// �÷�һ��
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual, false);
        /// searchObj.AddCondition("Test2", "Test2Value", SqlOperator.Like, true);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// 
        /// �÷�����AddCondition�������Դ�������Ӷ������
        /// SearchCondition searchObj = new SearchCondition();
        /// searchObj.AddCondition("Test", 1, SqlOperator.NotEqual, false).AddCondition("Test2", "Test2Value", SqlOperator.Like, true);
        /// string conditionSql = searchObj.BuildConditionSql();
        /// </example>
        /// </summary>
        /// <param name="fielName">�ֶ�����</param>
        /// <param name="fieldValue">�ֶ�ֵ</param>
        /// <param name="sqlOperator">SqlOperatorö������</param>
        /// <param name="excludeIfEmpty">����ֶ�Ϊ�ջ���Null����Ϊ��ѯ����</param>
        /// <returns></returns>
        public SearchCondition AddCondition(string fielName, object fieldValue, SqlOperator sqlOperator, bool excludeIfEmpty)
        {
            //this.conditionTable.Add(fielName, new SearchInfo(fielName, fieldValue, sqlOperator, excludeIfEmpty));
            this.lstCodition.Add(new SearchInfo(fielName, fieldValue, sqlOperator, excludeIfEmpty));
            return this;
        }

        /// <summary>
        /// ���ݶ�������ص�������䣨��ʹ�ò��������緵�ص������:
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
                //���ѡ��ExcludeIfEmptyΪTrue,���Ҹ��ֶ�Ϊ��ֵ�Ļ�,����
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
        /// �ų�Where �ؼ���
        /// </summary>
        /// <returns></returns>
        public string BuildConditionSqlExcludeWhere()
        {
            string sql = "(1=1) ";
            string fieldName = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (SearchInfo searchInfo in lstCodition)
            {
                //���ѡ��ExcludeIfEmptyΪTrue,���Ҹ��ֶ�Ϊ��ֵ�Ļ�,����
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


        #region ��������

        /// <summary>
        /// ת��ö������Ϊ��Ӧ��Sql����������
        /// </summary>
        /// <param name="sqlOperator">SqlOperatorö�ٶ���</param>
        /// <returns><![CDATA[��Ӧ��Sql���������ţ��� ">" "<>" ">=")]]></returns>
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
        /// ���ݴ�������ֵ���ͻ�ȡ���Ӧ��DbType����
        /// </summary>
        /// <param name="fieldValue">�����ֵ</param>
        /// <returns>DbType����</returns>
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
    /// ��ѯ��Ϣʵ����
    /// </summary>
    public class SearchInfo
    {
        private string fieldName;
        private object fieldValue;
        private SqlOperator sqlOperator;
        private bool excludeIfEmpty = false;

        public SearchInfo() { }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="fieldName">�ֶ�����</param>
        /// <param name="fieldValue">�ֶε�ֵ</param>
        /// <param name="sqlOperator">�ֶε�Sql��������</param>
        public SearchInfo(string fieldName, object fieldValue, SqlOperator sqlOperator)
            : this(fieldName, fieldValue, sqlOperator, false)
        { }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="fieldName">�ֶ�����</param>
        /// <param name="fieldValue">�ֶε�ֵ</param>
        /// <param name="sqlOperator">�ֶε�Sql��������</param>
        /// <param name="excludeIfEmpty">����ֶ�Ϊ�ջ���Null����Ϊ��ѯ����</param>
        public SearchInfo(string fieldName, object fieldValue, SqlOperator sqlOperator, bool excludeIfEmpty)
        {
            this.fieldName = fieldName;
            this.fieldValue = fieldValue;
            this.sqlOperator = sqlOperator;
            this.excludeIfEmpty = excludeIfEmpty;
        }

        /// <summary>
        /// �ֶ�����
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// �ֶε�ֵ
        /// </summary>
        public object FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }

        /// <summary>
        /// �ֶε�Sql��������
        /// </summary>
        public SqlOperator SqlOperator
        {
            get { return sqlOperator; }
            set { sqlOperator = value; }
        }

        /// <summary>
        /// ����ֶ�Ϊ�ջ���Null����Ϊ��ѯ����
        /// </summary>
        public bool ExcludeIfEmpty
        {
            get { return excludeIfEmpty; }
            set { excludeIfEmpty = value; }
        }
    }




}
