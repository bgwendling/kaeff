﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Character : MonoBehaviour
{
    public TextAsset config;
    public Behaviour behaviour;

    List<coffeeWish> CoffeeWishes = new List<coffeeWish>();
    private int currentCoffeeWishIndex = 0;

    private int lastSpeech = 0;

    public Speech talk()
    {
        return behaviour.speeches[lastSpeech++];
    }

public void Awake()
    {
        var behaviourSerializer = new XmlSerializer(typeof(Behaviour));

        using (TextReader reader = new StringReader(config.ToString()))
        {
            behaviour = (Behaviour)behaviourSerializer.Deserialize(reader);
        }
    }

    public void Start()
    {
        CoffeeWishes.Add(new coffeeWish { coffeeType = typeof(BlackCoffee), payment = 4.20f });
    }

    public void Update()
    {

    }

    public void OnReceiveObject(IDraggable draggable, GameObject gameObject)
    {
        if(draggable.Coffee?.GetType() == CoffeeWishes[currentCoffeeWishIndex].coffeeType)
        {
            GameManager.Instance.AddToCurrency(CoffeeWishes[currentCoffeeWishIndex].payment);
            gameObject.SetActive(false);
            gameObject.SetActive(true);

        }
    }

}

class coffeeWish
{
    public System.Type coffeeType;
    public float payment;
}
