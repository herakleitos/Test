using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.DataAccess.Sql;
using Chaint.Common.Core;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Sql;
using Chaint.Common.Core.Entity;
using Chaint.Common.ServiceHelper;
using Chaint.Common.Interface;
namespace Chaint.Common.Entity
{
    public class AbstractBillModel : IModel, IDisposable
    {
        private Context _context;
        private Dictionary<string, BillHead> _dicHeads;
        private Dictionary<string, string> _dicFieldMap;
        private Dictionary<string, string> _dicFieldTableMap;
        private Dictionary<string, BillEntry> _dicEntrys;
        private Dictionary<string, List<string>> _dicEntryIDs;
        public bool IsDirty = false;
        public bool isSaved = false;
        public AbstractBillModel(Context ctx)
        {
            _context = ctx;
            _dicHeads = new Dictionary<string, BillHead>();
            _dicEntrys = new Dictionary<string, BillEntry>();
            _dicFieldMap = new Dictionary<string, string>();
            _dicFieldTableMap = new Dictionary<string, string>();
            _dicEntryIDs = new Dictionary<string, List<string>>();
        }
        public void Add(BillHead head)
        {
            if (_dicHeads.Keys.Contains(head.TableName)) return;
            _dicHeads.Add(head.TableName, head);
            foreach (var fieldName in head.Fields.Keys)
            {
                if (!_dicFieldMap.Any(a => a.Key.EqualIgnorCase(fieldName)))
                {
                    _dicFieldMap.Add(fieldName, head.Fields[fieldName].DBName);
                }
                if (!_dicFieldTableMap.Any(a => a.Key.EqualIgnorCase(fieldName)))
                {
                    _dicFieldTableMap.Add(fieldName, head.TableName);
                }
            }
        }
        public void Add(BillEntry entry)
        {
            if (_dicEntrys.Keys.Contains(entry.TableName)) return;
            _dicEntrys.Add(entry.TableName, entry);
            foreach (var item in entry.Fields)
            {
                foreach (var fieldName in entry.Fields.Keys)
                {
                    if (!_dicFieldMap.Any(a => a.Key.EqualIgnorCase(fieldName)))
                    {
                        _dicFieldMap.Add(fieldName, entry.Fields[fieldName].DBName);
                    }
                    if (!_dicFieldTableMap.Any(a => a.Key.EqualIgnorCase(fieldName)))
                    {
                        _dicFieldTableMap.Add(fieldName, entry.TableName);
                    }
                }
            }
        }
        public void Remove(string tableName, Field field)
        {
            if (_dicHeads.Keys.Contains(tableName))
            {
                _dicHeads[tableName].Fields.Remove(field.FieldName);
            }
            else if (_dicEntrys.Keys.Contains(tableName))
            {
                _dicEntrys[tableName].Fields.Remove(field.FieldName);
            }
        }
        public void ChangeFieldMap(string tableName, string fieldName, string newDBName)
        {
            if (_dicHeads.Keys.Contains(tableName))
            {
                if (_dicHeads[tableName].Fields.Keys.Contains(fieldName))
                {
                    _dicHeads[tableName].Fields[fieldName].DBName
                        = newDBName;
                }
            }
            else if (_dicEntrys.Keys.Contains(tableName))
            {
                if (_dicEntrys[tableName].Fields.Keys.Contains(fieldName))
                {
                    _dicEntrys[tableName].Fields[fieldName].DBName
                        = newDBName;
                }
            }
            if (_dicFieldMap.Keys.Contains(fieldName))
            {
                _dicFieldMap[fieldName] = newDBName;
            }
        }
        public void Add(string tableName, Field field)
        {
            Remove(tableName, field);
            if (_dicHeads.Keys.Contains(tableName))
            {
                _dicHeads[tableName].Fields.Add(field.FieldName, field);
            }
            else if (_dicEntrys.Keys.Contains(tableName))
            {
                _dicEntrys[tableName].Fields.Add(field.FieldName, field);
            }
            if (!_dicFieldMap.Any(a => a.Key.EqualIgnorCase(field.FieldName)))
            {
                _dicFieldMap.Add(field.FieldName, field.DBName);
            }
            if (!_dicFieldTableMap.Any(a => a.Key.EqualIgnorCase(field.FieldName)))
            {
                _dicFieldTableMap.Add(field.FieldName, tableName);
            }
        }
        public void BindEntryData(string tableName, DataTable data)
        {
            if (_dicEntrys.Keys.Contains(tableName))
            {
                _dicEntrys[tableName].Data = data;
            }
            if (_dicEntryIDs.Keys.Contains(tableName))
            {
                _dicEntryIDs.Remove(tableName);
            }
            List<string> fids = new List<string>();
            foreach (DataRow row in data.Rows)
            {
                string pkId = Convert.ToString(row[_dicEntrys[tableName].PrimaryKey]);
                if (pkId.IsNullOrEmptyOrWhiteSpace()) continue;
                fids.Add(pkId);
            }
            _dicEntryIDs.Add(tableName, fids);
        }
        public void SetValue(string fieldname, object value, int row = -1)
        {
            string tableName = string.Empty;
            if (!_dicFieldTableMap.TryGetValue(fieldname, out tableName)) return;
            if (row < 0)//表头
            {
                if (!_dicHeads.Keys.Contains(tableName)) return;
                if (!_dicHeads[tableName].Fields.Keys.Contains(fieldname)) return;
                Field field = _dicHeads[tableName].Fields[fieldname];
                //如果是null或者空
                if (value == null || value.Equals(string.Empty))
                {
                    if (field.Type == Enums_FieldType.Decimal || field.Type == Enums_FieldType.Int32 ||
                        field.Type == Enums_FieldType.Int64)
                    {
                        field.Value = null;
                    }
                    else if (field.Type == Enums_FieldType.String)
                    {
                        field.Value = string.Empty;
                    }
                    else
                    {
                        field.Value = value;
                    }
                }
                else
                {
                    field.Value = value;
                }
            }
            else//表体
            {
                if (!_dicEntrys.Keys.Contains(tableName)) return;
                if (!_dicEntrys[tableName].Fields.Keys.Contains(fieldname)) return;
                string dbName = string.Empty;
                if (!_dicFieldMap.TryGetValue(fieldname, out dbName)) return;
                if (_dicEntrys[tableName].Data == null) return;
                if (_dicEntrys[tableName].Data.Rows.Count < row + 1) return;
                try
                {
                    _dicEntrys[tableName].Data.Rows[row][dbName] = value;
                }
                catch { };
            }
        }
        public T GetValue<T>(string name, int row = -1)
        {
            string tableName = string.Empty;
            if (!_dicFieldTableMap.TryGetValue(name, out tableName)) return default(T);
            if (row < 0)
            {
                if (!_dicHeads.Keys.Contains(tableName)) return default(T);
                if (!_dicHeads[tableName].Fields.Keys.Contains(name)) return default(T);
                try
                {
                    object value = _dicHeads[tableName].Fields[name].Value;
                    if (value == null) return default(T);
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else
            {
                string dbName = string.Empty;
                if (!_dicFieldMap.TryGetValue(name, out dbName)) return default(T);
                if (!_dicEntrys.Keys.Contains(tableName)) return default(T);
                if (!_dicEntrys[tableName].Fields.Keys.Contains(name)) return default(T);
                if (_dicEntrys[tableName].Data == null) return default(T);
                if (_dicEntrys[tableName].Data.Rows.Count < row + 1) return default(T);
                try
                {
                    object value = _dicEntrys[tableName].Data.Rows[row][dbName];
                    if (value == null) return default(T);
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); };
            }
        }
        public BillHead GetBillHead(string headTableName)
        {
            BillHead result = null;
            if (!_dicHeads.TryGetValue(headTableName, out result) || result == null) return null;
            return result;
        }
        public BillEntry GetBillEntry(string entryTableName)
        {
            BillEntry result = null;
            if (!_dicEntrys.TryGetValue(entryTableName, out result) || result == null) return null;
            return result;
        }
        public DataTable GetBillEntryData(string entryTableName)
        {
            BillEntry result = null;
            if (!_dicEntrys.TryGetValue(entryTableName, out result) || result == null) return null;
            return result.Data;
        }
        public OperationResult Save()
        {
            List<ExcuteObject> exeObjs = GetDeleteExcuteObj(true);//如果当前页面保存过，需要先删除
            //保存表头
            foreach (string tableName in _dicHeads.Keys)
            {
                if (!_dicHeads[tableName].NeedSave) continue;
                Dictionary<string, Field> fields = _dicHeads[tableName].Fields;
                if (fields.IsEmpty()) continue;
                InsertObject obj = new InsertObject();
                obj.TableName = tableName;
                foreach (var item in fields.Values)
                {
                    string parameterName =
                        string.Format("@{0}", item.DBName);
                    object parameterValue =
                        item.Type == Core.Enums.Enums_FieldType.String ?
                        Convert.ToString(item.Value).Trim() : item.Value;
                    if (parameterValue == null) continue;
                    InsertItem insertItem =
                        new InsertItem(item.DBName, parameterName, parameterValue, item.Type);
                    obj.AddInsertItem(insertItem);
                }
                if (obj.InsertItems.IsEmpty()) continue;
                ExcuteObject exeObj = new ExcuteObject();
                exeObj.Sql = obj.ToSqlString();
                exeObj.Parameters = obj.SqlParameters;
                exeObjs.Add(exeObj);
            }
            foreach (string tablename in _dicEntrys.Keys)
            {
                //约定表体数据源DataTable的列名和对应数据库表中的列名必须一致
                var entry = _dicEntrys[tablename];
                entry.Data.AcceptChanges();
                BatchInsertParam para = new BatchInsertParam();
                para.TableName = tablename;
                if (entry.CheckField.IsNullOrEmptyOrWhiteSpace())
                {
                    para.Data = entry.Data;
                }
                else
                {//如果表体有勾选框字段，则仅插入勾选上的分录，并且勾选框字段不保存到数据库中
                    DataTable dt = entry.Data.Clone();
                    foreach (DataRow row in entry.Data.Rows)
                    {
                        int checkStatus =
                            Convert.ToInt32(row[entry.CheckField]);
                        if (checkStatus == 1)
                        {
                            dt.ImportRow(row);
                        }
                    }
                    dt.Columns.Remove(entry.CheckField);
                    dt.AcceptChanges();
                    para.Data = dt;
                }
                if (para.Data.Rows.Count <= 0) continue;
                ReplaceSpace(para.Data, entry.Fields.Values.ToList());
                foreach (var item in entry.Fields.Values)
                {
                    para.FieldMapping.Add(item.DBName, item.DBName);
                }

                ExcuteObject exeObj = new ExcuteObject();
                exeObj.BatchInsertParam = para;
                exeObjs.Add(exeObj);
            }
            //使用带事务的保存方法
            OperationResult saveResult = DBAccessServiceHelper.ExcuteWithTransaction(_context, exeObjs);
            if (saveResult.IsSuccess)
            {
                this.isSaved = true;
                this.IsDirty = false;
            }
            return saveResult;
        }
        //去除空格
        private void ReplaceSpace(DataTable dt, List<Field> fields)
        {
            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in fields)
                {
                    if (row[item.DBName] is string)
                    {
                        row[item.DBName] =
                            Convert.ToString(row[item.DBName]).Trim();
                    }
                }
            }
        }
        public OperationResult Delete()
        {
            List<ExcuteObject> exeObjs = GetDeleteExcuteObj(false);
            OperationResult delResult = DBAccessServiceHelper.ExcuteWithTransaction(_context, exeObjs);
            return delResult;
        }
        private List<ExcuteObject> GetDeleteExcuteObj(bool beforeSave)
        {
            List<ExcuteObject> exeObjs = new List<ExcuteObject>();
            //model的删除对于表头来说，一张表每次只删除一条数据，所以table和fid是一一对应的
            Dictionary<string, object> fids = new Dictionary<string, object>();//记录删除的表头fid
            int i = 0;
            foreach (string tableName in _dicHeads.Keys)
            {
                if (!_dicHeads[tableName].NeedSave) continue;
                i++;
                DeleteObject delObj = new DeleteObject();
                delObj.TableName = tableName;
                string fidKey = _dicHeads[tableName].PrimaryKey;
                Field fidField;
                if (!_dicHeads[tableName].Fields.TryGetValue(_dicHeads[tableName].PrimaryKey, out fidField))
                    continue;
                fids.Add(tableName, fidField.Value);
                string parameterName =
                   string.Format("@{0}{1}", fidField.DBName, i);
                WhereItem whereItem = new WhereItem(fidField.DBName, parameterName, fidField.Value, fidField.Type);
                delObj.WhereItems.Add(whereItem);
                ExcuteObject exeObj = new ExcuteObject();
                exeObj.Sql = delObj.ToSqlString();
                exeObj.Parameters = delObj.SqlParameters;
                exeObjs.Add(exeObj);
            }
            int j = 0;
            foreach (string tableName in _dicEntrys.Keys)
            {
                j++;
                //如果不是保存之前的删除,并且表体存在表头，直接使用表头主键删除整个表体
                if (!beforeSave && !_dicEntrys[tableName].ParentPrimaryKey.IsNullOrEmptyOrWhiteSpace())
                {
                    Field fidField;
                    if (!_dicEntrys[tableName].Fields.TryGetValue(_dicEntrys[tableName].ParentPrimaryKey, out fidField))
                        continue;
                    object fidValue;
                    if (!fids.TryGetValue(_dicEntrys[tableName].ParentTable, out fidValue)) continue;
                    DeleteObject delObj = new DeleteObject();
                    delObj.TableName = tableName;
                    string parameterName =
                     string.Format("@{0}{1}", fidField.DBName, j);
                    WhereItem whereItem = new WhereItem(fidField.DBName, parameterName, fidValue, fidField.Type);
                    delObj.WhereItems.Add(whereItem);
                    ExcuteObject exeObj = new ExcuteObject();
                    exeObj.Sql = delObj.ToSqlString();
                    exeObj.Parameters = delObj.SqlParameters;
                    exeObjs.Add(exeObj);
                }
                else//如果是保存之前的删除，则使用表体分录主键删除需要更新的分录行
                {
                    Field fidField;
                    if (!_dicEntrys[tableName].Fields.TryGetValue(_dicEntrys[tableName].PrimaryKey, out fidField))
                        continue;
                    DeleteObject delObj = new DeleteObject();
                    delObj.TableName = tableName;
                    WhereItem whereItem = new WhereItem(fidField.DBName, fidField.Type);
                    List<string> entryids = new List<string>();
                    if (_dicEntrys[tableName].CheckField.IsNullOrEmptyOrWhiteSpace())
                    {
                        _dicEntryIDs.TryGetValue(tableName, out entryids);
                    }
                    foreach (DataRow row in _dicEntrys[tableName].Data.Rows)
                    {

                        bool isDelete = true;
                        if (!_dicEntrys[tableName].CheckField.IsNullOrEmptyOrWhiteSpace())
                        {//如果有勾选框这一列，那么勾选了的才被删除，没勾选的不删除
                         //如果没有勾选框这一列，则全部删除
                            int checkStatus =
                            Convert.ToInt32(row[_dicEntrys[tableName].CheckField]);
                            if (checkStatus == 0)
                                isDelete = false;
                        }
                        if (isDelete)
                        {
                            string parameterValue = Convert.ToString(row[fidField.DBName]);
                            entryids.Add(parameterValue);
                        }
                    }
                    int k = 0;
                    if (entryids.IsEmpty()) continue;
                    foreach (string id in entryids.Distinct())
                    {
                        k++;
                        string parameterName =
                            string.Format("@{0}{1}", fidField.DBName.Trim(), j + k);
                        whereItem.InOption.Add(new SqlParam(parameterName, id, fidField.Type));
                    }
                    delObj.WhereItems.Add(whereItem);
                    ExcuteObject exeObj = new ExcuteObject();
                    exeObj.Sql = delObj.ToSqlString();
                    exeObj.Parameters = delObj.SqlParameters;
                    exeObjs.Add(exeObj);
                }
            }
            return exeObjs;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    _dicHeads.Clear();
                    _dicFieldMap.Clear();
                    _dicFieldTableMap.Clear();
                    _dicEntrys.Clear();
                    _dicEntryIDs.Clear();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~AbstractBillModel() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
