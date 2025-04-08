using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.ComplianceSystem;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace GameDirection.ComplianceDataManagement
{
    public class ComplianceManagerData : IComplianceManagerData
    {
        private Dictionary<int, IComplianceObject> _mLoadedBaseCompliance = new Dictionary<int, IComplianceObject>();
        
        private List<IComplianceObject> _mActiveComplianceList = new List<IComplianceObject>();
        private List<IComplianceObject> _mPassedCompliance = new List<IComplianceObject>();
        private List<IComplianceObject> _mFailedComplianceList = new List<IComplianceObject>();

        public List<IComplianceObject> GetPassedComplianceObjects => _mPassedCompliance;  
        public List<IComplianceObject> GetFailedComplianceObjects { get; set; }
        
        public void ChangeComplianceStatus(ComplianceStatus newStatus, int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateActiveCompliance()
        {
            var newFailedCompliance = _mActiveComplianceList.Where(x => x.GetComplianceObjectData.ComplianceStatus == ComplianceStatus.Failed).ToList();
            var newPassedCompliance = _mActiveComplianceList.Where(x => x.GetComplianceObjectData.ComplianceStatus == ComplianceStatus.Passed).ToList();
            
            _mFailedComplianceList.AddRange(newFailedCompliance);
            _mPassedCompliance.AddRange(newPassedCompliance);

            foreach (var failedCompliance in newFailedCompliance)
            {
                _mActiveComplianceList.Remove(failedCompliance);
            }
            
            foreach (var failedCompliance in newPassedCompliance)
            {
                _mActiveComplianceList.Remove(failedCompliance);
            }
        }

        public void CleanComplianceData()
        {
            _mActiveComplianceList.Clear();
            _mPassedCompliance.Clear();
            _mFailedComplianceList.Clear();
        }


        public List<IComplianceObject> GetActiveComplianceObjects => _mActiveComplianceList;
        
        
        public void StartDayComplianceObjects(DayBitId dayBitId)
        {
            //Step 0: Clear Active Compliance List
            _mActiveComplianceList.Clear();
            //Step 1: Find all Objects Within the Day that are locked and have no unlock condition. 
            var lockedObjects = _mLoadedBaseCompliance.Values.Where(x=> 
                x.GetComplianceObjectData.StartDayId <= dayBitId 
                && x.GetComplianceObjectData.EndDayId >= dayBitId 
                && x.GetComplianceObjectData.NeedsUnlock == false).ToList();
            //Step 2: Unlock all objects that have no unlock condition.
            foreach (var lockedObject in lockedObjects)
            {
                if (lockedObject.GetComplianceObjectData.ComplianceStatus == ComplianceStatus.Locked && lockedObject.GetComplianceObjectData.NeedsUnlock == false)
                {
                    lockedObject.MarkAsActive();
                }
            }
            _mActiveComplianceList = _mLoadedBaseCompliance.Values.Where(x=> 
                x.GetComplianceObjectData.StartDayId <= dayBitId 
                && x.GetComplianceObjectData.EndDayId >= dayBitId 
                && x.GetComplianceObjectData.ComplianceStatus == ComplianceStatus.Active).ToList();
        }

        #region Init
        public void LoadComplianceData()
        {
            Debug.Log($"START: Download Base Compliance data process");
            var url = DataSheetUrls.GetBaseComplianceDataUrl;
            GameDirector.Instance.ActCoroutine(GetBaseComplianceFromServer(url));
        }
        private IEnumerator GetBaseComplianceFromServer(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("GetDayNewsFromServer: Error: " + webRequest.error);
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadComplianceBaseData(sourceJson);
            }
        }
        private void LoadComplianceBaseData(string sourceJson)
        {
            Debug.Log("ComplianceManagerData.LoadComplianceBaseData: Begin request");
            var complianceData = JsonConvert.DeserializeObject<ComplianceCatalogueData>(sourceJson);
            Debug.Log($"Finished parsing. Is Compliance data null?: {complianceData == null}");
            for (var i = 1; i < complianceData.values.Count;i++)
            {
                var hasId = int.TryParse(complianceData.values[i][0], out var complianceId);
                var hasStartDay = Enum.TryParse(complianceData.values[i][1], out DayBitId startDayId);
                var hasEndDay = Enum.TryParse(complianceData.values[i][2], out DayBitId endDayId);
                
                var complianceTitle = complianceData.values[i][3];
                var complianceSubtitle = complianceData.values[i][4];
                var complianceDescription = complianceData.values[i][5];
                
                var unlockIntValue = int.Parse(complianceData.values[i][6]);
                var needsUnlock = unlockIntValue > 0;
                
                var hasMotivationalLvl = Enum.TryParse(complianceData.values[i][7],
                    out ComplianceMotivationalLevel motivationLvl);
                var hasComplianceType = Enum.TryParse(complianceData.values[i][8],
                    out ComplianceActionType actionType);
                var hasComplianceObjectType = Enum.TryParse(complianceData.values[i][9],
                    out ComplianceObjectType objectType);
                var hasConsideredParameter = Enum.TryParse(complianceData.values[i][10],
                    out RequirementConsideredParameter consideredParameter);
                
                var requirementRawValues = complianceData.values[i][11].Split(',');
                var hasToleranceValue = int.TryParse(complianceData.values[i][12], out var toleranceValue);
                var rewardValues = complianceData.values[i][13].Split('|');
                var punishmentValues = complianceData.values[i][14].Split('|');
                var hasReqLogic = Enum.TryParse(complianceData.values[i][15], out RequirementLogicEvaluator complianceLogic);

                
                if (!hasId || !hasStartDay || !hasEndDay || !hasMotivationalLvl || !hasComplianceType || !hasComplianceObjectType || 
                    !hasConsideredParameter || !hasToleranceValue || !hasReqLogic)
                {
                    Debug.LogWarning($"[LoadComplianceBaseData] Compliance Object with id {i} has missing values.");
                    continue;
                }
                var complianceObject = Factory.CreateComplianceObject(complianceId, startDayId, endDayId, needsUnlock,
                    motivationLvl, actionType, objectType, consideredParameter, requirementRawValues,toleranceValue, rewardValues, punishmentValues, complianceTitle,complianceSubtitle ,complianceDescription, complianceLogic);                
                _mLoadedBaseCompliance.Add(complianceId, complianceObject);
            }
            Debug.Log("ComplianceManagerData.LoadComplianceBaseData: Finished request");
        }
        #endregion



    }
}