using Diaspora.Transport;
using FlatBuffers;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasporaServer.Modules.InterestManagement
{
    internal class InterestNode
    {
        // Fields
        public readonly string NodeID;
        public List<NetPeer> Subscribers;

        // Methods
        public InterestNode(string NID)
        {
            this.NodeID = NID;
        }

        private byte[] buildMessage(string message)
        {
            FlatBufferBuilder builder = new FlatBufferBuilder(0x400);
            StringOffset nameOffset = builder.CreateString("[SYSTEM]");
            StringOffset messageOffset = builder.CreateString(message);
            ChatMessage.StartChatMessage(builder);
            ChatMessage.AddName(builder, nameOffset);
            ChatMessage.AddMessage(builder, messageOffset);
            Offset<ChatMessage> offset3 = ChatMessage.EndChatMessage(builder);
            builder.Finish(offset3.Value);
            return builder.SizedByteArray();
        }

        public void SendToSubscribers(NetDataWriter writer, SendOptions options)
        {
            List<NetPeer> subscribers = this.Subscribers;
            lock (subscribers)
            {
                for (int i = 0; i < this.Subscribers.Count; i++)
                {
                    this.Subscribers[i].Send(writer, options);
                }
            }
        }

        public void SendToSubscribers(byte[] data, SendOptions options)
        {
            List<NetPeer> subscribers = this.Subscribers;
            lock (subscribers)
            {
                for (int i = 0; i < this.Subscribers.Count; i++)
                {
                    this.Subscribers[i].Send(data, options);
                }
            }
        }

        public void SendToSubscribers(NetDataWriter writer, SendOptions options, NetPeer exclude)
        {
            List<NetPeer> subscribers = this.Subscribers;
            lock (subscribers)
            {
                for (int i = 0; i < this.Subscribers.Count; i++)
                {
                    if (this.Subscribers[i] != exclude)
                    {
                        this.Subscribers[i].Send(writer, options);
                    }
                }
            }
        }

        public void SendToSubscribers(byte[] data, SendOptions options, NetPeer exclude)
        {
            List<NetPeer> subscribers = this.Subscribers;
            lock (subscribers)
            {
                for (int i = 0; i < this.Subscribers.Count; i++)
                {
                    if (this.Subscribers[i] != exclude)
                    {
                        this.Subscribers[i].Send(data, options);
                    }
                }
            }
        }

        public void SendToSubscribers(byte[] data, int start, int length, SendOptions options)
        {
            List<NetPeer> subscribers = this.Subscribers;
            lock (subscribers)
            {
                for (int i = 0; i < this.Subscribers.Count; i++)
                {
                    this.Subscribers[i].Send(data, start, length, options);
                }
            }
        }

        public void SendToSubscribers(byte[] data, int start, int length, SendOptions options, NetPeer exclude)
        {
            List<NetPeer> subscribers = this.Subscribers;
            lock (subscribers)
            {
                for (int i = 0; i < this.Subscribers.Count; i++)
                {
                    if (this.Subscribers[i] != exclude)
                    {
                        this.Subscribers[i].Send(data, start, length, options);
                    }
                }
            }
        }

        public void Subscribe(NetPeer Subscriber)
        {
            if (this.Subscribers == null)
            {
                this.Subscribers = new List<NetPeer>();
            }
            List<NetPeer> subscribers = this.Subscribers;
            lock (subscribers)
            {
                this.Subscribers.Add(Subscriber);
            }
            this.SendToSubscribers(this.buildMessage($"Peer connected {Subscriber.EndPoint}"), SendOptions.ReliableUnordered);
        }

        public void UnSubscribe(NetPeer peer)
        {
            List<NetPeer> subscribers = this.Subscribers;
            lock (subscribers)
            {
                if (this.Subscribers.Contains(peer))
                {
                    this.Subscribers.Remove(peer);
                    this.SendToSubscribers(this.buildMessage($"Peer disconnected {peer.EndPoint}"), SendOptions.ReliableUnordered);
                }
            }
        }
    }


}
