using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class JumpMoveFragment : ActionMoveFragment
    {
        private Point _vPosition;

        public JumpMoveFragment(MoveFragmentAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void DecodeData(IPacket packet)
        {
            _vPosition = packet.Decode<Point>();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<Point>(_vPosition);

            base.EncodeData(packet);
        }
    }
}