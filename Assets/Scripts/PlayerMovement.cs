using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView photonView;
    private CharacterController characterController;
    public float moveSpeed;
    public float rotationSpeed;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Movement();
            Rotation();
        }
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            characterController.Move(transform.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            characterController.Move(-transform.right * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            characterController.Move(-transform.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            characterController.Move(transform.right * Time.deltaTime * moveSpeed);
        }
    }

    void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0f, mouseX, 0));
    }
}
