using UnityEngine;

namespace LeanTween.Framework
{
    public class LeanAudioStream {

        public int position = 0;

        public AudioClip audioClip;
        public float[] audioArr;

        public LeanAudioStream( float[] audioArr ){
            this.audioArr = audioArr;
        }

        public void OnAudioRead(float[] data) {
            int count = 0;
            while (count < data.Length) {
                data[count] = audioArr[this.position];
                position++;
                count++;
            }
        }

        public void OnAudioSetPosition(int newPosition) {
            this.position = newPosition;   
        }
    }
}