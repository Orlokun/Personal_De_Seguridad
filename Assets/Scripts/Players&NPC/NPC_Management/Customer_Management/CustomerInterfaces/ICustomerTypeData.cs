namespace Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface ICustomerTypeData
    {
        /// <summary>
        /// Fields
        /// </summary>
        ///
        public int Cleanness { get; }
        public int SocialStatus { get; }
        public int Age{ get; }
        //public string UrbanStyle{ get; }  //TODO: Evaluate Later
        
        public int Intelligence{ get; }
        public int StealAbility{ get; }
        public int Corruptibility { get; }
        public int Daring { get; }
        public int Fearful { get; }
        public int Aggressive { get; }
    
        public float Agility { get; }
        public float Speed { get; }
        public float Strength { get; }

        //Functions
        public void InitializeData();

    }
}