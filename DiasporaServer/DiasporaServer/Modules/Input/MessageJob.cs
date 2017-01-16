using Diaspora.Transport;
using DiasporaServer.Modules.InterestManagement;
using FlatBuffers;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasporaServer.Modules.Input
{
    internal class MessageJob
    {
        // Methods
        public MessageJob(byte[] Data, NetPeer peer)
        {
            ByteBuffer buffer = new ByteBuffer(Data);
            ChatMessage rootAsChatMessage = ChatMessage.GetRootAsChatMessage(buffer);
            if (rootAsChatMessage.MHeader.HasValue)
            {
                Header header = rootAsChatMessage.MHeader.Value;
                InterestParent parent = InterestManager.Instance.Regions[header.RegionID];
                switch (header.MType)
                {
                    case MessageType.Chat:
                        parent.SendToSubscribers(parent.GetNode(header.InterestID), Data, SendOptions.Unreliable, peer);
                        break;

                    case MessageType.Move:
                        InterestManager.Instance.RemovePeer(peer);
                        parent.JoinNode(header.InterestID, peer);
                        break;
                }
            }
        }
    }



}
