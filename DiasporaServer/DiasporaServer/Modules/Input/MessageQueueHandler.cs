using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiasporaServer.Modules.Input
{
    class MessageQueueHandler
    {
        private ServerListener _serverListener;

        public void Run()
        {
            _serverListener = new ServerListener();
            var server = new NetServer(_serverListener, 1000, "TestApp");
            if (!server.Start(9050))
            {
                Console.WriteLine("Server start failed");
                Console.ReadKey();
                return;
            }
            _serverListener.Server = server;

            while (!Console.KeyAvailable)
            {
                server.PollEvents();
                Thread.Sleep(15);
            }
            server.Stop();
            Console.ReadKey();
        }
    }
}
