using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName ="Scriptable Object/ItemData", order = int.MaxValue)]
public class ItemData:ScriptableObject
{
    public string id;
    public Sprite icon;
    public string title_ko;
    public string title_en;
    public float[] val;
    public string[] itemUp_ko = { };
    public string[] itemUp_en = { };
    public string Upgraded_ko;
}
