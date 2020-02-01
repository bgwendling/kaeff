using Assets.Scripts.Object_interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCoffeMachine : MonoBehaviour, IMachine
{
    public void OnDropObject(GameObject dropped)
    {
        Debug.Log(dropped.name + " dropped onto: " + gameObject.name);
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
