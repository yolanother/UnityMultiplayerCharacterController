#if REACTOR
using DoubTech.MCC.Input;
using KS.Reactor;
using KS.Reactor.Client.Unity;
using UnityEngine;

namespace DoubTech.MCC.Integrations.Reactor
{
    public class ReactorPlayer : MonoBehaviour, IPlayerInfoProvider, ICharacterController
    {
        [ksEditable] private uint propertyStart = 2000;
        [ksEditable] private uint rpcStart = 2000;
        private ksEntityScript entity;
        private string playerName;
        private Player player;

        public const uint RPC_POSITION = 2001;
        public const uint RPC_ROTATION = 2002;
        public const uint RPC_VELOCITY = 2003;
        public const uint RPC_MOVE = 2004;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        public ksEntityScript Entity
        {
            get => entity;
            set
            {
                entity = value;
                if (!string.IsNullOrEmpty(playerName))
                {
                    PlayerName = playerName;
                }

                if (IsLocalPlayer)
                {
                    player.OnStartLocalPlayer();
                }
                else
                {
                    player.OnStartRemotePlayer();
                }
            }
        }

        public string PlayerName
        {
            get
            {
                if (entity)
                {
                    return entity.Properties[propertyStart + 1].String;
                }

                return "";
            }
            set
            {
                playerName = value;
                if (entity)
                {
                    entity.Properties[propertyStart + 1].String = value;
                }
            }
        }

        public uint PlayerId => entity.Entity.Id;
        public bool IsLocalPlayer => entity.Entity.PlayerController != null;
        public bool IsServer => IsLocalPlayer;

        public Vector3 Position
        {
            get => entity.Entity.Position;
            set
            {
                entity.Room.CallRPC(RPC_POSITION, value);
            }
        }

        public Quaternion Rotation
        {
            get => entity.Entity.Rotation;
            set
            {
                entity.Room.CallRPC(RPC_ROTATION, value);
            } 
        }

        public Vector3 Velocity
        {
            get => entity.Entity.Velocity;
            set
            {
                entity.Room.CallRPC(RPC_VELOCITY, value);
            }
        }

        public void Move(Vector3 target)
        {
            entity.Room.CallRPC(RPC_MOVE, target);
        }
    }
}
#endif
