using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GridSprite : MonoBehaviour
{
    public enum Type
    {
        None,
        Land,
        Wall,
        End
    }
    private SpriteRenderer spriteRender;
    [SerializeField] private Sprite[] groundSprite;
    [SerializeField] private Sprite[] wallSprite;
    [SerializeField] private Sprite endSprite;
    private PhotonView photonView;
    private Type spriteType = Type.None;

    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            if (!spriteRender)
            {
                photonView.RPC("SpriteRPC", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void SpriteRPC()
    {
        spriteRender = gameObject.AddComponent<SpriteRenderer>();

        switch (spriteType)
        {
            case Type.Land:
                spriteRender.sprite = groundSprite[0];
                break;
            case Type.Wall:
                spriteRender.sprite = wallSprite[0];
                gameObject.layer = LayerMask.NameToLayer("ColliderLayer");
                break;
            case Type.End:
                spriteRender.sprite = endSprite;
                break;
        }
    }
    [PunRPC]
    public void SetSpriteType(Type type)
    {
        photonView = GetComponent<PhotonView>();

        if (spriteType == Type.None)
        {
            spriteType = type;
            photonView.RPC("SetSpriteType", RpcTarget.All, type);
        }
    }
}
