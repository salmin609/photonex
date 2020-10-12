using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Sprite[] wallsprites;
    [SerializeField] private Sprite[] groundsprites;
    [SerializeField] private Sprite endsprite;

    public Sprite GetRandomWallSprite()
    {
        return wallsprites[Random.Range(1, wallsprites.Length - 1)];
    }
    public Sprite GetRandomGroundSprite()
    {
        //return groundsprites[Random.Range(1, groundsprites.Length - 1)];
        return groundsprites[0];
    }

    public Sprite GetEndSprite()
    {
        return endsprite;
    }
}
