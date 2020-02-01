using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Character : MonoBehaviour
{
    public TextAsset config;
    public Behaviour behaviour;


    public void Start()
    {
        var behaviourSerializer = new XmlSerializer(typeof(Behaviour));

        using (TextReader reader = new StringReader(config.ToString()))
        {
            behaviour = (Behaviour)behaviourSerializer.Deserialize(reader);

        }
        int x = 5;
    }

    public void Update()
    {

    }

}
