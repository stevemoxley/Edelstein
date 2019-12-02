using System;
using System.Threading.Tasks;
using DotNetty.Common.Utilities;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public abstract class AbstractSocketAdapter : ISocketAdapter
    {
        public static readonly AttributeKey<ISocketAdapter> Key = AttributeKey<ISocketAdapter>.ValueOf("Adapter");
        public ISocket Socket { get; }

        protected AbstractSocketAdapter(ISocket socket)
            => Socket = socket;

        public abstract Task OnPacket(IPacket packet);
        public abstract Task OnException(Exception exception);
        public abstract Task OnUpdate();
        public abstract Task OnDisconnect();
    }
}