using System;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management
{
    public class BaseCustomerTypeData : ICustomerTypeData
    {
        #region Protected Members
        //Social params
        protected int MCleanness;
        protected int MSocialStatus;
        protected int MAge;
        //protected string MUrbanStyle;       //TODO: Evaluate later
        
        //Psychological params
        protected int MIntelligence;
        protected int MStealAbility;
        protected int MCorruptibility;
        protected int MDaring;
        protected int MLawfulness;
        protected int MAggressive;
        
        //Physical params
        protected float MAgility;
        protected float MSpeed;
        protected float MStrength;
        #endregion

        public int Cleanness => MCleanness;
        public int SocialStatus => MSocialStatus;
        public int Age => MAge;
        //public string UrbanStyle => MUrbanStyle;

        public int Intelligence => MIntelligence;
        public int StealAbility => MStealAbility;
        public int Corruptibility => MCorruptibility;
        public int Daring => MDaring;
        public int Fearful => MLawfulness;
        public int Aggressive => MAggressive;
    
        public float Agility => MAgility;
        public float Speed => MSpeed;
        public float Strength => MStrength;

        public BaseCustomerTypeData()
        {
            InitializeData();
        }

        public void InitializeData()
        {
            Random.InitState(DateTime.Now.Millisecond);
            MCleanness = Random.Range(1, 10);
            Random.InitState(DateTime.Now.Millisecond);
            MSocialStatus = Random.Range(1, 10);
            Random.InitState(DateTime.Now.Millisecond);
            MAge = Random.Range(1, 100);

            MIntelligence = Random.Range(1, 10);
            MStealAbility = Random.Range(1, 10);
            MCorruptibility = Random.Range(1, 10);
            MDaring = Random.Range(1, 10);
            MLawfulness = Random.Range(1, 10);
            MAggressive = Random.Range(1, 10);
            
            MAgility = Random.Range(1, 10);
            MSpeed = Random.Range(1, 10);
            MStrength = Random.Range(1, 10);
            
            Debug.Log($"Cleanness: {Cleanness} ---SocialStatus: {SocialStatus} ---Age: {Age} " +
                      $"---Corruptibility: {Corruptibility} ---Daring: {Daring} ---Lawfulness: {Fearful} " +
                      $"---Aggressive: {Aggressive} ---Agility: {Agility} ---Speed: {Speed} ---Strength: {Strength}");
        }
    }
}