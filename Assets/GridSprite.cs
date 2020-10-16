using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
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
    private string playerTag;
    private GameObject currCollidingObj;
    [SerializeField] private Sprite[] groundSprite;
    [SerializeField] private Sprite[] wallSprite;
    [SerializeField] private Sprite endSprite;

    [SerializeField] private Sprite wallBreakSprite1;
    [SerializeField] private Sprite wallBreakSprite2;
    private Type spriteType = Type.None;

    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        playerTag = "Player";
    }

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
    public void SetSpriteType(Type type)
    {
        if (!spriteRender)
        {
            spriteRender = GetComponent<SpriteRenderer>();
        }

        switch (type)
        {
            case Type.None:
                break;
            case Type.Land:
                spriteRender.sprite = groundSprite[0];
                gameObject.layer = LayerMask.NameToLayer("Default");
                break;
            case Type.Wall:
                spriteRender.sprite = wallSprite[0];
                gameObject.layer = LayerMask.NameToLayer("ColliderLayer");
                break;
            case Type.End:
                spriteRender.sprite = endSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void WallToGroundSpriteChange(int hp)
    {
        if (hp <= 0)
        {
            spriteRender.sprite = groundSprite[0];
            gameObject.layer = LayerMask.NameToLayer("Default");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            if (currCollidingObj)
            {
                //currCollidingObj.transform.Find("ToolGfx").gameObject.SetActive(false);
                currCollidingObj.GetComponent<PlayerBehavior>().PlayerItem.DecreItemDuration();
            }
        }
        else if (hp <= 7)
        {
            spriteRender.sprite = wallBreakSprite2;
        }
        else if (hp <= 15)
        {
            spriteRender.sprite = wallBreakSprite1;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(playerTag))
        {
            currCollidingObj = col.gameObject;
        }
    }

}
