using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;
    class GridInfo
    {
        public GridInfo(float xpos, float ypos, int index, bool iswall, bool isEnd = false)
        {
            x = xpos;
            y = ypos;
            isWall = iswall;
            this.index = index;
            this.isEnd = isEnd;
            isChecked = false;
        }

        public float x, y;
        public bool isWall;
        public bool isEnd;
        public int index;
        public bool isChecked;
    }

    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gridPrefabs;
    [SerializeField] private GameObject item;
    [SerializeField] private TextAsset text0;
    [SerializeField] private TextAsset text1;
    [SerializeField] private TextAsset text2;
    [SerializeField] private TextAsset text3;
    [SerializeField] private TextAsset text4;

    private int width = 50;
    private int height = 50;
    private GameObject[] grids;
    private List<GridInfo> gridPos;
    private Stack<GridInfo> find;
    private List<Vector3> pathPos;
    private int playerStartPosx;
    private int playerStartPosy;
    private int itemNum = 100;

    void Start()
    {
        int a = 0;
    }

    public void Init()
    {
        photonView = GetComponent<PhotonView>();
        playerStartPosx = 1;
        playerStartPosy = 1;
        grids = new GameObject[width * height];

        gridPos = new List<GridInfo>();
        find = new Stack<GridInfo>();
        pathPos = new List<Vector3>();

        ReadText();
        SetPos();
        SetGrid();
        SetItem();
    }

    void SetItem()
    {
        for (int i = 0; i < itemNum; ++i)
        {
            int randomIndexX = Utils.Util.GetRandomPosInInt(0, width);
            int randomIndexY = Utils.Util.GetRandomPosInInt(0, height);

            photonView.RPC("InstanceItem", RpcTarget.All, randomIndexX, randomIndexY);
            //PhotonNetwork.Instantiate("Check", new Vector3(randomIndexX, randomIndexY), Quaternion.identity);
        }
    }

    void ReadText()
    {
        int randomMapNumber = Random.Range(0, 6);
        Stream textStream;
        switch (randomMapNumber)
        {
            case 0:
                textStream = new MemoryStream(text0.bytes);
                break;
            case 1:
                textStream = new MemoryStream(text1.bytes);
                break;
            case 2:
                textStream = new MemoryStream(text2.bytes);
                break;
            case 3:
                textStream = new MemoryStream(text3.bytes);
                break;
            case 4:
                textStream = new MemoryStream(text4.bytes);
                break;
            default:
                textStream = new MemoryStream(text0.bytes);
                break;
        }
        
        
        StreamReader stream = new StreamReader(textStream);
        while (!stream.EndOfStream)
        {
            string str = stream.ReadLine();
            string[] split = str.Split(',');
            float x = float.Parse(split[0]);
            float y = float.Parse(split[1]);

            pathPos.Add(new Vector3(x, y));
        }

        stream.Close();
    }

    [PunRPC]
    private void InstanceItem(int x, int y)
    {
        Utils.Util.InstanceGameObject(item, new Vector3(x, y), Utils.ObjKind.Item);
    }

    void SetPos()
    {
        int index = 0;

        for (float i = 0; i < height; i += 0.5f)
        {
            for (float j = 0; j < width; j += 0.5f)
            {
                gridPos.Add(new GridInfo(j, i, index, true));
                int check = 0;
                bool isChecked = false;
                foreach (Vector3 pos in pathPos)
                {
                    if (Math.Abs(i - pos.y) < float.Epsilon && Math.Abs(j - pos.x) < float.Epsilon)
                    {
                        gridPos[gridPos.Count - 1].isWall = false;
                        
                        isChecked = true;
                        break;
                    }
                    check++;
                }

                if (isChecked)
                {
                    pathPos.RemoveAt(check);
                }
            }
        }
    }

    void SetGrid()
    {
        foreach (GridInfo gridInfo in gridPos)
        {
            photonView.RPC("InstantiateMapGrid", RpcTarget.All, new Vector3(gridInfo.x, gridInfo.y, 0), gridInfo.isWall, gridInfo.isEnd);
        }
    }

    [PunRPC]
    private void InstantiateMapGrid(Vector3 pos, bool isWall, bool isEnd)
    {
        var gridGo = Utils.Util.InstanceGameObject(gridPrefabs, pos);

        if (isWall)
        {
            gridGo.GetComponent<GridSprite>().SetSpriteType(GridSprite.Type.Wall);
        }
        else if (isEnd)
        {
            gridGo.GetComponent<GridSprite>().SetSpriteType(GridSprite.Type.End);
        }
        else
        {
            gridGo.GetComponent<GridSprite>().SetSpriteType(GridSprite.Type.Land);
            gridGo.GetComponent<BoxCollider2D>().enabled = false;
        }
    }


    bool Search()
    {
        if (find.Count <= 0)
        {
            return false;
        }
        GridInfo info = find.Pop();

        if (info.isEnd)
            return true;

        if (info.x - 1 > 0)
        {
            int newIndex = info.index - 1;
            CheckAndPushGridInfo(gridPos[newIndex]);
        }

        if (info.x + 1 < width)
        {
            int newIndex = info.index + 1;
            CheckAndPushGridInfo(gridPos[newIndex]);
        }

        if (info.y - 1 > 0)
        {
            int newIndex = info.index - width;
            CheckAndPushGridInfo(gridPos[newIndex]);
        }

        if (info.y + 1 > 0)
        {
            int newIndex = info.index + width;
            CheckAndPushGridInfo(gridPos[newIndex]);
        }

        return Search();
    }
    void CheckAndPushGridInfo(GridInfo info)
    {
        if (!info.isWall)
        {
            if (!info.isChecked)
            {
                info.isChecked = true;
                find.Push(info);
            }
        }
    }

    void ClearList()
    {
        gridPos.Clear();
        find.Clear();
        SetPos();
        find.Push(gridPos[width + 1]);
        gridPos[width + 1].isChecked = true;
    }

    int TwoDtoOneD(int x, int y)
    {
        return y * width + x;
    }


}
/*
        int index = 0;
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                {
                    gridPos.Add(new GridInfo(j, i, index, true));
                }
                else
                {
                    if (i == height - 2 && j == width - 2)
                    {
                        gridPos.Add(new GridInfo(j, i, index, false, true));
                    }
                    else if (i == playerStartPosy && j == playerStartPosx)
                    {
                        gridPos.Add(new GridInfo(j, i, index, false));
                    }
                    else
                    {
                        int randomVal = Random.Range(1, 3);

                        if (randomVal == 1)
                        {
                            gridPos.Add(new GridInfo(j, i, index, false));
                        }
                        else
                        {
                            gridPos.Add(new GridInfo(j, i, index, true));
                        }
                    }
                }
                index++;
            }
        }
        */