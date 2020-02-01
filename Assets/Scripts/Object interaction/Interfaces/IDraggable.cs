using Assets.Scripts.Object_interaction.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDraggable : IInterractable
{
    ICoffee Coffee { get; set; }
    void OnUnselect();
}
