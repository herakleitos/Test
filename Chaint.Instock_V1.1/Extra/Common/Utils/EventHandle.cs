using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTWH.Common
{


    public delegate void MenuClickEventHandle(object sender, MenuClickEventArgs args);
    public class MenuClickEventArgs : EventArgs
    {

        private string menuName;
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }

        }

    }

   
}
