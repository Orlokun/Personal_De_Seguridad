using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DataUnits.JobSources;
using DialogueSystem;
using GamePlayManagement.BitDescriptions.Suppliers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;
using DataUnits.GameCatalogues.JsonModels;

namespace DataUnits.GameCatalogues
{
    public interface IBaseJobsCatalogue
    {
        bool JobSupplierExists(JobSupplierBitId jobSupplier);
        IJobSupplierObject GetJobSupplierObject(JobSupplierBitId jobSupplier);
        List<IJobSupplierObject> JobSuppliersInData { get; }
        public Tuple<bool, int> JobSupplierPhoneNumberExists(int phoneDialed);
        public IJobSupplierObject GetJobSupplierSpeakData(DialogueSpeakerId speakerId);
    }

    public class BaseJobsCatalogue : MonoBehaviour, IBaseJobsCatalogue, IInitialize
    {
        //Singleton magament
        private static BaseJobsCatalogue instance;
        public static IBaseJobsCatalogue Instance => instance;
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
            if (instance != null)
            {
                Destroy(this);
            }
            if (_mInitialized)
            {
                return;
            }
            _mInitialized = true;
            instance = this;
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
            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Jobs Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadJobSuppliersFromJson(sourceJson);
            }
        }

        private IJobSupplierObject CreateJobSupplierObject(JobSupplierBitId jobId)
        {
            switch (jobId)
            {
                case JobSupplierBitId.COPY_OF_EDEN:
                    return ScriptableObject.CreateInstance<CountPetrolkSupplierObject>();
                default:
                    return ScriptableObject.CreateInstance<JobSupplierObject>();
            }
        }

        private void LoadJobSuppliersFromJson(string sourceJson)
        {
            var jobsData = JsonConvert.DeserializeObject<JobsCatalogueFromData>(sourceJson);
            _mIjobSuppliersInData = new List<IJobSupplierObject>();

            foreach (var jobData in jobsData.Values)
            {
                var jobSupplier = CreateJobSupplierObject((JobSupplierBitId)jobData.JobSupplierBitId);
                jobSupplier.LocalInitialize((JobSupplierBitId)jobData.JobSupplierBitId);

                jobSupplier.JobSupplierData.JobSupplierBitId = (JobSupplierBitId)jobData.JobSupplierBitId;
                jobSupplier.JobSupplierData.StoreType = jobData.StoreType;
                jobSupplier.StoreName = jobData.StoreName;
                jobSupplier.StoreOwnerName = jobData.StoreOwnerName;
                jobSupplier.JobSupplierData.StoreUnlockPoints = jobData.StoreUnlockPoints;
                jobSupplier.JobSupplierData.StoreDescription = jobData.StoreDescription;
                jobSupplier.StorePhoneNumber = jobData.StorePhoneNumber;
                jobSupplier.SpeakerIndex = (DialogueSpeakerId)jobData.SpeakerIndex;
                jobSupplier.JobSupplierData.StoreMinMaxClients = jobData.StoreMinMaxClients;
                jobSupplier.SetStats(jobData.Sanity, jobData.Kindness, jobData.Violence, jobData.Intelligence, jobData.Money);
                jobSupplier.JobSupplierData.SpriteName = jobData.SpriteName;
                jobSupplier.JobSupplierData.StoreOwnerAge = jobData.StoreOwnerAge;
                jobSupplier.JobSupplierData.Budget = jobData.Budget;

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
            return _mIjobSuppliersInData.Any(x => x.JobSupplierData.JobSupplierBitId == jobSupplier);
        }

        public IJobSupplierObject GetJobSupplierObject(JobSupplierBitId jobSupplier)
        {
            return _mIjobSuppliersInData.SingleOrDefault(x => x.JobSupplierData.JobSupplierBitId == jobSupplier);
        }
        public Tuple<bool, int> JobSupplierPhoneNumberExists(int phoneDialed)
        {
            var anyNumberMatches = _mIjobSuppliersInData.Any(x => x.StorePhoneNumber == phoneDialed);
            if (!anyNumberMatches)
            {
                return new Tuple<bool, int>(false, 0);
            }
            var supplierId = (int)_mIjobSuppliersInData.SingleOrDefault(x => x.StorePhoneNumber == phoneDialed).JobSupplierData.JobSupplierBitId;
            return new Tuple<bool, int>(true, supplierId);
        }

        public IJobSupplierObject GetJobSupplierSpeakData(DialogueSpeakerId speakerId)
        {
            return _mIjobSuppliersInData.SingleOrDefault(x => x.SpeakerIndex == speakerId);
        }
    }
}