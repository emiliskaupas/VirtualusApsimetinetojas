using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OpenAI;
using TMPro;
using static UnityEngine.Rendering.STP;
using UnityEngine.Profiling;
using System.Globalization;
using System.Collections;
using UnityEngine.Networking;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        // References to UI elements
        [SerializeField] private TMP_InputField inputField; // User input field for text
        [SerializeField] private Button button; // Button to send the input text
        [SerializeField] private Button recordButton; // Button to start recording audio
        [SerializeField] private Button stopRecordButton; // Button to stop recording audio

        [SerializeField] private RectTransform sent; // UI template for sent messages
        [SerializeField] private RectTransform received; // UI template for received messages
        [SerializeField] private TMP_Text received_text; // Text field for displaying received message content

        // Internal variables
        private string model; // Model to use for the OpenAI API
        private OpenAIApi openai; // Instance of the OpenAI API
        private string prompt; // Custom prompt to prepend to the user's message
        private AudioRecorder recorder; // Recorder to capture audio
        private WhisperAPI whisper; // Whisper API to transcribe audio to text
        private string transcription; // Transcribed text from recorded audio

        // List to store chat messages
        private List<ChatMessage> messages = new List<ChatMessage>();

        // Awake is called when the script is first loaded
        private void Awake()
        {
            // Initialize the recorder and whisper components
            recorder = GetComponent<AudioRecorder>();
            whisper = GetComponent<WhisperAPI>();
        }
        // Start is called before the first frame update
        private void Start()
        {
            // Check if all required settings are properly initialized
            if (!checkSettingsforNull())
            {
                Debug.LogError("Check script options");
                return;
            }
            // Initialize OpenAI API with the key and set model and prompt
            openai = new OpenAIApi(ConfigLoader.config.api_key);
            model = ConfigLoader.config.model;
            prompt = ConfigLoader.config.prompt;
            Debug.Log("Using OpenAI model: " + model);

            // Assign button listeners for user interactions
            recordButton.onClick.AddListener(StartRecording);
            stopRecordButton.onClick.AddListener(StopRecordingAndTranscribe);
            button.onClick.AddListener(SendReply);
        }
        /// <summary>
        /// Checks if all script options and settings are set up correctly.
        /// </summary>
        /// <returns>true if all required settings are correctly initialized</returns>
        private bool checkSettingsforNull()
        {
            if (ConfigLoader.config == null)
            {
                Debug.LogError("Config failed to load. Check config.json!");
                return false;
            }
            if (inputField == null) return false;
            if (button == null) return false;
            if (recordButton == null) return false;
            if (stopRecordButton == null) return false;
            if (sent == null) return false;
            if (received == null) return false;
            if (received_text == null) return false;
            if (recorder == null) return false;
            if (whisper == null) return false;
            return true;
        }

        /// <summary>
        /// Begins recording of AudioRecorder
        /// </summary>
        public void StartRecording()
        {
            recorder.StartRecording();
        }

        /// <summary>
        /// Stops recording and transcribes the recorded text
        /// transcription- string of the transcribed text
        /// </summary>
        public async void StopRecordingAndTranscribe()
        {
            recorder.StopRecording();
            transcription = await whisper.TranscribeAudio(recorder.GetFilePath());
            inputField.text = transcription;
        }
        /// <summary>
        /// Appends a new message (user or AI) to the chat UI.
        /// </summary>
        /// <param name="message">The message object to display</param>
        private void AppendMessage(ChatMessage message)
        {
            // Select the correct UI template based on whether the message is from the user or AI
            RectTransform parent = message.Role == "user" ? sent : received;

            // Update the existing message's text with the new content
            Text messageText = parent.GetChild(0).GetChild(0).GetComponent<Text>();
            messageText.text = message.Content;

            // Force UI layout update
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
        }
        /// <summary>
        /// Sends a reply to the AI based on the user's input.
        /// </summary>
        private async void SendReply()
        {
            // Create a new message object for the user
            ChatMessage newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            Debug.Log($"NewMessageContent: {newMessage.Content}");

            // Append the user's message to the chat UI
            AppendMessage(newMessage);

            // If it's the first message, prepend the prompt to the user's input
            if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text;

            messages.Add(newMessage);// this could act like message history

            // Disable button and input field while awaiting the response
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;

            // Request a response from the OpenAI model
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = model,
                Messages = messages
            });

            // If the response contains a valid message, display it
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                // Add the AI response message to the list and update the UI
                messages.Add(message);//this could be used as message history
                AppendMessage(message);
                received_text.text = message.Content;

                // Speak the AI's response using the Whisper text-to-speech API
                SpeakText(message.Content);
                Debug.Log(message.Content);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
            // Re-enable the button and input field after processing
            button.enabled = true;
            inputField.enabled = true;
        }
        /// <summary>
        /// Converts text to speech and plays the audio.
        /// </summary>
        /// <param name="text">The text to be spoken</param>
        public async void SpeakText(string text)
        {
            string audioFilePath = await whisper.TextToSpeech(text);
            if (!string.IsNullOrEmpty(audioFilePath))
            {
                Debug.Log($"AUDIOFILEPATH: {audioFilePath}");
                PlayAudio(audioFilePath);
            }
        }

        /// <summary>
        /// Plays an audio file from a given file path.
        /// </summary>
        /// <param name="filePath">The path to the audio file</param>
        private void PlayAudio(string filePath)
        {
            try
            {
                StartCoroutine(PlayAudioCoroutine(filePath));
            }
            catch (MissingReferenceException)
            {
                Debug.Log("Audio playing crashed because program stopped: Exiting");
                return;
            }
        }
        /// <summary>
        /// Coroutine to play the audio file from the given path.
        /// </summary>
        /// <param name="filePath">The path to the audio file</param>
        private IEnumerator PlayAudioCoroutine(string filePath)
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Debug.LogError($"File not found: {filePath}");
                yield break;
            }
            // Prepare the URI for the audio file
            string uri = "file://" + filePath;
            Debug.Log($"Loading audio from: {uri}");

            // Load the audio file using UnityWebRequest
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG))
            {
                yield return www.SendWebRequest();// Wait for the request to complete

                // Check if there was an error loading the audio
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error loading audio: {www.error}");
                    yield break;
                }

                // Get the audio clip from the response
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip == null)
                {
                    Debug.LogError("Failed to load audio clip.");
                    yield break;
                }

                // Get the AudioSource component to play the audio
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    Debug.LogError("No AudioSource found on this GameObject.");
                    yield break;
                }

                // Check if audio is already playing, and stop it before playing the new clip
                if (audioSource.isPlaying)
                {
                    audioSource.Stop(); // Stop any currently playing audio before starting a new one
                }

                // Set the clip and play it
                audioSource.clip = clip;
                audioSource.Play();
                Debug.Log("Playing audio...");
            }
        }
    }
}
