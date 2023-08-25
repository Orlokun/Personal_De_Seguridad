using UnityEngine;
using UnityEngine.UI;

public class BaseInventoryItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    public Image IconImage
    {
        get => iconImage;
        set => iconImage = value;
    }
}
