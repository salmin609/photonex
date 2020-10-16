using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehavior : MonoBehaviour
{
    private int hp;
    private GridSprite sprite;
    void Start()
    {
        hp = 20;
        sprite = GetComponent<GridSprite>();
    }

    public void TryBreakWall()
    {
        hp--;
        sprite.WallToGroundSpriteChange(hp);
    }
}
