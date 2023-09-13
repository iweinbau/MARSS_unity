using RosMessageTypes.Std;
using System;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.Events;

namespace HoloScan.Runtime
{
    public class HoloScanAPI : MonoBehaviour
    {
        [Header("Ros Keys")]
        [SerializeField] string textKey = "text";
        [SerializeField] string gptKey = "gpttext";
        [SerializeField] string gptoutputKey = "gptoutput";
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
            rosNode.Subscribe<StringMsg>(textKey, OnMessageReceivedUnpack);
            rosNode.Subscribe<StringMsg>(gptoutputKey, OnGPTMessageReceivedUnpack);

            // publish audios recorded by HoloLens2 to /audio, Clara AGX will fetch data from this topic
            rosNode.RegisterPublisher<ByteMultiArrayMsg>(audioKey);
            rosNode.RegisterPublisher<StringMsg>(gptKey);
        }

        public void PublishBytes(byte[] data)
        {
            sbyte[] sdata = (sbyte[])(Array)data;
            ByteMultiArrayMsg msg = new ByteMultiArrayMsg(new MultiArrayLayoutMsg(), sdata);
            rosNode.Publish(audioKey, msg);
            Debug.Log($"publish a bytearray of size: {data.Length}");
        }

        public void PublishString(string msg)
        {
            StringMsg msgObj = new StringMsg(msg);
            rosNode.Publish(gptKey, msgObj);
        }

        private void OnMessageReceivedUnpack(StringMsg msg)
        {
            OnMessageReceived?.Invoke(msg.data);
            PublishString(msg.data);
        }
        
        private void OnGPTMessageReceivedUnpack(StringMsg msg)
        {
            Debug.Log(msg.data);
            //OnMessageReceived?.Invoke(msg.data);
        }
    }
}
