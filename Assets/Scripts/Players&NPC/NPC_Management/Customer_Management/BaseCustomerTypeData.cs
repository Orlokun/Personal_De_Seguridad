using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Players_NPC.NPC_Management.Customer_Management
{
    public interface ICustomerTypeData
    {
        /// <summary>
        /// Fields
        /// </summary>
        public int Cleanness { get; }
        public int SocialStatus { get; }
        public int Age{ get; }
        //public string UrbanStyle{ get; }  //TODO: Evaluate Later
    
        public int Corruptibility { get; }
        public int Daring { get; }
        public int Lawfulness { get; }
        public int Aggressive { get; }
    
        public float Agility { get; }
        public float Speed { get; }
        public float Strength { get; }

        //Functions
        public void InitializeData();

    }

    public class BaseCustomerTypeData : ICustomerTypeData
    {
        #region Protected Members
        //Social params
        protected int MCleanness;
        protected int MSocialStatus;
        protected int MAge;
        //protected string MUrbanStyle;       //TODO: Evaluate later
        
        //Psychological params
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
    
        public int Corruptibility => MCorruptibility;
        public int Daring => MDaring;
        public int Lawfulness => MLawfulness;
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
            
            
            MCorruptibility = Random.Range(1, 10);
            MDaring = Random.Range(1, 10);
            MLawfulness = Random.Range(1, 10);
            MAggressive = Random.Range(1, 10);
            
            MAgility = Random.Range(1, 10);
            MSpeed = Random.Range(1, 10);
            MStrength = Random.Range(1, 10);
            Debug.Log($"Cleanness: {Cleanness} ---SocialStatus: {SocialStatus} ---Age: {Age} " +
                      $"---Corruptibility: {Corruptibility} ---Daring: {Daring} ---Lawfulness: {Lawfulness} " +
                      $"---Aggressive: {Aggressive} ---Agility: {Agility} ---Speed: {Speed} ---Strength: {Strength}");
        }
    }
}