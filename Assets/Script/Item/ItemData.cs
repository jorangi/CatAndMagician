using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public Sprite icon;
    public Dictionary<string, Newtonsoft.Json.Linq.JToken> value = new();
    public bool isBulletSpawner = false;
    public bool inlaidAble = false;
    //public Dictionary<string, object> value = new();
}
