using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private PhotonView photonView;
    private GameObject[] players;
    private GameObject player;
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject obj in players)
            {
                if (obj.GetComponent<PhotonView>().IsMine)
                {
                    player = obj;
                }
            }

            Vector3 playerTrans = player.transform.position;
            transform.position = new Vector3(playerTrans.x, playerTrans.y, -10f);
        }
    }
}
