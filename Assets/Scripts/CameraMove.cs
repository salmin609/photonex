using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject[] players;
    private GameObject player;
    private Transform target;
    private float smoothSpeed;
    [SerializeField]
    private Vector3 offset;

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

        target = player.transform;
        smoothSpeed = 0.125f;
        offset = new Vector3(0f, 0f, -10f);

        Vector3 desiredPos = target.position + offset;
        transform.position = desiredPos;
        //transform.LookAt(target);
    }

    void FixedUpdate()
    {
        //Vector3 playerTrans = player.transform.position;
        //transform.position = new Vector3(playerTrans.x, playerTrans.y, -10f);
        if (target)
        {
            Vector3 desiredPos = target.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
        }
        //transform.LookAt(target);
    }
}
