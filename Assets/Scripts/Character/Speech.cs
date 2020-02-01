using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlType("speech")]
public class Speech
{
    [XmlAttribute]
    public int id;
    public Entry[] statements;

}

[XmlType("entry")]
public class Entry
{
    public string text;
    public string moodChange;
    public Option[] choice;
}

[XmlType("option")]
public class Option
{
    [XmlAttribute]
    public int result;
    [XmlText]
    public string value;
   

}