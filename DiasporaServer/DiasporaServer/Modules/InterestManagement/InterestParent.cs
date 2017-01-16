using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasporaServer.Modules.InterestManagement
{
    internal class InterestParent
    {
        // Fields
        public List<InterestNode> Nodes = new List<InterestNode>();

        // Methods
        public int GetNode(string NID)
        {
            InterestNode item = Nodes.SingleOrDefault(n => n.NodeID == NID);
            return ((item != null) ? this.Nodes.IndexOf(item) : -1);
        }

        public bool JoinNode(string NID, NetPeer peer)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                InterestNode node = nodes.SingleOrDefault(n => n.NodeID == NID);
                if (node == null)
                {
                    InterestNode item = new InterestNode(NID);
                    this.Nodes.Add(item);
                    item.Subscribe(peer);
                    return false;
                }
                node.Subscribe(peer);
                return true;
            }
        }

        public void LeaveNode(string NID, NetPeer peer)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                InterestNode node = nodes.SingleOrDefault(n => n.NodeID == NID);
                if (node != null)
                {
                    node.UnSubscribe(peer);
                }
            }
        }

        public void RemovePeer(NetPeer peer)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    this.Nodes[i].UnSubscribe(peer);
                }
                this.Nodes.RemoveAll(n => n.Subscribers.Count == 0);
            }
        }

        public void SendToSubscribers(NetDataWriter writer, SendOptions options)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    this.Nodes[i].SendToSubscribers(writer, options);
                }
            }
        }

        public void SendToSubscribers(byte[] data, SendOptions options)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    this.Nodes[i].SendToSubscribers(data, options);
                }
            }
        }

        public void SendToSubscribers(NetDataWriter writer, SendOptions options, NetPeer exclude)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    this.Nodes[i].SendToSubscribers(writer, options);
                }
            }
        }

        public void SendToSubscribers(byte[] data, SendOptions options, NetPeer exclude)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    this.Nodes[i].SendToSubscribers(data, options);
                }
            }
        }

        public void SendToSubscribers(int Node, NetDataWriter writer, SendOptions options)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                this.Nodes[Node].SendToSubscribers(writer, options);
            }
        }

        public void SendToSubscribers(int Node, byte[] data, SendOptions options)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                this.Nodes[Node].SendToSubscribers(data, options);
            }
        }

        public void SendToSubscribers(int Node, NetDataWriter writer, SendOptions options, NetPeer peer)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                this.Nodes[Node].SendToSubscribers(writer, options, peer);
            }
        }

        public void SendToSubscribers(int Node, byte[] data, SendOptions options, NetPeer peer)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                this.Nodes[Node].SendToSubscribers(data, options, peer);
            }
        }

        public void SendToSubscribers(byte[] data, int start, int length, SendOptions options)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    this.Nodes[i].SendToSubscribers(data, start, length, options);
                }
            }
        }

        public void SendToSubscribers(byte[] data, int start, int length, SendOptions options, NetPeer exclude)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    this.Nodes[i].SendToSubscribers(data, start, length, options);
                }
            }
        }

        public void SendToSubscribers(int Node, byte[] data, int start, int length, SendOptions options)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                this.Nodes[Node].SendToSubscribers(data, start, length, options);
            }
        }

        public void SendToSubscribers(int Node, byte[] data, int start, int length, SendOptions options, NetPeer peer)
        {
            List<InterestNode> nodes = this.Nodes;
            lock (nodes)
            {
                this.Nodes[Node].SendToSubscribers(data, start, length, options, peer);
            }
        }


    }


}
