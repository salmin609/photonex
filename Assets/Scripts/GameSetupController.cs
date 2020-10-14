using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    private GameObject checkObj;
    [SerializeField] private GameObject check;
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player..");
        PhotonNetwork.Instantiate("PhotonPlayer", new Vector3(1f, 0f, 1f), Quaternion.identity);
        checkObj = PhotonNetwork.Instantiate("MapManager", new Vector3(0f, 0f), Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            checkObj.GetComponent<MapManager>().Init();
        }
    }
}
