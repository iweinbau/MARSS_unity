using HoloScan.Runtime;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
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

    [SerializeField] private GPTUIController gptController;

    private bool shouldCreateViewItem;
    private bool shouldCreateVirtualViewItem;

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
        holoScanApI.OnMessageReceived.AddListener(OnTextReceived);
        holoScanApI.OnMessageReceived.AddListener(OnSaveObjectLocation);
        holoScanApI.OnMessageGPTReceived.AddListener(OnGPTTextReceived);
        storage.OnVisitItemProgress += uiController.ShowReachedVisitObject;
    }

    private void OnDisable()
    {
        holoScanApI.OnMessageReceived.RemoveListener(OnTextReceived);
        holoScanApI.OnMessageReceived.RemoveListener(OnSaveObjectLocation);
        holoScanApI.OnMessageGPTReceived.RemoveListener(OnGPTTextReceived);
        storage.OnVisitItemProgress -= uiController.ShowReachedVisitObject;
    }

    public void OnSaveObjectLocation(string key)
    {
        if (!shouldCreateViewItem)
            return;

        Debug.Log(key);

        if (!shouldCreateVirtualViewItem)
            storage.SavePosition(key, cameraTransform.position);

        else
            storage.AddVirtualObject(key, cameraTransform.position + cameraTransform.forward * 0.5f);

        shouldCreateViewItem = false;
        shouldCreateVirtualViewItem = false;
    }

    public void OnTextReceived(string key)
    {
        if (shouldCreateViewItem)
            return;

        gptController.AddUserPrompt(key);
        holoScanApI.PublishString(key);
    }

    public void OnGPTTextReceived(string message)
    {
        gptController.AddGPTPrompt(message);
    }
    
    public void OnStartRecordingLocation()
    {
        recorder.StartRecording();
    }
    public void OnStopRecordingLocation()
    {
        recorder.SaveRecording();
        shouldCreateViewItem = true;
        shouldCreateVirtualViewItem = false;

    }
    public void OnStartVirtualRecordingLocation()
    {
        recorder.StartRecording();
    }
    public void OnStopVirtualRecordingLocation()
    {
        recorder.SaveRecording();
        shouldCreateViewItem = true;
        shouldCreateVirtualViewItem = true;
    }

    public void OnStartRecording()
    {
        recorder.StartRecording();
    }
    public void OnStopRecording()
    {
        recorder.SaveRecording();
    }

    public void CreateMemoryBridge()
    {
        string gptPrompt = $"Make a list of these and all previous objects and it's locations: ";
        string[] keys = storage.GetKeys();
        for (int i = 0; i < keys.Length; i++)
        {
            if (i == 0)
                gptPrompt += $"\"{keys[i]}\"";
            else
                gptPrompt += $", \"{keys[i]}\"";

        }
        gptPrompt += ". Then tell me a mnemonic to help me remember these objects and its locations.";
        holoScanApI.PublishString(gptPrompt);
    }

    public void OnReplay()
    {
        Debug.Log("Game started");
        storage.OnReplayStarted();
    }
}
