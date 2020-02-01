using UnityEngine;

namespace Assets.Scripts.Object_interaction
{
    interface IMachine
    {
        /// <summary>
        /// When you drop an object on top of the machine
        /// </summary>
        /// <param name="dropped">the object being dropped</param>
        void OnDropObject(GameObject dropped, IDraggable draggable);

        bool isBusy();
    }
}
