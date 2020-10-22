using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerBehavior : MonoBehaviourPunCallbacks
    {
        private PlayerInput inputInfo;
        private PlayerMove moveInfo;
        private PlayerItem itemInfo;
        private PhotonView photonView;
        private Joystick joystick;
        private Rigidbody2D rigidBody;
        private SpriteRenderer playerSprite;

        void Start()
        {
            joystick = GameObject.Find("JoyStick").GetComponent<Joystick>();
            inputInfo = new PlayerInput(transform);
            itemInfo = new PlayerItem(transform);
            moveInfo = new PlayerMove(transform);
            photonView = GetComponent<PhotonView>();
            rigidBody = GetComponent<Rigidbody2D>();
            playerSprite = transform.Find("PlayerGfx").GetComponent<SpriteRenderer>();
        }

        public PlayerItem PlayerItem => itemInfo;
        public Joystick JoyStick => joystick;
        public SpriteRenderer Sprite => playerSprite;
        void Update()
        {
            if (photonView.IsMine)
            {
                inputInfo.Update();

                if (!inputInfo.IsPlayerInputEmpty())
                {
                    PlayerInputInfo input = inputInfo.GetPlayerInput();
                    moveInfo.Update(input);
                }
                else
                {
                    rigidBody.velocity = Vector2.zero;
                }
            }
        }

        public void FlipSprite(bool isLeft)
        {
            photonView.RPC("RPCFlip", RpcTarget.AllBuffered, isLeft);
        }

        [PunRPC]
        private void RPCFlip(bool isLeft)
        {
            if (isLeft)
            {
                playerSprite.flipX = true;
            }
            else
            {
                playerSprite.flipX = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnect issue : {cause}");
        }
    }
}
