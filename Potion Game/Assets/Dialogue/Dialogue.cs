using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Dialogue 
{
    public string Name;
    public string DebugDesc;
    public List<string> Blocks;


    public Dialogue(string _name, string _desc)
    {
        Name = _name;
        DebugDesc = _desc;
        Blocks = new List<string>();
    }
}
