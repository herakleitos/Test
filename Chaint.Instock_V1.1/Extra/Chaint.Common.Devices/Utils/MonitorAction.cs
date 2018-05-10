using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Chaint.Common.Devices.Utils
{

    /*-----------------------------------------------------------------------------------
     * 作者: Automation&IT Dept. 
     * 
     * 创建时间: 2016-04-28
     * 使用平台：NetFramework 3.5以上
     * 功能描述: 
     *          MonitorAction test = new MonitorAction(this);
                Action<object> sndAction = new Action<object>(SendRequest_Action);
                Action<object> revAction = new Action<object>(RevAck_Action);
                test.SendRequest(sndAction, "abc",  revAction, "124");

                test = null;
     * 
     * 采用回调的方式进行处理
     * 
     * 工作场景：
     *   (1) 向第三方发送数据(一般以中间数据表为基础)，然后实时等待第三方反馈的结果；
     *   (2) 根据反馈的结果然后向下进行处理；
     *   
     *   (3) 如玖龙在称重位置向ZMESTF02发送重量等信息，然后实时等待去向RecPort和IFSNO编号时；
     *   (4) 如CPCC向CPCS发送决策请求，然后实时等待反馈的结果信息时；
     * 
     ------------------------------------------------------------------------------------*/

    public class MonitorAction : IDisposable
    {
        private Action<object> m_RevHandler = null;
        private Control m_Control = null;


        public MonitorAction(Control owner)
        {
            m_Control = owner;
        }

        public bool SendRequest(Action<object> sendHandler,object objSndArg,Action<object> revHandler,object objRevArg)
        {
            m_RevHandler = revHandler;
            if (sendHandler != null)
            {
                sendHandler.BeginInvoke(objSndArg, new AsyncCallback(StartMonitor), objRevArg);
                return true;
            }
            else
                return false;
        }


        private void StartMonitor(IAsyncResult result)
        {
            System.Runtime.Remoting.Messaging.AsyncResult res = (System.Runtime.Remoting.Messaging.AsyncResult)result;
            Action<object> mydelegate = (Action<object>)res.AsyncDelegate;
            if(m_RevHandler!=null)
            {
                m_RevHandler.BeginInvoke(res.AsyncState, null, null);
            }
        }

        public void Dispose()
        {

        }

    }
}
