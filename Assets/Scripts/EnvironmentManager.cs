using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] Material litMat;
    [SerializeField] Material unlitMat;
    [SerializeField] List<Renderer> environment;
    [SerializeField] FlexibleColorPicker colorPicker;
    [SerializeField] TMP_Text modeText;
    bool isLit = true;

    public void SwitchEnvironment()
    {
        if (isLit)
        {
            ChangeMat(unlitMat);
        }
        else
        {
            ChangeMat(litMat);
        }
        isLit = !isLit;
        modeText.text = "Switch type: " + (isLit ? "shaded" : "solid");
    }

    void ChangeMat(Material material)
    {
        foreach (var part in environment)
        {
            part.material = material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLit)
        {
            litMat.color = colorPicker.color;
        }
        else
        {
            unlitMat.color = colorPicker.color;
        }
    }
}
