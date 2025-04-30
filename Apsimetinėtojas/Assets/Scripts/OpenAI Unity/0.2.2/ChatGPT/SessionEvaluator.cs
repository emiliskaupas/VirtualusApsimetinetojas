using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class SessionEvaluator : MonoBehaviour
{
    [SerializeField] private Button EndSessionButton;
    private static readonly string evaluationEndpoint = "https://api.openai.com/v1/chat/completions";
    private static readonly string evaluationModel = "gpt-4-1106-preview";  // Or "gpt-4o"
    private static readonly string systemPrompt = @"Tu esi profesionalus vertintojas, kuris skaito pokalbį tarp vartotojo ir AI asistento, kuris specifiškai ištreniruotas būti kuo emocingesniu. Vartotojas ieško būdų kaip apraminti Dirbtinį intelektą. 
Tavo darbas yra įvertinti vartotojo pagalbos kokybę pagal šiuos kriterijus:

1. Empatija ir emocinis brandumas
2. Aktyvus klausymas
3. Patarimų nauda
4. Pokalbio eiga
5. Bendra pokalbio nauda

Atsakyme aprašyk:
- Rezultatą, įvertintą nuo 1 iki 4 kiekvienam kriterijui.
- Apibendrinimą.
Visas pokalbis turi vykti lietuviškai ir tavo atsakymas turi būti lietuviškas. Visiškai ignoruok patį pirmą vartotojo teiginį, kadangi jis skirtas tik duoti dirbtiniui intelektui bendrą suvokimą apie situaciją.
Atrašyk tokiu formatu ir būtinai tik tokiu formatu::
1. Empatija ir emocinis brandumas: x/5
2. Aktyvus klausymas: x/5
3. Patarimų nauda: x/5
4. Pokalbio eiga: x/5
5. Bendra pokalbio nauda: x/5
Kur x yra vertinimas kategorijai
santrauka (iki 5 sakiniu)
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
            string apiKey = ConfigLoader.config.api_key;  // <- Add this in your new or extended config
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(evaluationEndpoint, content);
            string result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                JObject parsed = JObject.Parse(result);
                string evaluation = parsed["choices"][0]["message"]["content"].ToString();
                Debug.Log("Evaluation:\n" + evaluation);
                talkingPage.ShowEvaluation(evaluation);
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
