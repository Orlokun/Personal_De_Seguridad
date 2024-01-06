using UnityEngine;

namespace GameDirection.GeneralLevelManager
{
    public interface IStoreEntrancePosition
    {
        public Vector3 StartPosition { get; }
        public Vector3 EntrancePosition { get; }
    }
}