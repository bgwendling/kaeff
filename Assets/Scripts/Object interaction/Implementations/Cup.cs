using Assets.Scripts.Object_interaction;
using Assets.Scripts.Object_interaction.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour, IDraggable
{
    public ICoffee Coffee { get; set; }

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
                var cMachine = hitInfo.transform.gameObject;
                cMachine.GetComponent<IMachine>().OnDropObject(gameObject, this);
                //Debug.Log("Hit object:" + hitInfo.transform.gameObject.name);
            }
            if (hitInfo.transform.gameObject.tag.Equals("Character"))
            {
                var cMachine = hitInfo.transform.gameObject;
                //cMachine.GetComponent<ICharacter>().OnDropObject(gameObject, this);
                //Debug.Log("Hit object:" + hitInfo.transform.gameObject.name);
            }
        }


        gameObject.GetComponent<Rigidbody>().useGravity = true;
        //throw new System.NotImplementedException();
    }
}
