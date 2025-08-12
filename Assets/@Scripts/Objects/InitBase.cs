    using UnityEngine;

    public abstract class InitBase : MonoBehaviour
    {
        private bool _init = false;
        public virtual bool Init()
        {
            if (_init)
                return false;

            _init = true;
            return true;
        }
    }
