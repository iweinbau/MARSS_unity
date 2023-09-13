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

    [SerializeField] private LocationStorageUIController uiController;
    
    private Recorder recorder;
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
        storage.OnVisitItemProgress += uiController.ShowReachedVisitObject;
    }

    private void OnDisable()
    {
        holoScanApI.OnMessageReceived.RemoveListener(OnSaveObjectLocation);
        storage.OnVisitItemProgress -= uiController.ShowReachedVisitObject;
    }

    public void OnSaveObjectLocation(string key)
    {
        Debug.Log(key);
        storage.SavePosition(key, cameraTransform.position);
    }

    public void OnVirtualObjectLocation()
    {
        Debug.Log("Virtual object stored");
        storage.AddVirtualObject(cameraTransform.position + cameraTransform.forward * 0.5f);
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
        Debug.Log("Game started");
        storage.OnReplayStarted();
    }
}
