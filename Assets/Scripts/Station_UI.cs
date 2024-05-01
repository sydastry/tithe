using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using TMPro;

public class Station_UI : MonoBehaviour
{
    public Flowchart flowchart;
    public TextMeshProUGUI myNumber;

    // Update is called once per frame
    void Update()
    {
        float target = flowchart.GetFloatVariable("Haunted_Needle");
        target = target * 100;
        int noDecimal = Mathf.RoundToInt(target);
        myNumber.text = noDecimal.ToString();
    }
}
