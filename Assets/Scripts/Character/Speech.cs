using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;


public class Speech
{
    [XmlAttribute]
    public int id;
    public List<Statement> statements;

}


public class Statement
{

}
