using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using System;

public class Character : MonoBehaviour
{
	public TextAsset config;
	public Behaviour behaviour;

	List<CoffeeWish> CoffeeWishes = new List<CoffeeWish>();
	private int currentCoffeeWishIndex = 0;

	[SerializeField]
	private PoseGameObject[] poseGameObjects = null;

	private int lastSpeech = 0;
	private int? nextSpeech = null;

	private bool talking = false;

	private DialogueHandler stolenDialogueHandler;
	private GameObject stolenDialogueObject;


	private AudioSource audioSource;
	[SerializeField]
	private AudioClip talkSound1;
	[SerializeField]
	private AudioClip talkSound2;
	[SerializeField]
	private AudioClip talkSound3;

	public IEnumerator Talk(GameObject dialogueObject, GameObject cup)
	{
		cup.SetActive(false);
		CoffeeWishes.Clear();
		DialogueHandler dialogueHandler = dialogueObject.GetComponent<DialogueHandler>();
		if (stolenDialogueHandler == null)
			stolenDialogueHandler = dialogueHandler;
		if (stolenDialogueObject == null)
			stolenDialogueObject = dialogueObject;
		Speech speech = behaviour.speeches?[nextSpeech ?? UnityEngine.Random.Range(0, behaviour.speeches.Count)];
		if (speech != null)
		{
			foreach (Entry statement in speech.statements)
			{
				dialogueObject.SetActive(true);
                
				if (System.Enum.TryParse(statement.moodChange.ToLower().Trim(), out Pose pose))
				{
					changePose(pose);
				}
				else if (string.IsNullOrEmpty(statement.moodChange))
				{
					Debug.Log("Pose defined in XML but not recognized");
				}
				
				if (statement.choice.Count == 0)
				{
						yield return Say(statement.text, dialogueHandler, dialogueObject);
				}
				else
				{
					string[] answerStrings = new string[4];
					int?[] answerValues = new int?[4];
					for (int i = 0; i < (statement.choice.Count > 4 ? 4 : statement.choice.Count); i++)
					{
						answerStrings[i] = statement.choice[i].value;
						answerValues[i] = statement.choice[i].result;
					}

					dialogueHandler.SetQuestions(answerStrings, answerValues);
				}
			}
		}
		else
		{
			Debug.Log("Character without speech");
		}

		dialogueObject.SetActive(false);
		yield return null;
		CoffeeWishes.Add(new CoffeeWish { coffeeType = typeof(BlackCoffee), payment = 4.20f });
		cup.SetActive(true);
	}

	//return behaviour.speeches[lastSpeech++];

        private Coroutine Say(string text, DialogueHandler dialogueHandler, GameObject dialogueObject)
    {
		return StartCoroutine(populateSpeechBubble(text, dialogueHandler, dialogueObject));
    }

        private IEnumerator populateSpeechBubble(string text, DialogueHandler dialogueHandler, GameObject dialogueObject)
    {
		if (talking)
			yield break;
		talking = true;
		dialogueObject.SetActive(true);
		foreach (char c in text)
		{
			if (audioSource != null && !audioSource.isPlaying && Char.IsLetter(c))
			{
				int randomClip = UnityEngine.Random.Range(0, 3);
                if(randomClip == 0)
				    audioSource.clip = talkSound1;
				if (randomClip == 1)
					audioSource.clip = talkSound2;
				if (randomClip == 2)
					audioSource.clip = talkSound3;
				audioSource.Play();
			}
			dialogueHandler.SetDialogue(dialogueHandler.GetDialogue() + c);
			yield return new WaitForSeconds(0.1f);
			audioSource.Stop();
		}
		var wt = (float)System.Math.Ceiling(text.Length / 30f);
		yield return new WaitForSeconds(wt);
		dialogueHandler.SetDialogue("");
		talking = false;
		dialogueObject.SetActive(false);
		yield return new WaitForSeconds(1.5f);
	}


	public void Awake()
	{
		var behaviourSerializer = new XmlSerializer(typeof(Behaviour));
		audioSource = GetComponent<AudioSource>();

		using (TextReader reader = new StringReader(config.ToString()))
		{
			behaviour = (Behaviour)behaviourSerializer.Deserialize(reader);
		}
	}

	public void Start()
	{
	}

	public void Update()
	{

	}

	public void OnReceiveObject(IDraggable draggable, GameObject gameObject)
	{
		if (CoffeeWishes.Count == 0)
		{
			Say("I haven't ordered yet", stolenDialogueHandler, stolenDialogueObject);
			return;
		}
		if (draggable.Coffee == null)
		{
			Say("There's no coffee", stolenDialogueHandler, stolenDialogueObject);
			return;
		}

		if (draggable.Coffee?.GetType() == CoffeeWishes[currentCoffeeWishIndex].coffeeType)
		{
			GameManager.Instance.AddToCurrency(CoffeeWishes[currentCoffeeWishIndex].payment);
			gameObject.SetActive(false);
			Say("Thanks, exactly what I didn't order", stolenDialogueHandler, stolenDialogueObject);
			if (System.Enum.TryParse("drinking".ToLower().Trim(), out Pose pose))
			{
				changePose(pose);
			}

		}
	}

	private void changePose(Pose toPose)
	{
		foreach (var pose in poseGameObjects)
		{
			if (pose.pose == toPose)
			{
				pose.poseObject.SetActive(true);
			}
			else
			{
				pose.poseObject.SetActive(false);
			}
		}
	}

	public void SetNextSpeech(int? nextSpeech)
	{
		this.nextSpeech = nextSpeech;
	}
}

class CoffeeWish
{
	public System.Type coffeeType;
	public float payment;
}

[System.Serializable]
class PoseGameObject
{
	public Pose pose;
	public GameObject poseObject;
}

enum Pose
{
	neutral,
	sad,
	mad,
	drinking
}