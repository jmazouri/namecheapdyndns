using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NamecheapDynDnsUpdater
{
    //MICROSOFT!!! argh.
    public class ClipboardHelper
    {
        public static string GetClipboardText()
        {
            string cpdata = null;
            Exception threadEx = null;

            Thread staThread = new Thread(
                delegate()
                {
                    try
                    {
                        cpdata = Clipboard.GetText();
                    }

                    catch (Exception ex)
                    {
                        threadEx = ex;
                    }
                });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();

            return cpdata.Trim();
        }
    }
}
