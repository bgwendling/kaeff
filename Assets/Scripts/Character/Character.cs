using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;

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

	public IEnumerator Talk(GameObject dialogueObject)
	{
		CoffeeWishes.Clear();
		DialogueHandler dialogueHandler = dialogueObject.GetComponent<DialogueHandler>();
		Speech speech = behaviour.speeches?[nextSpeech ?? Random.Range(0, behaviour.speeches.Count - 1)];
		if (speech != null)
		{
			dialogueObject.SetActive(true);
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
                    foreach(char c in statement.text)
                    {
						dialogueHandler.SetDialogue(dialogueHandler.GetDialogue() + c);
						yield return new WaitForSeconds(0.07f);
                    }
					var wt = (float)System.Math.Ceiling(statement.text.Length / 20f);
					yield return new WaitForSeconds(wt);
					dialogueHandler.SetDialogue("");
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
				dialogueObject.SetActive(false);
				//  Wait a bit before the next text
				yield return new WaitForSeconds(1.5f);
			}
		}
		else
		{
			Debug.Log("Character without speech");
		}

		dialogueObject.SetActive(false);
		yield return null;
		CoffeeWishes.Add(new CoffeeWish { coffeeType = typeof(BlackCoffee), payment = 4.20f });
	}

	//return behaviour.speeches[lastSpeech++];


	public void Awake()
	{
		var behaviourSerializer = new XmlSerializer(typeof(Behaviour));

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
			return;
		if (draggable.Coffee?.GetType() == CoffeeWishes[currentCoffeeWishIndex].coffeeType)
		{
			GameManager.Instance.AddToCurrency(CoffeeWishes[currentCoffeeWishIndex].payment);
			gameObject.SetActive(false);
			gameObject.SetActive(true);

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