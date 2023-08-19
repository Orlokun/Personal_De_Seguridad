using UnityEngine;

namespace DataUnits
{
    [CreateAssetMenu(menuName = "Jobs/JobTabElement")]
    public class JobsTabElement : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _snippetText;
    }
}