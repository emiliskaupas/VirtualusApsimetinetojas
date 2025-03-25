using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public class WhisperAPI : MonoBehaviour
{
    // URLs for the Whisper and Text-to-Speech (TTS) API endpoints
    private static readonly string whisperUrl = "https://api.openai.com/v1/audio/transcriptions";
    private static readonly string ttsUrl = "https://api.openai.com/v1/audio/speech";

    /// <summary>
    /// Transcribes the audio file to text using the Whisper API.
    /// </summary>
    /// <param name="filePath">The path to the audio file to be transcribed.</param>
    /// <returns>The transcribed text, or null if an error occurs.</returns>
    public async Task<string> TranscribeAudio(string filePath)
    {
        // Check if the audio file exists
        if (!File.Exists(filePath))
        {
            Debug.LogError("Audio file not found: " + filePath);
            return null;
        }

        // Check if API key is present in the config
        if (string.IsNullOrEmpty(ConfigLoader.config?.api_key))
        {
            Debug.LogError("API key is missing!");
            return null;
        }

        using (HttpClient client = new HttpClient()) // Create an HTTP client to make the API request
        {
            // Add authorization header with API key
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigLoader.config.api_key);

            using (var form = new MultipartFormDataContent()) // Create a multipart form data content for the request
            {
                // Add necessary form data parameters for Whisper API
                form.Add(new StringContent(ConfigLoader.config.whisper), "model"); // Specify Whisper model
                form.Add(new StringContent(ConfigLoader.config.language), "language"); // Specify the language (e.g., "lt" for Lithuanian)

                // Read the audio file as a byte array
                byte[] audioBytes = File.ReadAllBytes(filePath);
                // Add the audio file content to the form data
                form.Add(new ByteArrayContent(audioBytes), "file", Path.GetFileName(filePath));

                try
                {
                    // Make the POST request to the Whisper API to transcribe the audio
                    HttpResponseMessage response = await client.PostAsync(whisperUrl, form);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response JSON
                        JObject json = JObject.Parse(jsonResponse);
                        // Extract transcribed text from the response
                        string transcribedText = json["text"]?.ToString();

                        if (string.IsNullOrEmpty(transcribedText))
                        {
                            Debug.LogError("Transcription returned empty text.");
                            return null;
                        }

                        return transcribedText; // Return the transcribed text
                    }
                    else
                    {
                        // Log error if the API request fails
                        Debug.LogError("Whisper API error: " + jsonResponse);
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    // Catch any exceptions that occur during the API request
                    Debug.LogError($"API request failed: {ex.Message}");
                    return null;
                }
            }
        }
    }

    /// <summary>
    /// Converts text to speech using the OpenAI TTS API.
    /// </summary>
    /// <param name="prompt">The text to be converted into speech.</param>
    /// <returns>The file path of the generated audio file, or null if an error occurs.</returns>
    public async Task<string> TextToSpeech(string prompt)
    {
        // Ensure the prompt is not empty
        if (string.IsNullOrEmpty(prompt))
        {
            Debug.LogError("No text for TTS found");
            return null;
        }

        // Check if API key is present in the config
        if (string.IsNullOrEmpty(ConfigLoader.config?.api_key))
        {
            Debug.LogError("API key is missing!");
            return null;
        }

        using (HttpClient client = new HttpClient()) // Create an HTTP client to make the API request
        {
            // Add authorization header with API key
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigLoader.config.api_key);

            // Create request data for the TTS API, specifying the model, input text, voice, and response format
            var requestData = new
            {
                model = "tts-1", // Specify TTS model
                input = prompt, // The text to be converted to speech
                voice = "nova", // The voice to use for the TTS
                response_format = "mp3" // Specify the response format as mp3 audio
            };

            // Serialize the request data to JSON
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            // Create a StringContent object with the serialized JSON data
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            try
            {
                // Make the POST request to the TTS API
                HttpResponseMessage response = await client.PostAsync(ttsUrl, content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the audio data from the response
                    byte[] audioData = await response.Content.ReadAsByteArrayAsync();
                    // Define the output file path to save the generated speech
                    string outputPath = Path.Combine(Application.persistentDataPath, "speech.mp3");

                    // Write the audio data to the file
                    File.WriteAllBytes(outputPath, audioData);
                    Debug.Log($"Speech saved: {outputPath}"); // Log the file path
                    return outputPath; // Return the file path of the saved speech
                }
                else
                {
                    // Log error if the API request fails
                    Debug.LogError("TTS API Error: " + await response.Content.ReadAsStringAsync());
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Catch any exceptions that occur during the API request
                Debug.LogError($"TTS request failed: {ex.Message}");
                return null;
            }
        }
    }
}
