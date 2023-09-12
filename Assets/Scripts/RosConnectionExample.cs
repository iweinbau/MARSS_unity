using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.UI;

public class RosConnectionExample : MonoBehaviour
{

    // WebCamTexture tex;
    Texture2D texRos;
    public RawImage display;
    ROSConnection rosNode;
    string imageTopic = "US_images";
    string audioTextTopic = "text";
    void Start()
    {
        // start the ROS connection
        rosNode = ROSConnection.GetOrCreateInstance();


        // subscribe ultrasound images from Clara AGX
        //rosNode.Subscribe<ImageMsg>(imageTopic, displayImage);
        rosNode.Subscribe<StringMsg>(audioTextTopic, ReseveMsg);

        // publish audios recorded by HoloLens2 to /audio, Clara AGX will fetch data from this topic
        rosNode.RegisterPublisher<RosMessageTypes.Std.ByteMultiArrayMsg>("audio");


    }

    private void ReseveMsg(StringMsg obj)
    {
        Debug.Log(obj.data);
    }


    // The image message from ROS has different format as Unity, need some processing before displaying it.
    public void displayImage(ImageMsg img)
    {

        texRos = new Texture2D((int)img.width, (int)img.height, TextureFormat.RGB24, false); // , TextureFormat.RGB24
        BgrToRgb(img.data);
        texRos.LoadRawTextureData(img.data);

        texRos.Apply();
        display.texture = texRos;
    }


    // Python, Unity have different image conventions, just use this code
    public void BgrToRgb(byte[] data)
    {
        for (int i = 0; i < data.Length; i += 3)
        {
            byte dummy = data[i];
            data[i] = data[i + 2];
            data[i + 2] = dummy;
        }
    }


    // audio are published as byte array data
    public void publishBytes(byte[] data)
    {
        sbyte[] sdata = (sbyte[])(Array)data;
        RosMessageTypes.Std.ByteMultiArrayMsg msg=new RosMessageTypes.Std.ByteMultiArrayMsg(new RosMessageTypes.Std.MultiArrayLayoutMsg(),sdata);
        rosNode.Publish("audio", msg);
        Debug.Log($"publish a bytearray of size: {data.Length}");
    }
}
