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
            _item = item;
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
                _item = _itemManager.PickRandomItem();
            }
            else
            {
                _item = _itemManager.UpgradeItem(_item);
            }
        }

        private void PickNotSoRandomItem()
        {
            if (_item == null)
            {
                _item = _itemManager.PickNotSoRandomItem();
            }
            else
            {
                _item = _itemManager.UpgradeItem(_item);
            }
        }

        public void UseItem()
        {
            if (_item == null)
                return;

            var item = GameObject.Instantiate<GameObject>(_item);
            item.name = _item.name;
            item.transform.position = transform.position;

            var baseItem = item.GetComponent<Items.BaseItem>();
            baseItem.User = this.gameObject;
            baseItem.DoAction();

            _item = null;
        }
    }
}