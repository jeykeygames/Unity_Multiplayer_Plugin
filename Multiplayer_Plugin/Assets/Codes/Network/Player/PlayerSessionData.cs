using System;
using Unity.Collections;
using Unity.Netcode;

namespace Codes.Network.Player
{
    public struct PlayerSessionData : INetworkSerializable, IEquatable<PlayerSessionData>
    {
        public ulong clientId;
        public FixedString32Bytes displayName; 
        public bool isReady;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref displayName);
            serializer.SerializeValue(ref isReady);
        }

        public bool Equals(PlayerSessionData other)
        {
            return other.clientId == clientId && other.displayName.Equals(displayName);
        }
    }
}