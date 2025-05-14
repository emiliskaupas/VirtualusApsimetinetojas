using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EvaluationGraph : MonoBehaviour
{
    [System.Serializable]
    public class Bar
    {
        public string label;
        public RectTransform barFill;
        public Text valueText;
        public Text labelText;
    }

    public List<Bar> bars;
    public float maxScore = 5f;
    public float animationDuration = 0.5f;

    public void SetValues(Evaluation evaluation)
    {
        Dictionary<string, int> values = new Dictionary<string, int>()
        {
            { "Empatija", evaluation.empatijaIrBrandumas },
            { "Aktyvus", evaluation.aktyvusKlausymas },
            { "Patarimai", evaluation.patarimuNauda },
            { "Eiga", evaluation.pokalbioEiga },
            { "Nauda", evaluation.bendraNauda }
        };

        for (int i = 0; i < bars.Count; i++)
        {
            Bar bar = bars[i];
            if (values.TryGetValue(bar.label, out int value))
            {
                bar.labelText.text = bar.label;
                bar.valueText.text = value.ToString();
                StartCoroutine(AnimateBar(bar.barFill, value / maxScore));
            }
        }
    }

    private IEnumerator<WaitForEndOfFrame> AnimateBar(RectTransform fill, float targetFill)
    {
        float elapsed = 0f;
        float startScale = fill.localScale.y;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float newScale = Mathf.Lerp(startScale, targetFill, elapsed / animationDuration);
            fill.localScale = new Vector3(1, newScale, 1);
            yield return new WaitForEndOfFrame();
        }

        fill.localScale = new Vector3(1, targetFill, 1);
    }
}
