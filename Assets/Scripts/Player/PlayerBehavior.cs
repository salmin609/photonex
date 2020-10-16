using System;
using Photon.Pun;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerBehavior : MonoBehaviour
    {
        private PlayerInput inputInfo;
        private PlayerMove moveInfo;
        private PlayerItem itemInfo;
        private PhotonView photonView;
        private Joystick joystick;
        private Rigidbody2D rigidBody;
        private SpriteRenderer playerSprite;
        private Animator toolAnimator;
        private bool isWallBreakEnabled;

        void Start()
        {
            joystick = GameObject.Find("JoyStick").GetComponent<Joystick>();
            inputInfo = new PlayerInput(transform);
            itemInfo = new PlayerItem(transform);
            moveInfo = new PlayerMove(transform);
            toolAnimator = transform.Find("ToolGfx").GetComponent<Animator>();
            photonView = GetComponent<PhotonView>();
            rigidBody = GetComponent<Rigidbody2D>();
            playerSprite = transform.Find("PlayerGfx").GetComponent<SpriteRenderer>();
            isWallBreakEnabled = true;
        }

        public PlayerItem PlayerItem => itemInfo;
        public Joystick JoyStick => joystick;

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

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.tag == "Item")
            {
                itemInfo?.SetItemKind(PlayerItem.ItemKind.Pickax);
                itemInfo?.SetItemDuration(3);
                Destroy(col.gameObject);
                photonView?.RPC("ActiveTool", RpcTarget.AllBuffered);

            }
        }

        [PunRPC]
        void ActiveTool()
        {
            transform.Find("ToolGfx").gameObject.SetActive(true);
        }

        void OnCollisionStay2D(Collision2D col)
        {
            if (col.collider.tag == "Wall")
            {
                if (itemInfo.GetItemKind() == PlayerItem.ItemKind.Pickax)
                {
                    GridSprite gridInfo = col.collider.gameObject.GetComponent<GridSprite>();

                    if (gridInfo)
                    {
                        if (isWallBreakEnabled)
                        {
                            Utils.Util.DeleteGameObj(col.collider.gameObject);
                            toolAnimator.SetTrigger("Chop");

                            isWallBreakEnabled = false;
                            Utils.Util.CoSeconds(() =>
                            {
                                isWallBreakEnabled = true;
                            }, 0.1f);
                            playerSprite.color = Color.white;
                        }
                        else
                        {
                            playerSprite.color = Color.red;
                        }
                    }
                    else
                    {
                        playerSprite.color = Color.blue;
                    }
                }
            }
        }
    }
}
