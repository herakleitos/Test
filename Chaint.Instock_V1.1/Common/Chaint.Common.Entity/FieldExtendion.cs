using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using Chaint.Common.Core;
namespace Chaint.Common.Entity.Utils
{
    public static class FieldExtendion
    {
        public static void Bind(this LookUpEdit field,DataSource dataSource)
        {
            field.Properties.Columns.Clear();
            field.Properties.Columns.Add(new LookUpColumnInfo(dataSource.DisplayMember, dataSource.DisplayTitle));
            field.Properties.DisplayMember = dataSource.DisplayMember;
            field.Properties.ValueMember = dataSource.DisplayTitle;
            field.Properties.DataSource = dataSource.Data;
            field.ItemIndex = 0;
        }
        public static void Bind(this SearchLookUpEdit field, DataSource dataSource)
        {
            field.Properties.DisplayMember = dataSource.DisplayMember;
            field.Properties.ValueMember = dataSource.ValueMember;
            field.Properties.DataSource =dataSource.Data;
        }
        public static void Bind(this RepositoryItemSearchLookUpEdit field, DataSource dataSource)
        {
            field.DisplayMember = dataSource.DisplayMember;
            field.ValueMember = dataSource.ValueMember;
            field.DataSource = dataSource.Data;
        }
        public static void Bind(this ComboBoxEdit field, DataSource dataSource)
        {
            field.Properties.Items.Clear();
            foreach (DataRow row in dataSource.Data.Rows)
            {
                object displayMember = row[dataSource.DisplayMember];
                field.Properties.Items.Add(displayMember);
            }
        }
        public static void Bind(this RepositoryItemComboBox field, DataSource dataSource)
        {
            field.Items.Clear();
            foreach (DataRow row in dataSource.Data.Rows)
            {
                object displayMember = row[dataSource.DisplayMember];
                field.Items.Add(displayMember);
            }
        }
        public static void Bind(this CheckedComboBoxEdit field, DataSource dataSource)
        {
            field.Properties.DisplayMember = dataSource.DisplayMember;
            field.Properties.ValueMember = dataSource.ValueMember;
            field.Properties.DataSource = dataSource.Data;
            field.EditValue = string.Empty;
            field.RefreshEditValue();
        }
    }
}
