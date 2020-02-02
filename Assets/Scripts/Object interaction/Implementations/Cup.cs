using Assets.Scripts.Object_interaction;
using Assets.Scripts.Object_interaction.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour, IDraggable
{

    Vector3 originalPosition;

    public ICoffee Coffee { get; set; }

    private void Awake()
    {
        originalPosition = transform.position;
    }

    public void OnSelect()
    {
        Debug.Log("Cup object clicked");
        gameObject.tag = "Draggable";
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        //throw new System.NotImplementedException();
    }

    public void OnUnselect()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit)
        {
            if (hitInfo.transform.gameObject.tag.Equals("CoffeeMachine"))
            {
                var cMachine = hitInfo.transform.gameObject.GetComponent<IMachine>();
                cMachine.OnDropObject(gameObject, this);
                //Debug.Log("Hit object:" + hitInfo.transform.gameObject.name);
            }
            if (hitInfo.transform.gameObject.tag.Equals("Character"))
            {
                var character = hitInfo.transform.gameObject;

                character.GetComponent<Character>().OnReceiveObject(this, gameObject);
                Debug.Log(gameObject.name + "Dropped upon character:" + character.name + "\nThe cup contains:" + Coffee?.Name);
            }
        }


        gameObject.GetComponent<Rigidbody>().useGravity = true;
        //throw new System.NotImplementedException();
    }

    public void lockCup()
    {
        gameObject.tag = "Untagged";
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void unlockCup()
{
        gameObject.tag = "Draggable";
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    void OnDisable()
    {
        Coffee = null;
        transform.position = originalPosition;
    }
}
