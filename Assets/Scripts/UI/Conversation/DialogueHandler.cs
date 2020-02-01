using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField]
    private Text dialogueText;
    [SerializeField]
    private Button[] answerButtons = new Button[4];

    public void SetDialogue(string dialogueText)
    {

    }

    /// <summary>
    /// When an answer button is clicked
    /// </summary>
    /// <param name="buttonAlternative">Int representation of the answer option</param>
    private void OnAnswer(int buttonAlternative)
    {

    }
}
