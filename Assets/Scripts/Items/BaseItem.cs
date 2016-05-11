using UnityEngine;
using System.Collections;

namespace Items
{
    public abstract class BaseItem : MonoBehaviour
    {
        private GameObject _user = null;
        public GameObject User
        {
            get { return _user; }
            set { _user = value; }
        }

        public abstract void DoAction();
    }
}