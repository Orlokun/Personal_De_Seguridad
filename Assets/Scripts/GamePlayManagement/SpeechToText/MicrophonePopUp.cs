using System.Diagnostics;
using System.Text.RegularExpressions;
using TMPro;
using UI.PopUpManager;
using UnityEngine;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

namespace GamePlayManagement.SpeechToText
{
    /// <summary>
    /// Record audio clip from microphone and make a transcription.
    /// </summary>
    public class MicrophonePopUp : PopUpObject
    {
        private ISpeechToTextOperator mSpeechOperator;
        
        
        public WhisperManager whisper;
        public MicrophoneRecord microphoneRecord;
        public bool streamSegments = true;
        public bool printLanguage = true;

        [Header("UI")] 
        public Button button;
        public TMP_Text buttonText;
        public TMP_Text outputText;
        public TMP_Text timeText;
        public Dropdown languageDropdown;
        
        private string _buffer;

        private void Awake()
        {
            mSpeechOperator = new SpeechToTextOperator();
            whisper.OnNewSegment += OnNewSegment;
            whisper.OnProgress += OnProgressHandler;
            
            microphoneRecord.OnRecordStop += OnRecordStop;
            
            button.onClick.AddListener(OnButtonPressed);
            /*languageDropdown.value = languageDropdown.options
                .FindIndex(op => op.text == whisper.language);
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            */
            //translateToggle.isOn = whisper.translateToEnglish;
            //translateToggle.onValueChanged.AddListener(OnTranslateChanged);

        }

        private void OnVadChanged(bool vadStop)
        {
            microphoneRecord.vadStop = vadStop;
        }

        private void OnButtonPressed()
        {
            if (!microphoneRecord.IsRecording)
            {
                buttonText.text = "Stop";
                microphoneRecord.StartRecord();
            }
            else
            {
                microphoneRecord.StopRecord();
            }
        }
        
        private async void OnRecordStop(AudioChunk recordedAudio)
        {
            _buffer = "";

            var sw = new Stopwatch();
            sw.Start();
            
            var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
            if (res == null || !outputText) 
                return;

            var time = sw.ElapsedMilliseconds;
            var rate = recordedAudio.Length / (time * 0.001f);
            timeText.text = $"Time: {time} ms\nRate: {rate:F1}x";
            var charsToTrimm = new[] { ',', '.', '?', '!', ' '};
            var text = res.Result;
            var trimmedText = text.Trim(charsToTrimm);
            var loweredCase = trimmedText.ToLower();
            var cleanedText = Regex.Replace(loweredCase, @"[,.?!]", ""); // Removes all specified characters
            
            if (printLanguage)
                cleanedText += $"\n\nLanguage: {res.Language}";
            ProcessVoiceMessageFromPlayer(cleanedText);
            outputText.text = cleanedText;
            buttonText.text = "Record";
        }

        private void ProcessVoiceMessageFromPlayer(string whisperResult)
        {
            mSpeechOperator.ReceiveTextFromVoice(whisperResult);
        }
        
        private void OnLanguageChanged(int ind)
        {
            var opt = languageDropdown.options[ind];
            whisper.language = opt.text;
        }
        
        private void OnTranslateChanged(bool translate)
        {
            whisper.translateToEnglish = translate;
        }

        private void OnProgressHandler(int progress)
        {
            if (!timeText)
                return;
            timeText.text = $"Progress: {progress}%";
        }
        
        private void OnNewSegment(WhisperSegment segment)
        {
            if (!streamSegments || !outputText)
                return;

            _buffer += segment.Text;
            outputText.text = _buffer + "...";
        }
    }
}