using Photon.Pun;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMove
    {
        private readonly Transform playerTransform;
        private readonly LayerMask collisionLayer;
        private float speed;
        private PlayerItem itemInfo;
        private Rigidbody2D rigidBody;
        private PhotonView photonView;
        private GameObject player;
        private SpriteRenderer spriteRenderer;
        public PlayerMove(Transform playerTrans = null)
        {
            playerTransform = playerTrans ? playerTrans : GameObject.Find("Player").transform;
            player = playerTransform.gameObject;
            collisionLayer = LayerMask.GetMask("ColliderLayer");
            itemInfo = player.GetComponent<PlayerBehavior>().PlayerItem;
            rigidBody = player.GetComponent<Rigidbody2D>();
            photonView = player.GetPhotonView();
            spriteRenderer = playerTransform.Find("PlayerGfx").GetComponent<SpriteRenderer>();
            speed = 2f;
        }

        public void Update(PlayerInputInfo inputInfo)
        {
            rigidBody.velocity = new Vector2(inputInfo.horizontal * speed, inputInfo.vertical * speed);
            playerTransform.Find("PlayerGfx").GetComponent<Animator>().SetTrigger("Run");

            if (inputInfo.horizontal < 0)
            {
                player.GetComponent<PlayerBehavior>().FlipSprite(true);
            }
            else
            {
                player.GetComponent<PlayerBehavior>().FlipSprite(false);
            }
        }
    }
}
