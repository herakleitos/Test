namespace Chaint.Common.Interface.View
{
    public interface IBillView
    {
        void AddControl(string name, object obj);
        T GetControl<T>(string name);
        T GetValue<T>(string name);
        void SetValue(string name, object value);
    }
}
