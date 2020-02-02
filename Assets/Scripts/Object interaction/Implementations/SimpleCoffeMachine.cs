using Assets.Scripts.Object_interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCoffeMachine : MonoBehaviour, IMachine
{

    bool busy = false;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip pouringSound;
    [SerializeField]
    private AudioClip readySound;

    public bool isBusy()
    {
        return busy;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnDropObject(GameObject dropped, IDraggable draggable)
    {
        draggable.Coffee = new BlackCoffee();
        Debug.Log(dropped.name + " dropped onto: " + gameObject.name + '\n' +
            "The " + dropped.name + " now contains " + draggable.Coffee.Name);
        StartCoroutine(brew(dropped));
    }

    IEnumerator brew(GameObject dropped)
    {
        Cup cup = dropped.transform.gameObject.GetComponent<Cup>();
        cup.lockCup();
        busy = true;
        audioSource.clip = pouringSound;
        audioSource.loop = true;
        audioSource.Play();
        yield return new WaitForSeconds(6.0f);
        audioSource.Stop();
        busy = false;
        audioSource.loop = false;
        audioSource.clip = readySound;
        audioSource.Play();
        cup.unlockCup();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
