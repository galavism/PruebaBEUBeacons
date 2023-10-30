using System;
using System.Collections.Generic;
using System.Text;

namespace personalization.InterfaceBeacons
{
    public interface iOSScan
    {
        void InitializeScannerService();
        void startranging();
        void stopranging();
    }
}
