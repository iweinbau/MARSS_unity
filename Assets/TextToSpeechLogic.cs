using Microsoft.MixedReality.Toolkit.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToSpeechLogic : MonoBehaviour
{
    private TextToSpeech textToSpeech;
    public string speakText;

    private void Awake()
    {
        textToSpeech = GetComponent<TextToSpeech>();
    }

    public void OnInputClicked() 
    {
        var msg = string.Format(
            speakText, textToSpeech.Voice.ToString());

        textToSpeech.StartSpeaking(msg);
    }
}
