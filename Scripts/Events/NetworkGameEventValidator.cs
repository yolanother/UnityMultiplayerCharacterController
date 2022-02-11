using System;
using DoubTech.ScriptableEvents;
using UnityEngine;

namespace DoubTech.MCC.Events
{
    public class NetworkGameEventValidator : MonoBehaviour, ISimpleGameEventValidator
    {
        [SerializeField] private bool localPlayer;
        
        private IPlayerInfoProvider providerInfo;

        private void Awake()
        {
            providerInfo = GetComponentInParent<IPlayerInfoProvider>();
        }

        public bool OnValidateEvent()
        {
            if (null == providerInfo) return false;
            
            return localPlayer && providerInfo.IsLocalPlayer
                || !localPlayer && !providerInfo.IsLocalPlayer;
        }
    }
}
