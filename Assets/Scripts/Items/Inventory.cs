using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        public delegate void Void_V_GameObject(GameObject go);
        public event Void_V_GameObject onItemIsSet;

        private GameObject _item;
        public GameObject Item
        {
            get { return _item; }
            set
            {
                _item = value;
                if (onItemIsSet != null)
                    onItemIsSet(_item); // so that the UI can display some infos on this item.
            }
        }
        private ItemManager _itemManager;

        void Start()
        {
            _itemManager = GameObject.FindObjectOfType<ItemManager>();
        }

        public void PickSpecificItem(GameObject item)
        {
            Item = item;
        }

        public void PickItem()
        {
            // if (estDansLaMerde)
            //     PickNotSoRandomItem();
            // else
            PickRandomItem();
        }

        private void PickRandomItem()
        {
            if (_item == null)
            {
                Item = _itemManager.PickRandomItem();
            }
            else
            {
                Item = _itemManager.UpgradeItem(Item);
            }
        }

        private void PickNotSoRandomItem()
        {
            if (Item == null)
            {
                Item = _itemManager.PickNotSoRandomItem();
            }
            else
            {
                Item = _itemManager.UpgradeItem(Item);
            }
        }

        public void UseItem()
        {
            if (Item == null)
                return;

            var item = GameObject.Instantiate<GameObject>(_item);
            item.name = Item.name;
            item.transform.position = transform.position;

            var baseItem = item.GetComponent<Items.BaseItem>();
            baseItem.User = this.gameObject;
            baseItem.DoAction();

            Item = null;
        }
    }
}