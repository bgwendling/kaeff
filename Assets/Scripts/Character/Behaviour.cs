using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRootAttribute("behaviour")]
public class Behaviour
{
    public Speech[] speeches;
    [XmlAttribute]
    public string defaultMood;
    [XmlAttribute]
    public string name;
}
