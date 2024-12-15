using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI UIFpsText;

    // Update is called once per frame
    void Update()
    {
        float current = (int)(1f / Time.unscaledDeltaTime);
        UIFpsText.text = "FPS: " + current;
    }
}
