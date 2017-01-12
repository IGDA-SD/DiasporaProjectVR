using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;
using FlatBuffers;
using Diaspora.Transport;

namespace TestClient
{
    class ClientListener : INetEventListener
    {
        public NetClient Client;
        private string chatName;
        public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
        {
            Console.WriteLine("[Client] error! " + socketErrorCode);
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            
        }

        public void OnNetworkReceive(NetPeer peer, NetDataReader reader)
        {
            //Console.WriteLine("Data recieved");
            //Console.WriteLine(reader.GetString(100));
            var buf = new ByteBuffer(reader.GetBytes());
            var msg = ChatMessage.GetRootAsChatMessage(buf);
            Console.WriteLine(string.Format("-->{0}{1}",msg.Name,msg.Message));
        }

        public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
        {
            //Client.Connect(remoteEndPoint.Host, 9050);
            Console.WriteLine("recieved unconnected");
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine("[Client] connected to: {0}:{1}", peer.EndPoint.Host, peer.EndPoint.Port);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectReason disconnectReason, int socketErrorCode)
        {
            Console.WriteLine("[Client] disconnected: " + disconnectReason);
        }

        public void Run()
        {
            //Client.SendUnconnectedMessage(new byte[] { 1 }, new NetEndPoint("50.113.84.153", 9050));
            NetDataWriter dataWriter = new NetDataWriter();
            string input;
            Console.WriteLine("Enter your name");
            chatName = Console.ReadLine();
            Console.WriteLine("Welcome " + chatName + " \nEnter q to quit");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    input = Console.ReadLine();
                    if (input != "" && input != "q")
                    {
                        //dataWriter.Reset();
                        //dataWriter.Data. = ;
                        Client.Peer.Send(buildMessage(input), SendOptions.Unreliable);
                    }
                    else if(input == "q")
                    {
                        Client.Disconnect();
                        break;
                    }
                }
                else
                {
                    Client.PollEvents();
                }
            }
        }

        private byte[] buildMessage(string message)
        {
            var builder = new FlatBufferBuilder(1024);
            var cName = builder.CreateString(string.Format("[{0}]: ", chatName));
            var m = builder.CreateString(message);
            ChatMessage.StartChatMessage(builder);
            ChatMessage.AddName(builder, cName);
            ChatMessage.AddMessage(builder, m);
            var msg = ChatMessage.EndChatMessage(builder);
            builder.Finish(msg.Value);
            return builder.SizedByteArray();
        }
    }
}
