using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
public class AudioRecorder : MonoBehaviour
{
    private AudioClip recordedClip;
    private string filePath;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/recordedAaudio.wav";
    }
    public void StartRecording()
    {
        recordedClip = Microphone.Start(null, false, 10, 44100);
        Debug.Log("Recording started...");
    }
    public void StopRecording()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            SaveRecording();
            Debug.Log("Recording stopped.");
        }
    }
    private void SaveRecording()
    {
        if (recordedClip == null) return;

        byte[] audioData = WavUtility.FromAudioClip(recordedClip);
        File.WriteAllBytes(filePath, audioData);
        Debug.Log("Audio saved at: " + filePath);

    }
    public string GetFilePath()
    {
        return filePath;
    }

}

