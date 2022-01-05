using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;

namespace usart
{
    class COMPortEvent
    {
        public delegate void EventDataReceived(object sender, EventArgs e);

        /// <summary>
        /// event handle
        /// </summary>
        public event EventDataReceived DataReceived;
       protected virtual void onReceived()
        {
            object obj = new object();
            DataReceived?.Invoke(obj, new EventArgs());
        }
    }
}
