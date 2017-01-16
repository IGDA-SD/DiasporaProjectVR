using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientListener cList = new ClientListener();
            cList.Client = new LiteNetLib.NetClient(cList, "TestApp");
            cList.Client.Start();
            cList.Client.Connect("52.160.111.116", 9050);
            cList.Run();
            Console.ReadKey();
            cList.Client.Stop();
        }
    }
}
