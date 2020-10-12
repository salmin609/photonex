using Photon.Pun;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerBehavior : MonoBehaviour
    {
        private PlayerInput inputInfo;
        private PlayerMove moveInfo;
        private PhotonView photonView;
        void Start()
        {
            inputInfo = new PlayerInput();
            moveInfo = new PlayerMove(transform);
            photonView = GetComponent<PhotonView>();
        }

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
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Item")
            {
                Destroy(col.gameObject);
            }
        }
    }
}
