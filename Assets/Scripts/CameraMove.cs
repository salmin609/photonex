using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject[] players;
    private GameObject player;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in players)
        {
            if (obj.GetComponent<PhotonView>().IsMine)
            {
                player = obj;
            }
        }
    }

    void Update()
    {
        Vector3 playerTrans = player.transform.position;
        transform.position = new Vector3(playerTrans.x, playerTrans.y, -10f);
    }
}
