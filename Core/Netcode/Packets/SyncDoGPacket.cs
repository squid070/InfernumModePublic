using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.DoG.DoGPhase1HeadBehaviorOverride;

namespace InfernumMode.Core.Netcode.Packets
{
    public class SyncDoGPacket : BaseInfernumPacket
    {
        public override bool ResendFromServer => false;
        public override void Write(ModPacket packet, params object[] context)
        {
            if(Main.netMode == NetmodeID.Server)
            {
                ModContent.GetInstance<InfernumMode>().Logger.Debug("Server tried to write packet with context length "+context.Length.ToString());
            }
            packet.Write((int)context[0]);
            packet.Write((double)context[1]);
        }

        public override void Read(BinaryReader reader)
        {
            int n = reader.ReadInt32();
            double damage = reader.ReadDouble();
            if (Main.netMode == NetmodeID.Server)
            {
                ModContent.GetInstance<InfernumMode>().Logger.Debug("Server received packet with context NPC id "+n.ToString() +" damage: "+damage.ToString());
            }
            else
            {
                ModContent.GetInstance<InfernumMode>().Logger.Debug("Client received packet with context NPC id " + n.ToString() + " damage: " + damage.ToString());
            }
            UpdateDoGPhaseServer(n, damage);
        }
    }
}