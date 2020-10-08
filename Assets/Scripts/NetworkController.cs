using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    
    void Start()
    {
        Screen.SetResolution(1280, 860, false);
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We are connected to " + PhotonNetwork.CloudRegion + " Server!");
    }
}
