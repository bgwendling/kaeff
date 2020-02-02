using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
	[SerializeField]
	private Text dialogueText;
	[SerializeField]
	private GameObject[] answerButtons = new GameObject[4];

	private void Start()
	{
		foreach (var button in answerButtons)
		{
			button.gameObject.SetActive(false);
		}
	}

	public void SetDialogue(string dialogueText)
	{
		this.dialogueText.text = dialogueText;
	}

	/// <summary>
	/// When an answer button is clicked
	/// </summary>
	/// <param name="buttonValue">Possible conversationchange</param>
	private void OnAnswer(int? buttonValue)
	{
		if (buttonValue != null)
			GameManager.Instance.GetActiveCharacter().SetNextSpeech(buttonValue);

		foreach (var button in answerButtons)
		{
			button.gameObject.SetActive(false);
		}
	}

	public void SetQuestions(string[] questionTexts, int?[] value)
	{
		for (int i = 0; i < (questionTexts.Length > 4 ? 4 : questionTexts.Length); i++)
		{
			answerButtons[i].GetComponent<Button>().onClick.AddListener(delegate () { OnAnswer(value[i]); });
			answerButtons[i].GetComponentInChildren<Text>().text = questionTexts[i];
			answerButtons[i].SetActive(true);
		}
	}
}
