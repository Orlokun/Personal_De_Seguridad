using DataUnits;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace Utils
{
    public static class DataSheetUrls
    {
        /// <summary>
        /// Daily Data
        /// </summary>
        public static string RentValuesData = "https://sheets.googleapis.com/v4/spreadsheets/1yLh7jS95a9ARy1tikX9sdVmKltdwfTv0I09GJNaAAxU/values/RentData?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        public static string FoodValuesData = "https://sheets.googleapis.com/v4/spreadsheets/1wqEySwe--YKJR_neXAiKVJ6A-covdHRBsvgYyd2Dklk/values/FoodData?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        public static string TransportValuesData = "https://sheets.googleapis.com/v4/spreadsheets/13WxlP3SCO85agDBMF_U22nfuQW4zPaW_RhIKIuA1JgQ/values/TransportData?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        /// <summary>
        /// Special Stats
        /// </summary>
        public static string GuardsSpecialStats =
            "https://sheets.googleapis.com/v4/spreadsheets/1CRV2rBLnzGJzjpb3ydAEwLstM9ud6aH66Wc5LZIi5vk/values/Stats?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        public static string CameraSpecialStats =
            "https://sheets.googleapis.com/v4/spreadsheets/1xc4PAQXv78xpLSZAMq9hN-G9R9adEqUoy4sIN0vs2kM/values/Stats?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        public static string WeaponsSpecialStats =
            "https://sheets.googleapis.com/v4/spreadsheets/1Aizb_yaWEXh6D2MznZNzBON-00VtuiOWEfLfhcklPLs/values/Stats?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        public static string TrapsSpecialStats =
            "https://sheets.googleapis.com/v4/spreadsheets/1qMlDbWtwEHplxhFnd0bqaaioCGE2i4O0jzucJyjNmw0/values/Stats?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        public static string OtherItemsSpecialStats =
            "https://sheets.googleapis.com/v4/spreadsheets/1v4pUiKUzvH9pMGTjLUGcXM_aDyH22l2FAPLlRAnzq30/values/Stats?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";

        /// <summary>
        /// Feedback & Dialogues
        /// </summary>
        public static string FeedbacksGameDataUrl = 
            "https://sheets.googleapis.com/v4/spreadsheets/1aahEf2opdO7gwsnFmZJ5nVtpjaAa3O-rgoFqBW9b74Q/values/GeneralFeedbacks?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        public static string IntroDialoguesGameData =
            "https://sheets.googleapis.com/v4/spreadsheets/1uIxuYdSEPwiogcnTni-PLYJDXAsMST8-H9nKcDrTxTU/values/IntroDialogue_00?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        
        /// <summary>
        /// Suppliers & Items
        /// </summary>
        public static string JobSuppliersGameData =
            "https://sheets.googleapis.com/v4/spreadsheets/1-oli_LmChsRrjEXqW5E8Ajt0V5IuqnY8cx3HilCTBfM/values/JobSuppliers?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        public static string ItemSuppliersGameData =
            "https://sheets.googleapis.com/v4/spreadsheets/144vHYkW7Bn86_fJ7DZUEZhUITgDAcocH-FKkivNrpQs/values/ItemSuppliers?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
        public static string ItemsCatalogueGameData =
            "https://sheets.googleapis.com/v4/spreadsheets/1bli4diwooBNk5K04Cdl1bytCw7QtPfkVTBTi5SiRjGc/values/ItemsCatalogue?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";

        public static string SuppliersDialogueGameData(DialogueSpeakerId speakerIndex)
        {
            switch (speakerIndex)
            {
                //ItemSuppliers
                case DialogueSpeakerId.Moncho:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1V_R4X5cjb7Tx_rwfl45KBgsQerAjebZnx4I5ElaTM4I/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.D1TV:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1gK7wsCtyGc12D4j-DhlB2Yten9PqzBptzpsGHAxnm2Q/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.FalaPela:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1rMH634fh2_xv3C0iaIOKLOqifeQVc1rFwftjQH6G7aU/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.CuaticCams:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1ekTiVNaBMtg2OZx4DDIL183JMDsgGAXDjO0zggYghE4/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.MahelEquips:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1XDzU8lnCVPspWrCSUf6dzQubL8PDgebVzzcwMGtR2Ok/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.DataVamps:
                    return "https://sheets.googleapis.com/v4/spreadsheets/12MK-Hk4EjFUXVfoVwGXkEQIldaXbs0XsTTLd5vdbqnE/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.PredatorCo:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1jNJmncbk6YmP5VelPbneSw-26bum2tJ5dwWeOrXdXDA/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.AbismalCorp:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1d8awZDfurx86LQ7hF6g4jghFGsApiEuN4Z2zyoMRWh0/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.ThirdAir:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1oDB9272os7t27Ls_orD-yB61-oDISB0xVfa-ymd01PA/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.MuscleHead:
                    return "https://sheets.googleapis.com/v4/spreadsheets/19hvQ_oGdZx5tBHdhgjYWkZjukZcGI_6IercyEmP-jvo/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.PapoContreras:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1dYTyX65iKa4Old6EPCa5qlREMF021yPPWwmyyQ9K9Zo/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.Cachete:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1gwA5nNVkeIvYaFxv0_tKcVk5X94pwZFIqYiznnp7bd8/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.BushidoCenter:
                    return "https://sheets.googleapis.com/v4/spreadsheets/18siqqLenonq9VQyP13c2XgxCi4ig2LKCricWJvU6yOk/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.Robotiquerie:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1HveLedSw3oJkW9suJE0UKVP6VtYWa8g5w-dlnaRo2Bg/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.Unoccupopes:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1bOucKzJ9ylrO93e6CORD1-nUn5ByVAUMsL_NhwI6m1k/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.RingTitans:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1wUpngrgf3NFJGV4zpsUHDEgvPAVgj5Sobu5Nr5eoOTE/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.ClassicalSecurity:
                    return "https://sheets.googleapis.com/v4/spreadsheets/17vtsi2Hr7bbp4nHwLrHfHqUrhaIUzjrTAZyUQR7xN14/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.MedievalInnovations:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1GibENZ_H1f02x7r9TOP2rQSf8Ud54gPrc4PDWkcTuPI/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                //JobSuppliers
                case DialogueSpeakerId.HappyCopyOfEden:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1HKIw3cAmtAyQgTH5LrJHIKUhVgr0gR7k3k_BuLfY37A/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.XOXO:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1ke8CBMh1juTpBWlAkxC4lr7yTqdLIuVdiWtHK8--h3s/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.Potobras:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1eZz-JI3z7_TUpiYEi2d4ESWp8P-8gDCbE84TWWf48Eg/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.Aristo:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1pJ-cLDtZQerk07bVkqy9Nnd_b64O6F45RGv3bcnkX7k/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.FootNote:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1TRi_fO5lbnQW7H0vGU07jq6jzvfeIQb25R4kD_WK09w/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.LittleOasis:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1syYI7FIqrk2d7v7RDcVb1BAnnE8kX9EOXCFiruO_oyU/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.LiBiDo:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1JVcCmEOE5IBqf81YFXvkSNtjTf5r0NtOsZJEVQTRNxM/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.TheFigs:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1YeRUOGh3OWdFQe8n-33TYKyIHkXUnzI57OsOzP5MhUo/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.Michimalonco:
                    return "https://sheets.googleapis.com/v4/spreadsheets/199aSv6vFntE1dfoTeys2DOPdy2KGOpuC8h9aIm9WrDw/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.JustAnotherFair:
                    return "https://sheets.googleapis.com/v4/spreadsheets/164NNg0XHbAAoXoTZAbk0WVsuMS36p-h_dqt1f1aQj4c/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.Veganerie:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1iC7s2L278Dn7tqvPNYeO3qTW7vlf9dbRqKJ2CGgWGTs/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.JustTakeIt:
                    return "https://sheets.googleapis.com/v4/spreadsheets/15B7MlDWUJbnP11gkTR9bfPMPC1SXcLM6ldrYiKiTejU/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.Zootopia:
                    return "https://sheets.googleapis.com/v4/spreadsheets/19_Ma-mEdT-qukWpl_P0Dx9k3t3eGlp3UclBjjGK6Rdo/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.OmnicorpMuseum:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1tXALGkSj-wab3yuRjtG_aLsiyHRQTXvAedx57w_Iku8/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.LakeMansion:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1Qna5DHgwEbFPCFkRhMFSJoXsaFPaVSNdrFQ_A3tM-QI/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.Bank:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1IcNIdCRRfGtTMYCm5K_-rELfS-ZjgzCLiKQ-ITHJ0cg/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case DialogueSpeakerId.VitaNova:
                    Debug.LogWarning("Vita nova has the same dialogues as bank!");
                    return "https://sheets.googleapis.com/v4/spreadsheets/1IcNIdCRRfGtTMYCm5K_-rELfS-ZjgzCLiKQ-ITHJ0cg/values/Dialogues?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                default:
                    return "";
            }
        }
        
        //Products for Job Suppliers
        public static string GetStoreProducts(BitGameJobSuppliers jobSupplierId)
        {
            Debug.LogWarning("[DataSheetsUrl.StoreProducts] All store products use the same url right now! Must make sure each has its own");
            switch (jobSupplierId)
            {
                //JobSuppliers
                case BitGameJobSuppliers.COPY_OF_EDEN:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.XOXO_MINIMARKET:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.POTOBRAS_GAS_STATION:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.ARISTO_SUPERMARKET:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.FOOTNOTE_LIBRARY:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.LITTLE_OASIS_STRIP_CENTER:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.LIBIDOH_CHINESE_MALL:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.FIGS_CENTRAL_MARKET:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.MICHIMALONCO_BLACK_MARKET:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.MEDIEVAL_FAIR_AND_JUST:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.VEGAN_MARKET:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.JUST_TAKE_IT_MAYORMARKET:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.ZOOTOPIA_EXOTIC_ANIMALS:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.OMNICORP_MUSEUM:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.LAKE_MANSION:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.CBGB_BANK:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                case BitGameJobSuppliers.VITA_NOVA_MALL:
                    return "https://sheets.googleapis.com/v4/spreadsheets/1LucMO2RU7jUoUNim57rMHGE6VPa5d7CTdsWfQ8tzv_s/values/LvlProducts?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";
                default:
                    return "";
            }
        }
    }
}