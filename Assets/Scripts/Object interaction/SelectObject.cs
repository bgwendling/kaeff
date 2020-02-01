using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    private GameObject selectedObject = null;
    private IDraggable draggable = null;
    private float distanceToDraggable = 0f;

    // Update is called once per frame
    void Update()
    {
        //Select object
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("lmb clicked");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.transform.gameObject.tag == "Draggable")
                {
                    selectedObject = hitInfo.transform.gameObject;
                    draggable = selectedObject.GetComponent(typeof(IDraggable)) as IDraggable;
                    selectedObject.layer = 2; //Ignore raycast layer

                    distanceToDraggable = selectedObject.transform.position.z - Camera.main.transform.position.z;
                    draggable.OnSelect();
                   
                    
                    //Debug.Log("Hit a draggable object, name: " + selectedObject.name);
                }
                if (hitInfo.transform.gameObject.tag == "Clickable")
                {
                    hitInfo.transform.gameObject.GetComponent<IInterractable>().OnSelect();
                    //fire and forget something on the child script (abstract class clickable)
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject != null)
            {
                draggable.OnUnselect();
                selectedObject.layer = 0; //default layer
                selectedObject = null;
            }
        }

        if(selectedObject != null)
        {
            Vector3 pos = Input.mousePosition;
            pos.z = distanceToDraggable;
            pos = Camera.main.ScreenToWorldPoint(pos);
            pos.z = selectedObject.transform.position.z;
            selectedObject.transform.position = pos;
        }
    }
}
