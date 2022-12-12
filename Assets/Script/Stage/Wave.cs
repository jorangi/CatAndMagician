using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Object/Wave", order = int.MaxValue)]
public class Wave : ScriptableObject
{
    public StrIntDictionary monsters;
}