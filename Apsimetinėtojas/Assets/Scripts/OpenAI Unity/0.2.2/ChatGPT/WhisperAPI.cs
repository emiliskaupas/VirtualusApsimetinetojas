using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class WhisperAPI : MonoBehaviour
{
    private static readonly string whisperUrl = "https://api.openai.com/v1/audio/transcriptions";
    private static readonly string ttsUrl = "https://api.openai.com/v1/audio/speech";

    public async Task<string> TranscribeAudio(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Audio file not found: " + filePath);
            return null;
        }

        ///HttpClient- class for making httprequests for the API
        using (HttpClient client = new HttpClient())
        {
            ///Initialize the API from the config file's api key
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigLoader.config.api_key);

            ///form for API request
            using (var form = new MultipartFormDataContent())
            {
                form.Add(new StringContent("whisper-1"), "model"); ///WhisperAPI model
                form.Add(new StringContent("lt"), "language"); ///language selection
                Debug.Log($"File getting from: {filePath}");
                form.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "file", Path.GetFileName(filePath) + "/ recordedAaudio.wav"); ///rexorded audio file

                HttpResponseMessage response = await client.PostAsync(whisperUrl, form);
                string jsonResponse = await response.Content.ReadAsStringAsync(); ///response in json format for parsing as string

                if (response.IsSuccessStatusCode)
                {
                    // Extract the "text" field from JSON response
                    JObject json = JObject.Parse(jsonResponse);
                    return json["text"]?.ToString();

                }
                else
                {
                    Debug.LogError("Whisper API error: " + jsonResponse);
                    return null;
                }
            }
        }
    }
    public async Task <string>TextToSpeech(string prompt)
    {
        if(prompt == null)
        {
            Debug.LogError("No text for TTS found");
            return null;
        }
        using(HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigLoader.config.api_key);
            var requestData = new
            {
                model = "tts-1",
                input = prompt,
                voice = "nova",
                response_format = "mp3"
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            string outputPath = Path.Combine(Application.persistentDataPath, "speech.mp3");
            HttpResponseMessage response = await client.PostAsync(ttsUrl, content);
            if (response.IsSuccessStatusCode)
            {
                byte[] audioData = await response.Content.ReadAsByteArrayAsync();
                File.WriteAllBytes(outputPath, audioData);
                Debug.Log($"Speech saved: {outputPath}");
                return outputPath;
            }
            else
            {
                {
                    Debug.LogError("TTS API Error: " + await response.Content.ReadAsStringAsync());
                    return null;
                }
            }
        }
    }
}
