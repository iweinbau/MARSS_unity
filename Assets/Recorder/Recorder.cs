using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Recorder
{
    /// <summary>
    /// Add this component to a GameObject to Record Mic Input 
    /// </summary>
    [AddComponentMenu("AhmedSchrute/Recorder")]
    [RequireComponent(typeof(AudioSource))]
    public class Recorder : MonoBehaviour
    {
        #region Constants &  Static Variables
        /// <summary>
        /// Audio Source to store Microphone Input, An AudioSource Component is required by default
        /// </summary>
        static AudioSource audioSource;
        /// <summary>
        /// The samples are floats ranging from -1.0f to 1.0f, representing the data in the audio clip
        /// </summary>
        static float[] samplesData;

        #endregion

        #region Editor Exposed Variables

        /// <summary>
        /// Set a keyboard key for saving the Audio File
        /// </summary>
        [Tooltip("Set a keyboard key for saving the Audio File")]
        public KeyCode saveKey, startKey;

        private float startRecordingTime;
        [SerializeField] private HoloScanAPI holoScanApI;
        #endregion

        void Start()
        {
            holoScanApI = GameObject.Find("HoloScanAPI").GetComponent<HoloScanAPI>();
            holoScanApI.OnMessageReceived.AddListener(LogMessage);
            audioSource = GetComponent<AudioSource>();
        }

        private void LogMessage(string msg)
        {
            Debug.Log(msg);
        }

        private void Update()
        {
            if (Input.GetKeyDown(startKey))
            {
                StartRecording();
            }

            if (Input.GetKeyDown(saveKey))
            {
                Save();
            }
        }

        public void StartRecording()
        {
            Debug.Log("Pressed start!!!!!!!!!!!!!!!!!");
            startRecordingTime = Time.time;
            audioSource.clip = Microphone.Start(Microphone.devices[0], true, 300, 44100);
        }

        public void Save()
        {


            while (!(Microphone.GetPosition(null) > 0)) { }
            Microphone.End(Microphone.devices[0]);
            AudioClip trimedClip = AudioClip.Create(audioSource.clip.name, (int)((Time.time - startRecordingTime) * audioSource.clip.frequency), audioSource.clip.channels, audioSource.clip.frequency, false);
            float[] data = new float[(int)((Time.time - startRecordingTime) * audioSource.clip.frequency)];
            audioSource.clip.GetData(data, 0);
            trimedClip.SetData(data, 0);
            audioSource.clip = trimedClip;

            byte[] bytes = AudioConverterHoloScan.ConvertAudioToByteArray(audioSource);
            holoScanApI.PublishBytes(bytes);
        }
    }
}