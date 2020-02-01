using Assets.Scripts.Object_interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCoffeMachine : MonoBehaviour, IMachine
{
    public void OnDropObject(GameObject dropped, IDraggable draggable)
    {
        draggable.Coffee = new BlackCoffee();
        Debug.Log(dropped.name + " dropped onto: " + gameObject.name + '\n' +
            "The " + dropped.name + " now contains " + draggable.Coffee.Name);
        //throw new System.NotImplementedException();
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
