﻿using System.Collections;
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

    private int? nextSpeech = 0;

    private bool talking = false;

    [SerializeField]
    private float textSpeedModifier = 1.0f;

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
                    yield return Say(statement.text, dialogueHandler, dialogueObject);
                    dialogueHandler.SetQuestions(answerStrings, answerValues);
                }
            }
        }
        else
        {
            Debug.Log("Character without speech");
        }

		dialogueObject.SetActive(false);
		CoffeeWishes.Add(new CoffeeWish { coffeeType = typeof(BlackCoffee), payment = 4.20f });
		cup.SetActive(true);
		yield return null;
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
			yield return new WaitForSeconds(0.08f);
			if (c.Equals('.') || c.Equals('?') || c.Equals('!'))
			{
				yield return new WaitForSeconds(0.7f);
			}
			audioSource.Stop();
		}
		var wt = (float)System.Math.Ceiling(text.Length / 60f);
		yield return new WaitForSeconds(wt);
		dialogueHandler.SetDialogue("");
		talking = false;
		dialogueObject.SetActive(false);
		yield return new WaitForSeconds(0.6f);
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
            Say("Uhm, I haven't ordered yet", stolenDialogueHandler, stolenDialogueObject);
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
            Say("Wow, thanks, that's exactly what I DIDN'T order", stolenDialogueHandler, stolenDialogueObject);
            Speech speech = behaviour.speeches[1];
            if (speech != null)
            {
                foreach (Entry statement in speech.statements)
                {
                    if (System.Enum.TryParse(statement.moodChange?.ToLower().Trim(), out Pose poseks))
                    {
                        changePose(poseks);
                    }
                    else if (string.IsNullOrEmpty(statement.moodChange))
                    {
                        Debug.Log("Pose defined in XML but not recognized");
                    }

                    if (statement.choice.Count == 0)
                    {
                        Say(statement.text, stolenDialogueHandler, stolenDialogueObject);
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
                        Say(statement.text, stolenDialogueHandler, stolenDialogueObject);
                        stolenDialogueHandler.SetQuestions(answerStrings, answerValues);
                    }
                }
            }
            if (System.Enum.TryParse("drinking".ToLower().Trim(), out Pose pose))
            {
                changePose(pose);
            }

        }
        //GameManager.Instance.SetTalking(false);
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