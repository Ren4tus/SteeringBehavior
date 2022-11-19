using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DebugText : MonoBehaviour
{
    private TextMeshProUGUI resourceText;
    private const string firstLine = "NowMode : ";
    // Start is called before the first frame update
    void Start()
    {
        resourceText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        resourceText.text = firstLine + S_GameManager.Instance.mode.ToString();
    }

}
