using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Photon.Pun;

public class Utils : MonoBehaviour
{
    public enum ObjKind
    {
        Wall,
        Monster,
        Item
    }



    private static Utils util;
    public static Utils Util => util;
    private PhotonView photonView;
    private GameObject go;
    public int ObjName = 0;

    void Start()
    {
        if (!util)
        {
            util = this;
        }
        DontDestroyOnLoad(gameObject);
        photonView = GetComponent<PhotonView>();
    }

    public IEnumerator CoSeconds(Action act, float seconds)
    {
        IEnumerator coRoutine = CoRoutineTimer(seconds, act);
        StartCoroutine(coRoutine);
        return coRoutine;
    }

    public void CoStopRoutine(IEnumerator co)
    {
        StopCoroutine(co);
    }

    private IEnumerator CoRoutineTimer(float seconds, Action act)
    {
        yield return new WaitForSeconds(seconds);
        act.Invoke();
    }

    public Vector2 GetRandomPosInMap()
    {
        float randomx = Random.Range(-5f, 5f);
        float randomy = Random.Range(-4f, 4f);

        return new Vector2(randomx, randomy);
    }

    public int GetRandomPosInInt(int min, int max)
    {
        return Random.Range(min, max);
    }

    public GameObject InstanceGameObject(GameObject obj, Vector3 pos, ObjKind kind = ObjKind.Wall)
    {
        go = Instantiate(obj, pos, Quaternion.identity); ;
        string objName;
        switch (kind)
        {
            case ObjKind.Wall:
                objName = "Wall" + ObjName;
                break;
            case ObjKind.Monster:
                objName = "Monster" + ObjName;
                break;
            case ObjKind.Item:
                objName = "Item" + ObjName;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }

        go.name = objName;
        ObjName++;
        return go;
    }

    [PunRPC]
    private void Instance(Vector3 pos)
    {
        Instantiate(go, pos, Quaternion.identity);
    }

    public void DeleteGameObj(GameObject obj)
    {
        if (photonView)
        {
            photonView.RPC("BreakWallTile", RpcTarget.AllBuffered, obj.name);
        }
    }
    [PunRPC]
    private void DeleteObjInRPC(string objName)
    {
        GameObject objFind = GameObject.Find(objName);

        if (objFind)
        {
            Debug.Log("Successfully find go");
            Destroy(objFind);
        }
        else
        {
            Debug.Log("cant find go");
        }
    }

    [PunRPC]
    private void BreakWallTile(string objName)
    {
        GameObject objFind = GameObject.Find(objName);

        if (objFind)
        {
            WallBehavior wallInfo = objFind.GetComponent<WallBehavior>();
            wallInfo.TryBreakWall();
        }
    }


}
