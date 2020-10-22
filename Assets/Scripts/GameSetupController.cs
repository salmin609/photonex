using System.Collections;
using Photon.Pun;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetupController : MonoBehaviour
{
    private GameObject mapManager;
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player..");
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(1f, 1f), Quaternion.identity);
        mapManager = PhotonNetwork.Instantiate("MapManager", new Vector3(0f, 0f), Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            player.transform.position = new Vector3(1.2f, 1.2f);
            mapManager.GetComponent<MapManager>().Init();
        }
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while(PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        SceneManager.LoadScene("SampleScene");
    }
}
