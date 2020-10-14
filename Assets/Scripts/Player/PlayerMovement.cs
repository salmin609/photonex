using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView photonView;
    private CharacterController characterController;
    //private Rigidbody rigidBody;
    private float moveSpeed;
    private Animator chickenAnimator;
    private Vector3 movementDir;
    private LayerMask colliderLayer;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();
        moveSpeed = 300f;
        chickenAnimator = transform.GetChild(0).GetComponent<Animator>();
        colliderLayer = LayerMask.NameToLayer("ColliderLayer");
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Movement();
        }
    }


    void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            characterController.SimpleMove(direction * moveSpeed * Time.deltaTime);

            if (chickenAnimator)
            {
                chickenAnimator.SetBool("Run", true);
            }
        }
        else
        {
            chickenAnimator.SetBool("Run", false);
        }
    }
}
