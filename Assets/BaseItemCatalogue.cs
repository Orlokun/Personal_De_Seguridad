using System.Collections.Generic;
using System.Linq;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

public interface IBaseItemCatalogue
{ 
    public List<IItemObject> ExistingObjects { get; }
    public IItemObject GetItemFromCatalogue(BitItemSupplier itemSupplier, int itemBitId);
}

public class BaseItemCatalogue : MonoBehaviour, IBaseItemCatalogue
{
    private static BaseItemCatalogue _instance;
    public static IBaseItemCatalogue Instance => _instance;
    
    [SerializeField] private List<ItemObject> _createdItems;
    
    private List<IItemObject> _itemsInterface = new List<IItemObject>();
    public List<IItemObject> ExistingObjects => _itemsInterface;
    public void Awake()
    {
        DontDestroyOnLoad(this);
        if (_instance != null)
        {
            Destroy(this);
        }
        _instance = this;
        LoadInterfaces();
    }

    private void LoadInterfaces()
    {
        _itemsInterface.Clear();
        foreach (var item in _createdItems)
        {
            var itemManager = (IItemManage) item;
            itemManager.SetBitId();
            
            _itemsInterface.Add(item);
        }
    }

    public IItemObject GetItemFromCatalogue(BitItemSupplier itemSupplier, int itemBitId)
    {
        return _itemsInterface.SingleOrDefault(x => x.ItemSupplier == itemSupplier && x.BitId == itemBitId);
    }
    
}
