#if MIRROR
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace DoubTech.Multiplayer
{
    public interface ISpawner
    {
        void Spawn(NetworkConnection conn, bool isClientAuthority);
    }
}
#endif