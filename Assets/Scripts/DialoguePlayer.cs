using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguePlayer : MonoBehaviour
{
    /// <summary>
    /// Puts dialogue in textbox
    /// Puts name in name box
    /// Never has direct acess to elements
    /// Only given them
    /// </summary>
    /// 
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayNextLine(string nextLine, TextMeshProUGUI textbox, float textSpeed)
    {
        StartCoroutine(PlayNextLineCoroutine(nextLine, textbox, textSpeed));
    }

    private IEnumerator PlayNextLineCoroutine(string nextLine, TextMeshProUGUI textbox, float textSpeed)
    {
        // To skip fully
        // Pause this and just make it equal to the full thing

        char[] chars = nextLine.ToCharArray();
        textbox.text = "";

        for (int i = 0; i < chars.Length; i++)
        {
            textbox.text += chars[i];
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
