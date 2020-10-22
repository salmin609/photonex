using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class PlayerItem
    {
        private Transform playerToolGfxTransform;
        public enum ItemKind
        {
            None,
            Pickax
        }

        public PlayerItem(Transform playerTrans)
        {
            itemKind = ItemKind.None;
            playerTransform = playerTrans;
            playerToolGfxTransform = playerTransform.Find("ToolGfx");
        }

        private ItemKind itemKind;
        private Transform playerTransform;
        private int itemDuration;

        public ItemKind Kind => itemKind;
        public int ItemDuration => itemDuration;
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
                SetItemNull();
            }
        }

        public void SetItemNull()
        {
            SetItemDuration(0);
            SetItemKind(ItemKind.None);
            SetToolGfxToggle(false);
        }
        public void SetItem(int itemDur, ItemKind kind)
        {
            SetItemDuration(itemDur);
            SetItemKind(kind);

            if (kind != ItemKind.None)
            {
                SetToolGfxToggle(true);
            }
        }

        public void SetToolGfxToggle(bool toggle)
        {
            if (playerToolGfxTransform)
            {
                playerToolGfxTransform.gameObject.SetActive(toggle);
                Debug.Log($"SetToolGfxToggle {toggle}");
            }
        }
    }
}
