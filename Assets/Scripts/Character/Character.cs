using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Character : MonoBehaviour
{
    public TextAsset config;
    public int id;

    List<coffeeWish> CoffeeWishes = new List<coffeeWish>();
    private int currentCoffeeWishIndex = 0;

    public void Awake()
    {
        CoffeeWishes.Add(new coffeeWish { coffeeType = typeof(BlackCoffee), payment = 4.20f });
    }

    public void Update()
    {

    }

    public void OnReceiveObject(IDraggable draggable)
    {
        if(draggable.Coffee?.GetType() == CoffeeWishes[currentCoffeeWishIndex].coffeeType)
        {
            GameManager.Instance.AddToCurrency(CoffeeWishes[currentCoffeeWishIndex].payment);
        }
    }

}

class coffeeWish
{
    public System.Type coffeeType;
    public float payment;
}
