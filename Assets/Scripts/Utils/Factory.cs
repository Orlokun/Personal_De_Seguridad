using DataUnits.GameCatalogues;
using DialogueSystem;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;

namespace Utils
{
    public static class Factory
    {
        public static WorkDayObject CreateWorkday(DayBitId dayBitId)
        {
            return new WorkDayObject(dayBitId);
        }
        public static CalendarModule CreateCalendarModule()
        {
            return new CalendarModule(DayBitId.DayOne, PartOfDay.EarlyMorning);
        }
        public static DialogueCameraMan CreateCameraMan()
        {
            return new DialogueCameraMan();
        }
        public static ItemSupplierShop CreateItemStoreSupplier(BitItemSupplier itemSupplier, IItemsDataController itemDataController,IBaseItemSuppliersCatalogue suppliersCatalogue)
        {
            return new ItemSupplierShop(itemSupplier, itemDataController, suppliersCatalogue);
        }
        
        public static PlayerGameProfile CreatePlayerGameProfile(IItemSuppliersModule itemSuppliersModule, IJobsSourcesModule jobsModule, 
            ICalendarModule calendarManager)
        {
            return new PlayerGameProfile(itemSuppliersModule, jobsModule, calendarManager);
        }

        public static IItemSuppliersModule CreateItemSuppliersModule(IItemsDataController itemDataController, IBaseItemSuppliersCatalogue suppliersCatalogue)
        {
            return new ItemSuppliersModule(itemDataController, suppliersCatalogue);
        }
        public static IJobsSourcesModule CreateJobSourcesModule(IBaseJobsCatalogue jobsCatalogue)
        {
            return new JobsSourcesModule(jobsCatalogue);
        }
    }
}