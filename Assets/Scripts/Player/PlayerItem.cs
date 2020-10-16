using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class PlayerItem
    {
        public enum ItemKind
        {
            None,
            Pickax
        }

        public PlayerItem(Transform playerTrans)
        {
            itemKind = ItemKind.None;
            playerTransform = playerTrans;
        }

        private ItemKind itemKind;
        private Transform playerTransform;
        private int itemDuration;

        public void SetItemKind(ItemKind kind)
        {
            itemKind = kind;
        }
        public ItemKind GetItemKind()
        {
            return itemKind;
        }

        public void SetItemDuration(int num)
        {
            itemDuration = num;
        }

        public void DecreItemDuration()
        {
            itemDuration--;

            if (itemDuration <= 0)
            {
                itemKind = ItemKind.None;
                playerTransform.Find("ToolGfx").gameObject.SetActive(false);
            }
        }

    }
}
