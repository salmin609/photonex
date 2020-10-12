using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMove
    {
        private readonly Transform playerTransform;
        private readonly LayerMask collisionLayer;
        public PlayerMove(Transform playerTransform = null)
        {
            this.playerTransform = playerTransform ? playerTransform : GameObject.Find("Player").transform;
            collisionLayer = LayerMask.GetMask("ColliderLayer");
        }

        public void Update(PlayerInputInfo inputInfo)
        {
            Vector3 nextPos = playerTransform.position + new Vector3(inputInfo.horizontal, inputInfo.vertical, 0f);

            if (CheckNextMoveBlocked(nextPos))
            {
                playerTransform.position = nextPos;
            }
        }

        private bool CheckNextMoveBlocked(Vector3 nextPos)
        {
            Collider2D colInfo = Physics2D.OverlapCircle(nextPos, 0.2f, collisionLayer);

            if (colInfo)
            {
                return false;
            }
            return true;
        }
    }
}
