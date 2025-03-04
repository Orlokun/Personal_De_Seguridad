using DataUnits.ItemScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UIItemInStoreObject : MonoBehaviour, IInitializeWithArg1<IItemObject>
{
    private IItemObject _mItemObject;
    
    [SerializeField] private Image mItemIcon;
    
    [SerializeField] private Button mMainButton;
    [SerializeField] private Button mAddToCart;
    [SerializeField] private Button mRemoveFromCart;
    
    [SerializeField] private TMP_Text mItemName;
    [SerializeField] private TMP_Text mItemMaxAmount;
    [SerializeField] private TMP_Text mItemAmount;
    
    public bool IsInitialized => _mInitialized;
    private bool _mInitialized;
    
    private void Awake()
    {
        mMainButton.onClick.AddListener(OnMainButtonClicked);
        mAddToCart.onClick.AddListener(OnAddButtonClicked);
        mMainButton.onClick.AddListener(OnRemoveButtonClicked);
    }

    private void OnRemoveButtonClicked()
    {
        throw new System.NotImplementedException();
    }

    private void OnAddButtonClicked()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize(IItemObject injectionClass)
    {
        _mItemObject = injectionClass;
        mItemName.text = _mItemObject.ItemName;
        mItemAmount.text = _mItemObject.ItemAmount.ToString();
        mItemMaxAmount.text = _mItemObject.ItemAmount.ToString();
        mItemIcon.sprite = _mItemObject.ItemIcon;
    }
    private void OnMainButtonClicked()
    {
        //Open Item Details Info Panel
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
