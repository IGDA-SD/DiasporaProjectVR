using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasporaServer.Modules.InterestManagement
{
    internal class InterestManager
    {
        // Fields
        private static InterestManager _inst;
        public Dictionary<string, InterestParent> Regions = new Dictionary<string, InterestParent>();

        // Methods
        public InterestManager()
        {
            this.Regions.Add("System", new InterestParent());
            this.Regions["System"].Nodes.Add(new InterestNode("Global"));
        }

        public void AddRegion(string Region, string DefaultNode)
        {
            Dictionary<string, InterestParent> regions = this.Regions;
            lock (regions)
            {
                InterestParent parent = new InterestParent();
                parent.Nodes.Add(new InterestNode(DefaultNode));
                this.Regions.Add(Region, parent);
            }
        }

        public void RemovePeer(NetPeer peer)
        {
            foreach (KeyValuePair<string, InterestParent> pair in this.Regions)
            {
                pair.Value.RemovePeer(peer);
            }
        }

        // Properties
        public static InterestManager Instance =>
            (_inst ?? (_inst = new InterestManager()));
    }


}
