#if REACTOR
using System.Collections.Generic;
using KS.Reactor;
using KS.Reactor.Client.Unity;
using UnityEngine;

namespace DoubTech.MCC.Integrations.Reactor.Connections
{
    public class MCCServerConnect : MonoBehaviour
    {
        // Public property used to enable connections to local or live servers.
        public bool UseLocalServer = true;

        // Current room.
        private ksRoom m_room;

        // Run when the game starts.
        void Start()
        {
            // If we are using a local server, then fetch room information from the ksRoomType component. Otherwise use the
            // ksReactor service to request a list of live servers.
            if (UseLocalServer)
            {
                ConnectToServer(GetComponent<ksRoomType>().GetRoomInfo());
            }
            else
            {
                ksReactor.GetServers(OnGetServers);
            }
        }

        // Handle the response to a ksReactor.GetServers() request.
        private void OnGetServers(List<ksRoomInfo> roomList, string error)
        {
            // Write errors to the logs.
            if (error != null)
            {
                ksLog.Error("Error fetching servers. " + error);
                return;
            }

            // Connect to the first room in the list of rooms returned.
            if (roomList != null && roomList.Count > 0)
            {
                ConnectToServer(roomList[0]);
            }
            else
            {
                ksLog.Warning("No servers found.");
            }
        }

        // Connect to a server whose location is described in a ksRoomInfo object.
        private void ConnectToServer(ksRoomInfo roomInfo)
        {
            m_room = new ksRoom(roomInfo);

            // Register an event handler that will be called when a connection attempt completes.
            m_room.OnConnect += HandleConnect;

            // Register an event handler that will be called when a connected room disconnects.
            m_room.OnDisconnect += HandleDisconnect;

            m_room.Connect();
        }

        // Handle a ksRoom connect event.
        private void HandleConnect(ksRoom.ConnectError status, uint customStatus)
        {
            if (status == ksRoom.ConnectError.CONNECTED)
            {
                ksLog.Info("Connected to " + m_room);
            }
            else
            {
                ksLog.Error(this, "Unable to connect to " + m_room + " (" + status + ", " + customStatus + ")");
            }
        }

        // Handle a ksRoom disconnect event.
        private void HandleDisconnect(ksRoom.DisconnectError status)
        {
            ksLog.Info("Disconnected from " + m_room + " (" + status + ")");
            m_room.CleanUp();
            m_room = null;
        }
    }
}
#endif
