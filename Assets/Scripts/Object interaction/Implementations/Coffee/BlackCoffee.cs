using Assets.Scripts.Object_interaction.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCoffee : IRegularCoffee
{
    public string Name { get => "Black Coffee"; }
    public int MilkType { get; set; }
    public int BeanID { get; set; }
}
