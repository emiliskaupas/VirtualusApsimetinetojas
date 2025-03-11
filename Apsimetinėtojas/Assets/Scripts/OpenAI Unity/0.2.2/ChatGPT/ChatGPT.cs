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

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private Button recordButton;
        [SerializeField] private Button stopRecordButton;
        //[SerializeField] private ScrollRect scroll;

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;
        [SerializeField] private TMP_Text received_text;

        private float height;
        private string model;
        private OpenAIApi openai;
        private string prompt;
        private AudioRecorder recorder;
        private WhisperAPI whisper;
        private bool recorded = false;
        private string transcription;

        private List<ChatMessage> messages = new List<ChatMessage>();

        private void Awake()
        {
            recorder = GetComponent<AudioRecorder>();
            whisper = GetComponent<WhisperAPI>();
        }
        private void Start()
        {
            if (ConfigLoader.config == null)
            {
                Debug.LogError("Config failed to load. Check config.json!");
                return;
            }
            
            openai = new OpenAIApi(ConfigLoader.config.api_key);
            model = ConfigLoader.config.model;
            prompt = ConfigLoader.config.prompt;
            Debug.Log("Using OpenAI model: " + model);
            recordButton.onClick.AddListener(StartRecording);
            stopRecordButton.onClick.AddListener(StopRecordingAndTranscribe);
            button.onClick.AddListener(SendReply);



        }
        /// <summary>
        /// Begins recording of AudioRecorder
        /// </summary>
        public void StartRecording()
        {
            recorder.StartRecording();
            recorded = true;
        }
        /// <summary>
        /// Stops recording and transcribes the recorded text
        /// transcription- string of the transcribed text
        /// </summary>
        public async void StopRecordingAndTranscribe()
        {
            recorder.StopRecording();
            transcription = await whisper.TranscribeAudio(recorder.GetFilePath());
        }

        private void AppendMessage(ChatMessage message)
        {
            //scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received/*, scroll.content*/);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            //scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            //scroll.verticalNormalizedPosition = 0;
        }

        private async void SendReply()
        {
            ChatMessage newMessage;
            if (!recorded)
            {
                newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
                Debug.Log($"NOT RECORDED {inputField.text}");
            }
            else
            {
                await Task.Delay(4000); ///delay because transcription takes a long time (it's all running in parallel. Why? idek man)
                newMessage = new ChatMessage()
                {
                    Role = "user",
                    Content = transcription
                };
                Debug.Log($"RECORDED {transcription}");
            recorded = false;
            }
            Debug.Log($"NewMessageContent: {newMessage.Content}");

            AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text;

            messages.Add(newMessage);

            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;

            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = model,
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);
                AppendMessage(message);
                received_text.text = message.Content;
                Debug.Log(message.Content);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
            Destroy(GameObject.Find("Sent Message(Clone)"), 1f);
            Destroy(GameObject.Find("Received Message(Clone)"), 1f);
            button.enabled = true;
            inputField.enabled = true;
        }
    }
}
