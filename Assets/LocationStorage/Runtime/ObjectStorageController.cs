using HoloScan.Runtime;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectStorageController : MonoBehaviour
{
    // Camera object reference
    private Transform cameraTransform;
    [SerializeField] private ObjectStorage storage;

    [SerializeField] private HoloScanAPI holoScanApI;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Recorder recorder;
    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        holoScanApI = GameObject.Find("HoloScanAPI").GetComponent<HoloScanAPI>();
        audioSource = GetComponent<AudioSource>();
        recorder = new Recorder(audioSource, holoScanApI);
    }

    private void OnEnable()
    {
        holoScanApI.OnMessageReceived.AddListener(OnSaveObjectLocation);
    }

    private void OnDisable()
    {
        holoScanApI.OnMessageReceived.RemoveListener(OnSaveObjectLocation);
    }

    public void OnSaveObjectLocation(string key)
    {
        Debug.Log(key);
        storage.SavePosition(key, cameraTransform.position);
    }
    public void OnStartRecording()
    {
        recorder.StartRecording();
    }
    public void OnStopRecording()
    {
        recorder.SaveRecording();
    }

    public void OnReplay()
    {
        storage.OnReplayStarted();
    }
}
