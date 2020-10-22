using Photon.Pun;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCollision : MonoBehaviourPunCallbacks
    {
        private PlayerItem itemInfo;
        private bool isWallBreakEnabled;
        private Animator toolAnimator;
        private SpriteRenderer playerSprite;
        private PhotonView photonView;
        private GameObject collidingObj;
        private bool isTaken;

        public void SetIsTaken(bool toggle)
        {
            isTaken = toggle;
        }

        void Start()
        {
            itemInfo = GetComponent<PlayerBehavior>().PlayerItem;
            isWallBreakEnabled = true;
            isTaken = false;
            toolAnimator = transform.Find("ToolGfx").GetComponent<Animator>();
            playerSprite = transform.Find("PlayerGfx").GetComponent<SpriteRenderer>();
            photonView = GetComponent<PhotonView>();
            Debug.Log($"On Enabled called {photonView.IsMine}");
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.tag == "Item")
            {
                collidingObj = col.gameObject;
                itemInfo?.SetItemKind(PlayerItem.ItemKind.Pickax);
                itemInfo?.SetItemDuration(3);
                transform.Find("ToolGfx").gameObject.SetActive(true);
                Destroy(collidingObj);
            }
            else if (col.gameObject.tag == "Player")
            {
                if (!isTaken)
                {
                    PlayerBehavior playerInfo = col.gameObject.GetComponent<PlayerBehavior>();
                    PlayerBehavior thisPlayerInfo = GetComponent<PlayerBehavior>();
                    PlayerBehavior takenPlayer = null;
                    PlayerBehavior takePlayer = null;

                    #region Distinguish

                    if (playerInfo != null && thisPlayerInfo != null)
                    {
                        if (playerInfo.PlayerItem != null && thisPlayerInfo.PlayerItem != null)
                        {
                            if (playerInfo.PlayerItem.Kind != PlayerItem.ItemKind.None &&
                                thisPlayerInfo.PlayerItem.Kind == PlayerItem.ItemKind.None)
                            {
                                takenPlayer = playerInfo;
                                takePlayer = thisPlayerInfo;
                            }
                            else if (playerInfo.PlayerItem.Kind == PlayerItem.ItemKind.None &&
                                    thisPlayerInfo.PlayerItem.Kind != PlayerItem.ItemKind.None)
                            {
                                takenPlayer = thisPlayerInfo;
                                takePlayer = playerInfo;
                            }
                        }
                    }
                    #endregion

                    if (takenPlayer != null)
                    {
                        takePlayer.PlayerItem.SetItem(takenPlayer.PlayerItem.ItemDuration, takenPlayer.PlayerItem.Kind);
                        takenPlayer.PlayerItem.SetItemNull();
                        
                        takePlayer.Sprite.color = Color.yellow;
                        takenPlayer.Sprite.color = Color.green;

                        Debug.Log("Take!");

                        #region Timer
                        isTaken = true;
                        col.gameObject.GetComponent<PlayerCollision>().SetIsTaken(true);

                        Utils.Util.CoSeconds(() =>
                        {
                            isTaken = false;
                            col.gameObject.GetComponent<PlayerCollision>().SetIsTaken(false);
                        }, 0.5f);
                        #endregion
                    }
                }
            }
        }

        void OnCollisionStay2D(Collision2D col)
        {
            if (col.collider.tag == "Wall")
            {
                if (itemInfo != null)
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
}
