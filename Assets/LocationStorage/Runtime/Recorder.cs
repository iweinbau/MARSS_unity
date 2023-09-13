using HoloScan.Runtime;
using UnityEngine;

/// <summary>
/// Add this component to a GameObject to Record Mic Input 
/// </summary>
public class Recorder
{
    /// <summary>
    /// Audio Source to store Microphone Input, An AudioSource Component is required by default
    /// </summary>
    private AudioSource audioSource;

    private float startRecordingTime;
    [SerializeField] private HoloScanAPI holoScanApi;

    public Recorder(AudioSource audioSource, HoloScanAPI holoScanApI)
    {
        this.audioSource = audioSource;
        this.holoScanApi = holoScanApI;
    }

    public void StartRecording()
    {
        Debug.Log("Pressed start!!!!!!!!!!!!!!!!!");
        startRecordingTime = Time.time;
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 300, 44100);
    }

    public void SaveRecording()
    {
        while (!(Microphone.GetPosition(null) > 0)) { }
        Microphone.End(Microphone.devices[0]);
        AudioClip trimedClip = AudioClip.Create(audioSource.clip.name, (int)((Time.time - startRecordingTime) * audioSource.clip.frequency), audioSource.clip.channels, audioSource.clip.frequency, false);
        float[] data = new float[(int)((Time.time - startRecordingTime) * audioSource.clip.frequency)];
        audioSource.clip.GetData(data, 0);
        trimedClip.SetData(data, 0);
        audioSource.clip = trimedClip;

        byte[] bytes = AudioConverterHoloScan.ConvertAudioToByteArray(audioSource);
        holoScanApi.PublishBytes(bytes);
    }
}