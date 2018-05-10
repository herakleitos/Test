
namespace Chaint.Common.Core.EventArgs
{
    public  class BaseEventArgs
    {
        public string Parent { get; set; }
        public string Sender { get; set; }
        public bool Cancel { get; set; }
    }
}
