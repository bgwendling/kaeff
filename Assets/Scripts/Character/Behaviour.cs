using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRootAttribute("Behaviour")]
public class Behaviour
{
    public List<Speech> dialogueEntries;
    [XmlAttribute]
    public string defaultMood;
}
