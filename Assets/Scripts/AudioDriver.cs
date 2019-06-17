using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDriver : MonoBehaviour
{
    string audioStream;

    public AudioDriver(string configPath){

        audioStream=FileRead.FindPathFromConfig(configPath,"AUDIO_STREAM_URL")
    }
}
