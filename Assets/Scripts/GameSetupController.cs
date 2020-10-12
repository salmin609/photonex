using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    private GameObject checkObj;
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player..");
        PhotonNetwork.Instantiate("Player", new Vector3(1f, 1f), Quaternion.identity);
        checkObj = PhotonNetwork.Instantiate("MapManager", new Vector3(0f, 0f), Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            checkObj.GetComponent<MapManager>().Init();
            //checkObj = PhotonNetwork.Instantiate("Check", new Vector3(6f, 0f), Quaternion.identity);
            if (checkObj)
            {
                Debug.Log("ckck1");
            }
            else
            {
                Debug.Log("ckck2");
            }
        }

        PhotonNetwork.Instantiate("Camera", Vector3.zero, Quaternion.identity);


    }

    void Update()
    {
        if (checkObj)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PhotonNetwork.Destroy(checkObj);
            }
        }
    }
}
