using System;
using System.Collections.Generic;
using System.Text;

namespace personalization.InterfaceBeacons
{
    public interface iOSTransmit
    {
        void InitializeService();

        void StartBroadcasting();
    }
}
