using DiasporaServer.Modules.Input;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasporaServer
{
    class Program
    {
        static void Main(string[] args)
        {
            NtpSyncModule ntpSync = new NtpSyncModule("pool.ntp.org");
            ntpSync.GetNetworkTime();
            if (ntpSync.SyncedTime.HasValue)
            {
                Console.WriteLine("Synced time test: " + ntpSync.SyncedTime.Value);
            }

            MessageQueueHandler mHandler = new MessageQueueHandler();
            mHandler.Run();
        }
    }
}
