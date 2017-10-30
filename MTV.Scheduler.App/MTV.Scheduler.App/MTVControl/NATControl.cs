using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NATUPNPLib;
using System.IO;
using MTV.Scheduler.App.UI;

namespace MTV.Scheduler.App.MTVControl
{
    public static class NATControl
    {
        public static UPnPNAT NAT = new UPnPNAT();
        private static IStaticPortMappingCollection _mappings;

        public static IStaticPortMappingCollection Mappings
        {
            get
            {
                if (_mappings == null)
                {
                    //looking at NATEventManager seems to populate the collection... :S
                    try
                    {
                        if (NAT.NATEventManager != null)
                            _mappings = NAT.StaticPortMappingCollection;
                    }
                    catch
                    {
                    }
                }

                return _mappings;
            }
        }

        public static bool SetPorts(int wanPort, int lanPort)
        {
            bool b = false;
            int i = 3;
            while (Mappings == null && i > 0)
            {
                Thread.Sleep(2000);
                i--;
            }

            if (Mappings != null)
            {
                try
                {
                    Mappings.Remove(wanPort, "TCP");
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }

               
                
                try
                {
                    Mappings.Add(wanPort, "TCP", lanPort, MainForm.AddressIPv4, true, "MTVSchedulerAPP");
                    b = true;
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
            }
            
            return b;
        }
    }
}
