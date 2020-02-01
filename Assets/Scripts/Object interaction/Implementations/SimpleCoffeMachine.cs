using Assets.Scripts.Object_interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCoffeMachine : MonoBehaviour, IMachine
{

    bool busy = false;

    public bool isBusy()
    {
        return busy;
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
        yield return new WaitForSeconds(5.0f);
        busy = false;
        cup.unlockCup();

    }

 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
