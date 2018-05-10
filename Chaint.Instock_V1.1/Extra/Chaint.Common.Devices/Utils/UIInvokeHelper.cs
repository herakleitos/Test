using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Chaint.Common.Devices.Utils
{
    public static class UIInvokeHelper
    {
        /// <summary>
        /// Runs code in UI thread synchronously with BeginInvoke.
        /// </summary>
        /// <param name="code">the code, like "delegate { this.Text = "new text"; }"
        /// </param>
        public static void UIThreadBeginInvoke(Control control, MethodInvoker code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);
                return;
            }
            code.Invoke();
        }

        /// <summary>
        /// Runs code in UI thread synchronously with Invoke.
        /// </summary>
        /// <param name="code">the code, like "delegate { this.Text = "new text"; }"
        /// </param>
        public static void UIThreadInvoke(Control control, MethodInvoker code)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(code);
                return;
            }
            code.Invoke();
        }
    }
}
