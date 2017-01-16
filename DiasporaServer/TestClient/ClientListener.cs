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
        private string RegionId = "System";
        private string RoomId = "Global";


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
            NetDataWriter writer = new NetDataWriter();
            Console.WriteLine("Enter your name");
            this.chatName = Console.ReadLine();
            Console.WriteLine("Welcome " + this.chatName + " \nEnter q to quit");
            Console.WriteLine("To change rooms type /r roomName, leave roomName blank to rejoin global");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    string message = Console.ReadLine();
                    if ((message != "") && (message != "q"))
                    {
                        char[] separator = new char[] { ' ' };
                        string[] strArray = message.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        if (strArray[0].Contains("/r"))
                        {
                            this.Client.Peer.Send(this.buildMove((strArray.Length > 1) ? strArray[1] : "Global"), SendOptions.ReliableUnordered);
                        }
                        else
                        {
                            this.Client.Peer.Send(this.buildMessage(message), SendOptions.Unreliable);
                        }
                    }
                    else if (message == "q")
                    {
                        this.Client.Disconnect();
                        return;
                    }
                }
                else
                {
                    this.Client.PollEvents();
                }
            }

        }

        private byte[] buildMessage(string message)
        {
            FlatBufferBuilder builder = new FlatBufferBuilder(1024);
            StringOffset nameOffset = builder.CreateString($"[{this.chatName}]: ");
            StringOffset messageOffset = builder.CreateString(message);
            StringOffset interestIDOffset = builder.CreateString(this.RoomId);
            StringOffset regionIDOffset = builder.CreateString(this.RegionId);
            Offset<Header> mHeaderOffset = Header.CreateHeader(builder, interestIDOffset, regionIDOffset, MessageType.Chat);
            ChatMessage.StartChatMessage(builder);
            ChatMessage.AddMHeader(builder, mHeaderOffset);
            ChatMessage.AddName(builder, nameOffset);
            ChatMessage.AddMessage(builder, messageOffset);
            var msg = ChatMessage.EndChatMessage(builder);
            builder.Finish(msg.Value);
            return builder.SizedByteArray();
        }


        private byte[] buildMove(string NewRoom)
        {
            this.RoomId = NewRoom;
            FlatBufferBuilder builder = new FlatBufferBuilder(1024);
            StringOffset interestIDOffset = builder.CreateString(this.RoomId);
            StringOffset regionIDOffset = builder.CreateString(this.RegionId);
            Offset<Header> mHeaderOffset = Header.CreateHeader(builder, interestIDOffset, regionIDOffset, MessageType.Move);
            ChatMessage.StartChatMessage(builder);
            ChatMessage.AddMHeader(builder, mHeaderOffset);
            Offset<ChatMessage> offset4 = ChatMessage.EndChatMessage(builder);
            builder.Finish(offset4.Value);
            return builder.SizedByteArray();
        }

    }
}
