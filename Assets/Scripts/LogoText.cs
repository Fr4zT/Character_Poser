using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class LogoText : MonoBehaviour
{
    [SerializeField] TMP_Text tmtext;
    [SerializeField] GameObject linePrefab;
    [SerializeField] List<int> subdivision = new List<int>();
    [SerializeField] RectTransform textRect;
    [SerializeField] List<Color> colors = new List<Color>();

    float textSize;
    List<Image> imgs = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        textSize = textRect.rect.width;
        StartCoroutine(CreateLines());
    }

    IEnumerator CreateLines()
    {
        subdivision.Shuffle();
        for (int i = 0; i < subdivision.Count; i++)
        {
            var newLine = Instantiate(linePrefab, textRect);
            var lineRect = newLine.GetComponent<RectTransform>();
            lineRect.localPosition = new Vector2((subdivision[i] * (textRect.sizeDelta.x / subdivision.Count)) - textRect.sizeDelta.x / 2, 0);
            lineRect.sizeDelta = new Vector2(0, lineRect.sizeDelta.y);
            newLine.GetComponent<Image>().color = colors[Random.Range(0, colors.Count)];
            imgs.Add(newLine.GetComponent<Image>());
            DOTween.To(()=>lineRect.sizeDelta, x=> lineRect.sizeDelta=x, new Vector2(textRect.sizeDelta.x / subdivision.Count, lineRect.sizeDelta.y), 1f);
            yield return new WaitForSeconds(Random.Range(0.05f,0.2f));
        }
        yield return new WaitForSeconds(1);
        foreach(var img in imgs)
        {
            img.DOColor(Color.white, 1);
        }
        yield return new WaitForSeconds(2);
        foreach (var img in imgs)
        {
            img.DOColor(Color.black, 1);
        }
        yield return new WaitForSeconds(1.5f);
        UsefulFunctions.ChangeScene("Poser");
    }
}
