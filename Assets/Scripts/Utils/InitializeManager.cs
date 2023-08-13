using UnityEngine;

namespace Utils
{
    public abstract class InitializeManager : MonoBehaviour, IInitialize
    {
        protected bool MIsInitialized;
        public bool IsInitialized => MIsInitialized;
        public virtual void Initialize()
        {
            //Implemented in Inheritors
        }
    }
}