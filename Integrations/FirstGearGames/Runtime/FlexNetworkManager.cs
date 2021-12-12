#if FIRST_GEAR_GAMES
using System;
using FirstGearGames.FlexSceneManager;
using Mirror;

namespace DoubTech.Multiplayer
{
    public class FlexNetworkManager : NetworkManager
    {
        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);
            FlexSceneManager.OnServerConnect(conn);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            FlexSceneManager.OnServerDisconnect(conn);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            FlexSceneManager.ResetInitialLoad();
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);
            FlexSceneManager.OnServerAddPlayer(conn);
        }
    }
}
#endif
