using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask stopsMovement;
    public LayerMask exitLayer;
    public string nextlevel;
    private bool isReachEnd;
    private SpriteRenderer spriteRender;
    void Start()
    {
        isReachEnd = false;
        movePoint.parent = null;
        spriteRender = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(movePoint.position.x, movePoint.position.y, -10f), moveSpeed * Time.deltaTime);
        if (!isReachEnd)
        {
            PlayerInput();
        }
    }

    void PlayerInput()
    {
        if (Vector3.Distance(transform.position, movePoint.position) < 0.05f)
        {
            if (Math.Abs(Mathf.Abs(Input.GetAxisRaw("Horizontal")) - 1f) < float.Epsilon)
            {
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(horizontalInput, 0f, 0f),
                    .2f, stopsMovement))
                {
                    if (horizontalInput > 0)
                    {
                        spriteRender.flipX = false;
                    }
                    else
                    {
                        spriteRender.flipX = true;
                    }

                    Vector3 moveVec = new Vector3(horizontalInput, 0f, 0f);
                    movePoint.position += moveVec;

                    if (Physics2D.Raycast(transform.position, new Vector2(horizontalInput, 0f), 1f, exitLayer))
                    {
                        isReachEnd = true;
                        StartCoroutine("LoadNextLevel", 2f);
                    }
                }
            }

            else if (Math.Abs(Mathf.Abs(Input.GetAxisRaw("Vertical")) - 1f) < float.Epsilon)
            {
                float verticalInput = Input.GetAxisRaw("Vertical");
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, verticalInput, 0f), .2f, stopsMovement))
                {
                    if (Physics2D.Raycast(transform.position, new Vector2(0f, verticalInput), 1f, exitLayer))
                    {
                        isReachEnd = true;
                        StartCoroutine("LoadNextLevel", 2f);
                    }

                    Vector3 moveVec = new Vector3(0f, verticalInput, 0f);
                    movePoint.position += moveVec;
                }
            }
        }
    }

    public void Attacked(Vector3 enemyPos)
    {
        Vector3 playerPos = transform.position;

        Vector3 backDir = playerPos - enemyPos;

        if (!Physics2D.Raycast(playerPos, backDir, 1f, stopsMovement))
        {

        }
        else
        {
            float distance = 1f;
            while (Physics.Raycast(playerPos, backDir, distance, stopsMovement))
            {

            }
        }
    }

    IEnumerator LoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextlevel);
    }
}
