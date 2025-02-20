using UI;
using UnityEngine;

public class UIHighlightComponent : MonoBehaviour, IUIHighlightComponent
{
    [SerializeField] private HighlightObjectID mHighlightObjectID;
    public HighlightObjectID HighlightObjectID => mHighlightObjectID;
    public RectTransform GetRectTransform => GetComponent<RectTransform>();

}

public interface IUIHighlightComponent
{
    public RectTransform GetRectTransform { get; }
}
