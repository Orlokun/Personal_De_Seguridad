using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DataUnits.JobSources;
using GamePlayManagement.BitDescriptions.Suppliers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DataUnits.GameCatalogues
{
    public interface IBaseJobsCatalogue
    {
        bool JobSupplierExists(JobSupplierBitId jobSupplier);
        IJobSupplierObject GetJobSupplierObject(JobSupplierBitId jobSupplier);
        List<IJobSupplierObject> JobSuppliersInData { get; }
        public Tuple<bool, int> JobSupplierPhoneNumberExists(int phoneDialed);

    }

    public class BaseJobsCatalogue : MonoBehaviour, IBaseJobsCatalogue, IInitialize
    {
        //Singleton magament
        private static BaseJobsCatalogue _instance;
        public static IBaseJobsCatalogue Instance => _instance;
        //Catalogue loaded and parsed from server
        private JobsCatalogueFromData _jobsData;
        //Interfaces extracted from loaded data
        private List<IJobSupplierObject> _mIjobSuppliersInData;
        public List<IJobSupplierObject> JobSuppliersInData => _mIjobSuppliersInData;
        private bool _mInitialized;
        public bool IsInitialized => _mInitialized;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Initialize();
            GetJobSuppliersData();
        }

        public void Initialize()
        {
            if (_instance != null)
            {
                Destroy(this);
            }
            if (_mInitialized)
            {
                return;
            }
            _mInitialized = true;
            _instance = this;
        }

        private void GetJobSuppliersData()
        {
            Debug.Log($"START: COLLECTING JOB SOURCES DATA");
            var url = DataSheetUrls.JobSuppliersGameData;
            StartCoroutine(GetJobsCatalogueData(url));
        }

        private IEnumerator GetJobsCatalogueData(string url)
        {
            //
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Jobs Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadJobSuppliersFromJson(sourceJson);
            }
        }

        private void LoadJobSuppliersFromJson(string sourceJson)
        {
            Debug.Log($"BaseJobsCatalogue.LoadJobSuppliersFromJson");
            _jobsData = JsonConvert.DeserializeObject<JobsCatalogueFromData>(sourceJson);
            Debug.Log($"Finished parsing. Is _jobsCatalogueFromData null?: {_jobsData == null}");
            _mIjobSuppliersInData = new List<IJobSupplierObject>();
            for (var i = 1; i < _jobsData.values.Count;i++)
            {
                var jobSupplier = (IJobSupplierObject)ScriptableObject.CreateInstance<JobSupplierObject>();
                    
                int jobId;
                var gotId = int.TryParse(_jobsData.values[i][0], out jobId);
                jobSupplier.JobSupplierBitId = (JobSupplierBitId) jobId;
                    
                jobSupplier.StoreType = _jobsData.values[i][1];
                jobSupplier.StoreName = _jobsData.values[i][2];
                jobSupplier.StoreOwnerName = _jobsData.values[i][3];
                    
                int unlockPoints;
                var gotUp = int.TryParse(_jobsData.values[i][4], out unlockPoints);
                if (!gotUp)
                {
                    Debug.LogWarning("GetJobsCatalogueData");
                }
                jobSupplier.StoreUnlockPoints = unlockPoints;
                jobSupplier.StoreDescription = _jobsData.values[i][5];
                   
                var gotPhone = int.TryParse(_jobsData.values[i][6], out var phoneNumber);
                jobSupplier.StorePhoneNumber = phoneNumber;
                
                var gotSpeakerId = int.TryParse(_jobsData.values[i][7], out var speakerIndex);
                jobSupplier.SpeakerIndex = (DialogueSpeakerId)speakerIndex;
                
                //Minimum and maximum clients in store
                var gotClientsMin = int.TryParse(_jobsData.values[i][8], out var clientsMin);
                var gotClientsMax = int.TryParse(_jobsData.values[i][9], out var clientsMax);
                var clientRange = new int[2];
                clientRange[0] = clientsMin;
                clientRange[1] = clientsMax;
                jobSupplier.StoreMinMaxClients = clientRange;
                
                var gotSanity = int.TryParse(_jobsData.values[i][10], out var sanity);
                var gotKindness = int.TryParse(_jobsData.values[i][11], out var kindness);
                var gotViolence = int.TryParse(_jobsData.values[i][12], out var violence);
                var gotIntelligence = int.TryParse(_jobsData.values[i][13], out var intelligence);
                var gotMoney = int.TryParse(_jobsData.values[i][14], out var money);
                
                if (!gotSanity || !gotKindness || !gotViolence || !gotIntelligence || !gotMoney)
                {
                    Debug.LogError("Job Supplier Stats must be available!");
                }
                else
                {
                    jobSupplier.SetStats(sanity, kindness, violence, intelligence, money);
                }
                var spriteName = _jobsData.values[i][15];
                jobSupplier.SpriteName = spriteName;
                
                var gotOwnerAge = int.TryParse(_jobsData.values[i][16], out var ownerAge);
                jobSupplier.StoreOwnerAge = ownerAge;

                var initialBudget = int.TryParse(_jobsData.values[i][17], out var initBudget);
                jobSupplier.Budget = initBudget;
                _mIjobSuppliersInData.Add(jobSupplier);
            }
        }
        
        /// <summary>
        /// Do Web request
        /// </summary>
        /// <param name="jobSupplier"></param>
        /// <returns></returns>
        public bool JobSupplierExists(JobSupplierBitId jobSupplier)
        {
            return _mIjobSuppliersInData.Any(x => x.JobSupplierBitId == jobSupplier);
        }

        public IJobSupplierObject GetJobSupplierObject(JobSupplierBitId jobSupplier)
        {
            return _mIjobSuppliersInData.SingleOrDefault(x => x.JobSupplierBitId == jobSupplier);
        }
        public Tuple<bool, int> JobSupplierPhoneNumberExists(int phoneDialed)
        {
            var anyNumberMatches = _mIjobSuppliersInData.Any(x => x.StorePhoneNumber == phoneDialed);
            if (!anyNumberMatches)
            {
                return new Tuple<bool, int>(false, 0);
            }
            var supplierId = (int)_mIjobSuppliersInData.SingleOrDefault(x => x.StorePhoneNumber == phoneDialed).JobSupplierBitId;
            return new Tuple<bool, int>(true, supplierId);
        }
    }
}