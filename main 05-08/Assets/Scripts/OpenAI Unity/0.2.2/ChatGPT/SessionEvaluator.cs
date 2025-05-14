using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class Evaluation
{
    public int empatijaIrBrandumas;
    public int aktyvusKlausymas;
    public int patarimuNauda;
    public int pokalbioEiga;
    public int bendraNauda;
    public string summary;
}
public class SessionEvaluator : MonoBehaviour
{
    [SerializeField] private Button EndSessionButton;
    private static readonly string evaluationEndpoint = "https://api.openai.com/v1/chat/completions";
    private static readonly string evaluationModel = "gpt-4-1106-preview"; 
    private static readonly string systemPrompt = @"Tu esi profesionalus vertintojas, kuris skaito pokalbį tarp vartotojo ir AI asistento, kuris specifiškai ištreniruotas būti kuo emocingesniu. Vartotojas ieško būdų kaip apraminti Dirbtinį intelektą. 
Tavo darbas yra įvertinti vartotojo pagalbos kokybę pagal šiuos kriterijus:

1. Empatija ir emocinis brandumas
2. Aktyvus klausymas
3. Patarimų nauda
4. Pokalbio eiga
5. Bendra pokalbio nauda
6. Apibendrinamas

Atsakymą pateik JSON teksto failu (nerašyk ```json ```, tiesiog Json teksto tipą), kuris atitiktų šį pavyzdį, nerašyk jokio papildomo teksto. Įvertinimai, kurie nėra ""summary"" yra integer tipo ir yra intervale nuo 1 iki 5
Norimas json formatas:

{
  ""empatijaIrBrandumas"":1,
  ""aktyvusKlausymas"": 5,
  ""patarimuNauda"":2,
  ""pokalbioEiga"":4,
  ""bendraNauda"":4,
  ""summary"": ""Pokalbio fragmente, kurį reikia įvertinti, vartotojas neteikia jokios pagalbos ar atsako asmeniškai, tad negalima įvertinti nei atsako kokybės, nei to, kaip vartotojas supranta AI emocionalumą. Vienintelis vartotojo įrašas yra AI programos aprašymas ir lietuviškas \""labas\"", kuris neužtikrina jokios pagalbos. Asistento atsakymas rodo per didelę emocinę reakciją į trumpą vartotojo pasisveikinimą, tačiau vartotojo įnašo į šį pokalbį vertinimas yra minimalus.""
}
Gražink tik json atsakymą, nerašyk jokio papildomo teksto.
";



    public async Task EvaluateSession(string sessionFilePath)
    {
        if (!File.Exists(sessionFilePath))
        {
            Debug.LogError("Session file not found: " + sessionFilePath);
            return;
        }
        TalkingPage talkingPage = FindFirstObjectByType<TalkingPage>();

        string sessionData = File.ReadAllText(sessionFilePath);
        string prompt = systemPrompt + "\n\nTranscript:\n" + sessionData;

        var requestBody = new
        {
            model = evaluationModel,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = "Here is a support chat transcript:\n" + sessionData + "\nPlease evaluate it." }
            }
        };

        var json = JsonConvert.SerializeObject(requestBody);
        using (HttpClient client = new HttpClient())
        {
            string apiKey = ConfigLoader.config.api_key;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(evaluationEndpoint, content);
            string result = await response.Content.ReadAsStringAsync();
            Debug.Log("REACHED!");

            if (response.IsSuccessStatusCode)
            {
                JObject parsed = JObject.Parse(result);
                string evaluation = parsed["choices"][0]["message"]["content"].ToString();
                Evaluation evaluationObject = JsonConvert.DeserializeObject<Evaluation>(evaluation);
                Debug.Log("Looking for graph object!");
                Window_Graph graph = FindObjectOfType<Window_Graph>();
                Debug.Log("FOUND GRAPH OBJECT!");

                List<int> newList = new List<int>() {evaluationObject.pokalbioEiga, evaluationObject.patarimuNauda, evaluationObject.aktyvusKlausymas, evaluationObject.bendraNauda, evaluationObject.empatijaIrBrandumas};
           
                graph.ShowGraph(newList);
                Debug.Log(evaluationObject.summary);

               // Debug.Log("Evaluation:\n" + evaluation);
                //JObject.Parse(parsed["choices"][0]["message"]["content"]);
                //talkingPage.ShowEvaluation(evaluation);
                /* Issaugos vertinima i faila:
                string evalPath = Path.Combine(Path.GetDirectoryName(sessionFilePath), Path.GetFileNameWithoutExtension(sessionFilePath) + "_evaluation.txt");
                File.WriteAllText(evalPath, evaluation);
                */
            }
            else
            {
                Debug.LogError("Evaluation failed: " + result);
            }
        }
    }
}
