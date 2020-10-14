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
        public GridInfo(int xpos, int ypos, int index, bool iswall, bool isEnd = false)
        {
            x = xpos;
            y = ypos;
            isWall = iswall;
            this.index = index;
            this.isEnd = isEnd;
            isChecked = false;
        }

        public int x, y;
        public bool isWall;
        public bool isEnd;
        public int index;
        public bool isChecked;
    }

    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gridPrefabs;
    [SerializeField] private GameObject item;

    private int width = 30;
    private int height = 30;
    private GameObject[] grids;
    private List<GridInfo> gridPos;
    private Stack<GridInfo> find;
    private int playerStartPosx;
    private int playerStartPosy;
    private int itemNum = 10;

    void Start()
    {
    }

    public void Init()
    {
        photonView = GetComponent<PhotonView>();
        playerStartPosx = 1;
        playerStartPosy = 1;
        grids = new GameObject[width * height];

        gridPos = new List<GridInfo>();
        find = new Stack<GridInfo>();

        do
        {
            ClearList();
        } while (!Search());

        SetGrid();
        SetItem();
    }

    void SetItem()
    {
        for (int i = 0; i < itemNum; ++i)
        {
            int randomIndexX = Utils.Util.GetRandomPosInInt(0, 80);
            int randomIndexY = Utils.Util.GetRandomPosInInt(0, 80);

            //all map manager need to do setting
            photonView.RPC("InstanceItem", RpcTarget.All, randomIndexX, randomIndexY);
            //Utils.Util.InstanceGameObject(item, new Vector3(randomIndexX, randomIndexY), Utils.ObjKind.Item);
        }
    }

    [PunRPC]
    private void InstanceItem(int x, int y)
    {
        Utils.Util.InstanceGameObject(item, new Vector3(x, y), Utils.ObjKind.Item);
    }

    void SetPos()
    {
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
    }

    void SetGrid()
    {
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                int index = TwoDtoOneD(j, i);

                //if (gridPos[index].isWall)
                //{
                    photonView.RPC("InstantiateMapGrid", RpcTarget.All, new Vector3(j, i, 0), gridPos[index].isWall, gridPos[index].isEnd);
                //}
            }
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
