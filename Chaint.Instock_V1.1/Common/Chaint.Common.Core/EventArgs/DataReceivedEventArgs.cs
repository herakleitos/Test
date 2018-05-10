using System.IO.Ports;
namespace Chaint.Common.Core.EventArgs
{
    public class DataReceivedEventArgs:BaseEventArgs
    {
        public SerialData EventType { get; set; }
    }
}
