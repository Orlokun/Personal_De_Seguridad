using UnityEngine;

namespace LeanTween.Framework
{
    /**
* Pass in options to LeanAudio
*
* @class LeanAudioOptions
* @constructor
*/
    public class LeanAudioOptions : object {

        public enum LeanAudioWaveStyle{
            Sine,
            Square,
            Sawtooth,
            Noise
        }

        public LeanAudioWaveStyle waveStyle = LeanAudioWaveStyle.Sine;
        public Vector3[] vibrato;
        public Vector3[] modulation;
        public int frequencyRate = 44100;
        public float waveNoiseScale = 1000;
        public float waveNoiseInfluence = 1f;

        public bool useSetData = true;
        public LeanAudioStream stream;

        public LeanAudioOptions(){}

        /**
	* Set the frequency for the audio is encoded. 44100 is CD quality, but you can usually get away with much lower (or use a lower amount to get a more 8-bit sound).
	* 
	* @method setFrequency
	* @param {int} frequencyRate:int of the frequency you wish to encode the AudioClip at
	* @return {LeanAudioOptions} LeanAudioOptions describing optional values
	* @example
	* AnimationCurve volumeCurve = new AnimationCurve( new Keyframe(0f, 1f, 0f, -1f), new Keyframe(1f, 0f, -1f, 0f));<br>
	* AnimationCurve frequencyCurve = new AnimationCurve( new Keyframe(0f, 0.003f, 0f, 0f), new Keyframe(1f, 0.003f, 0f, 0f));<br>
	* AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato( new Vector3[]{ new Vector3(0.32f,0f,0f)} ).setFrequency(12100) );<br>
	*/
        public LeanAudioOptions setFrequency( int frequencyRate ){
            this.frequencyRate = frequencyRate;
            return this;
        }

        /**
	* Set details about the shape of the curve by adding vibrato modulations through it (alters the peak values giving it a wah-wah effect). You can add as many as you want to sculpt out more detail in the sound wave.
	* 
	* @method setVibrato
	* @param {Vector3[]} vibratoArray:Vector3[] The first value is the period in seconds that you wish to have the vibrato wave fluctuate at. The second value is the minimum height you wish the vibrato wave to dip down to (default is zero). The third is reserved for future effects.
	* @return {LeanAudioOptions} LeanAudioOptions describing optional values
	* @example
	* AnimationCurve volumeCurve = new AnimationCurve( new Keyframe(0f, 1f, 0f, -1f), new Keyframe(1f, 0f, -1f, 0f));<br>
	* AnimationCurve frequencyCurve = new AnimationCurve( new Keyframe(0f, 0.003f, 0f, 0f), new Keyframe(1f, 0.003f, 0f, 0f));<br>
	* AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato( new Vector3[]{ new Vector3(0.32f,0.3f,0f)} ).setFrequency(12100) );<br>
	*/
        public LeanAudioOptions setVibrato( Vector3[] vibrato ){
            this.vibrato = vibrato;
            return this;
        }

        /*
	public LeanAudioOptions setModulation( Vector3[] modulation ){
		this.modulation = modulation;
		return this;
	}*/

        public LeanAudioOptions setWaveSine(){
            this.waveStyle = LeanAudioWaveStyle.Sine;
            return this;
        }

        public LeanAudioOptions setWaveSquare(){
            this.waveStyle = LeanAudioWaveStyle.Square;
            return this;
        }

        public LeanAudioOptions setWaveSawtooth(){
            this.waveStyle = LeanAudioWaveStyle.Sawtooth;
            return this;
        }

        public LeanAudioOptions setWaveNoise(){
            this.waveStyle = LeanAudioWaveStyle.Noise;
            return this;
        }

        public LeanAudioOptions setWaveStyle( LeanAudioWaveStyle style ){
            this.waveStyle = style;
            return this;
        }


        public LeanAudioOptions setWaveNoiseScale( float waveScale ){
            this.waveNoiseScale = waveScale;
            return this;
        }

        public LeanAudioOptions setWaveNoiseInfluence( float influence ){
            this.waveNoiseInfluence = influence;
            return this;
        }
    }
}