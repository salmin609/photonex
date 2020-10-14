using Photon.Pun;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMove
    {
        private readonly Transform playerTransform;
        private readonly LayerMask collisionLayer;
        private float speed;
        public PlayerMove(Transform playerTransform = null)
        {
            this.playerTransform = playerTransform ? playerTransform : GameObject.Find("Player").transform;
            collisionLayer = LayerMask.GetMask("ColliderLayer");
            speed = 30f;
        }

        public void Update(PlayerInputInfo inputInfo)
        {
            Vector3 nextPos = playerTransform.position + new Vector3(inputInfo.horizontal, inputInfo.vertical, 0f);

            if (CheckNextMoveBlocked(nextPos))
            {
                //float timeMultiplySpeed = Time.deltaTime * speed;
                //playerTransform.position += new Vector3(inputInfo.horizontal * timeMultiplySpeed, 0f,
                //    inputInfo.vertical * timeMultiplySpeed);
                playerTransform.position = nextPos;
            }
        }

        private bool CheckNextMoveBlocked(Vector3 nextPos)
        {
            Collider2D colInfo = Physics2D.OverlapCircle(nextPos, 0.2f, collisionLayer);

            if (colInfo)
            {
                GridSprite gridInfo = colInfo.gameObject.GetComponent<GridSprite>();

                if (gridInfo)
                {
                    Utils.Util.DeleteGameObj(colInfo.gameObject);
                }
                return false;
            }
            return true;
        }
    }
}
