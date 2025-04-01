namespace ExternalAssets.AdvancedPeopleSystem2.Scripts.ScriptableObjects
{
    public enum CharacterInstanceStatus
    {
        /// <summary>
        /// The object is not part of a Prefab instance.
        /// </summary>
        NotAPrefab,
        /// <summary>
        /// The Prefab instance is connected to its Prefab Asset.
        /// </summary>
        Connected,
        /// <summary>
        /// The Prefab instance is not connected to its Prefab Asset.
        /// </summary>
        Disconnected,
        /// <summary>
        /// The Prefab instance is missing its Prefab Asset.
        /// </summary>
        MissingAsset,
        /// <summary>
        /// The object is not part of a Prefab instance by user.
        /// </summary>
        NotAPrefabByUser,
        /// <summary>
        /// The object is prefab stage opened
        /// </summary>
        PrefabStageSceneOpened,
        /// <summary>
        /// The object editing in project view
        /// </summary>
        PrefabEditingInProjectView,
    }
}