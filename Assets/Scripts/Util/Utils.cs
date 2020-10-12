using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Utils : MonoBehaviour
{
    private static Utils util;
    public static Utils Util => util;


    void Start()
    {
        if (!util)
        {
            util = this;
        }
        DontDestroyOnLoad(gameObject);
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
}
