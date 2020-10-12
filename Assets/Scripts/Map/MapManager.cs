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

    private int width = 8;
    private int height = 8;
    private GameObject[] grids;
    private List<GridInfo> gridPos;
    private Stack<GridInfo> find;
    private int playerStartPosx;
    private int playerStartPosy;

    //public override void OnEnable()
    //{
    //    PhotonNetwork.AddCallbackTarget(this);
    //    Init();
    //}
    //public override void OnDisable()
    //{
    //    PhotonNetwork.RemoveCallbackTarget(this);
    //}
    void Start()
    {
        //photonView = GetComponent<PhotonView>();
        //if (photonView.IsMine)
        //{
        //    Init();
        //}
        //Debug.Log("Why");
        //playerStartPosx = 1;
        //playerStartPosy = 1;
        //grids = new GameObject[width * height];
        //gridPos = new List<GridInfo>();
        //find = new Stack<GridInfo>();

        //do
        //{
        //    ClearList();
        //} while (!Search());

        //SetGrid();
        ////Instantiate(player, new Vector3(playerStartPosx, playerStartPosy), Quaternion.identity);
        //PhotonNetwork.Instantiate("Player", new Vector3(playerStartPosx, playerStartPosy), Quaternion.identity);
    }

    public void Init()
    {
        Debug.Log("Why");
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
        //Instantiate(player, new Vector3(playerStartPosx, playerStartPosy), Quaternion.identity);
        //PhotonNetwork.Instantiate("Player", new Vector3(playerStartPosx, playerStartPosy), Quaternion.identity);
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
        int count = 0;
        //GameObject parent = Instantiate(new GameObject("Grid Parent"), new Vector3(0f, 0f), Quaternion.identity);
        GameObject parent = PhotonNetwork.Instantiate("GridParentPrefab", Vector3.zero, Quaternion.identity);
        
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                int index = TwoDtoOneD(j, i);

                GameObject obj = PhotonNetwork.Instantiate("GridPrefab", Vector3.zero, Quaternion.identity);

                //photonView.RPC("AddGridInfo", RpcTarget.AllBuffered, obj);
                obj.name = count.ToString();
                count++;
                obj.transform.position = new Vector3(j, i, 0);
                //obj.GetComponent<GridSprite>().SetSpriteType();
                //obj.layer = LayerMask.NameToLayer("ColliderLayer");
                //obj.transform.SetParent(parent.transform);

                if (gridPos[index].isWall)
                {
                    obj.GetComponent<GridSprite>().SetSpriteType(GridSprite.Type.Wall);

                    //obj.AddComponent<SpriteRenderer>().sprite =
                    //    grid.GetComponent<Grid>().GetRandomWallSprite();
                    //obj.AddComponent<BoxCollider2D>();
                    //obj.layer = LayerMask.NameToLayer("ColliderLayer");
                }
                else if (gridPos[index].isEnd)
                {
                    //obj.AddComponent<SpriteRenderer>().sprite =
                    //    grid.GetComponent<Grid>().GetEndSprite();
                    obj.GetComponent<GridSprite>().SetSpriteType(GridSprite.Type.End);
                }
                else
                {
                    //obj.AddComponent<SpriteRenderer>().sprite =
                    //    grid.GetComponent<Grid>().GetRandomGroundSprite();
                    obj.GetComponent<GridSprite>().SetSpriteType(GridSprite.Type.Land);
                }

            }
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

    [PunRPC]
    void AddGridInfo(GameObject gridInfoToAdd)
    {
        gridInfoToAdd.name = "1";
        gridInfoToAdd.AddComponent<SpriteRenderer>().sprite = grid.GetComponent<Grid>().GetRandomGroundSprite();
    }
}
