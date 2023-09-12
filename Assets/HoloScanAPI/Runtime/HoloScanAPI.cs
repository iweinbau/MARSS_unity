using RosMessageTypes.Std;
using System;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.Events;

public class HoloScanAPI : MonoBehaviour
{
    [Header("Ros Keys")]
    [SerializeField] string textKey = "text";
    [SerializeField] string audioKey = "audio";

    
    private ROSConnection rosNode;

    [Header("Event handlers")]
    public UnityEvent<string> OnMessageReceived;

    // Start is called before the first frame update
    void Awake()
    {
        // start the ROS connection
        rosNode = ROSConnection.GetOrCreateInstance();

        // subscribe ultrasound images from Clara AGX
        //rosNode.Subscribe<ImageMsg>(imageTopic, displayImage);
        rosNode.Subscribe<StringMsg>(textKey, OnMessageReceivedUnpack);

        // publish audios recorded by HoloLens2 to /audio, Clara AGX will fetch data from this topic
        rosNode.RegisterPublisher<RosMessageTypes.Std.ByteMultiArrayMsg>(audioKey);
    }

    public void PublishBytes(byte[] data)
    {
        sbyte[] sdata = (sbyte[])(Array)data;
        ByteMultiArrayMsg msg = new ByteMultiArrayMsg(new MultiArrayLayoutMsg(), sdata);
        rosNode.Publish("audio", msg);
        Debug.Log($"publish a bytearray of size: {data.Length}");
    }

    private void OnMessageReceivedUnpack(StringMsg msg)
    {
        OnMessageReceived?.Invoke(msg.data);
    }

}
