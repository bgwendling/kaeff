using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour, IDraggable
{
    public void OnSelect()
    {
        Debug.Log("Cup object clicked");
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        //throw new System.NotImplementedException();
    }

    public void OnUnselect()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        //throw new System.NotImplementedException();
    }
}
