using DataUnits;

namespace Utils
{
    public static class DataSheetUrls
    {
        public static string JobSuppliersGameData =
            "https://sheets.googleapis.com/v4/spreadsheets/1-oli_LmChsRrjEXqW5E8Ajt0V5IuqnY8cx3HilCTBfM/values/JobSuppliers?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";

        public static string ItemSuppliersGameData =
            "https://sheets.googleapis.com/v4/spreadsheets/144vHYkW7Bn86_fJ7DZUEZhUITgDAcocH-FKkivNrpQs/values/ItemSuppliers?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";

        public static string IntroDialoguesGameData =
            "https://sheets.googleapis.com/v4/spreadsheets/1uIxuYdSEPwiogcnTni-PLYJDXAsMST8-H9nKcDrTxTU/values/IntroDialogue_00?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";

        public static string ItemsCatalogueGameData =
            "https://sheets.googleapis.com/v4/spreadsheets/1bli4diwooBNk5K04Cdl1bytCw7QtPfkVTBTi5SiRjGc/values/ItemsCatalogue?key=AIzaSyDkMJ4WemjaSx92OzN7YXs6Hy7RcgHvM4A";

        //https://docs.google.com/spreadsheets/d//edit?usp=sharing
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
                default:
                    return "";
            }
        }
    }
}