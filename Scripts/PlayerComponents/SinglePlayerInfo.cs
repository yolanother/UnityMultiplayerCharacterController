using System.Collections;
using System.Collections.Generic;
using DoubTech.MCC;
using UnityEngine;

public class SinglePlayerInfo : MonoBehaviour, IPlayerInfoProvider
{
    [SerializeField] private string playerName;

    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }

    public uint PlayerId => 0;
    public bool IsLocalPlayer => true;
    public bool IsServer => true;
}
