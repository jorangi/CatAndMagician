using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class ItemManager : MonoBehaviour
{
    public TextAsset json;
    public static Dictionary<string, ItemData>datas = new();

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        JObject jObject = JObject.Parse(json.text);
        foreach(JToken obj in jObject["Items"])
        {
            ItemData data = new();
            string id = string.Empty;
            foreach (JToken token in obj)
            {
                JProperty prop = token as JProperty;
                data.value.Add(prop.Name, prop.Value);
                if(prop.Name == "id")
                {
                    id = prop.Value.ToString();
                    GameManager.Inst.player.itemLevels.Add(id, 0);
                    GameManager.Inst.player.itemEvos.Add(id, false);
                    data.icon = Resources.Load<Sprite>($"ItemIcon/{id}");
                }
            }
            datas.Add(id, data);
        }
    }
    public static T[] ConvertJToken<T>(JToken value)
    {
        List<T> values = new();
        int i = 0;
        foreach (JToken val in value)
        {
            values.Add((T)System.Convert.ChangeType(val, typeof(T)));
            i++;
        }
        return values.ToArray();
    }
    public static string Interpret(string id, string origin)
    {
        string[] division = origin.Split('<', '>');
        for(int i = 0; i<division.Length; i++)
        {
            if(i%2==1)
            {
                string re = string.Empty;
                string[] val = division[i].Split(':');
                if(val.Length == 1)
                {
                    re = ConvertJToken<string>(datas[id].value[val[0]])[0];
                }
                else
                {
                    if (val[1] == "lv")
                    {
                        re = ConvertJToken<string>(datas[id].value[val[0]])[GameManager.Inst.player.itemLevels[id]];
                    }
                    else
                    {
                        re = datas[id].value[val[0]][System.Convert.ToInt32(val[1])].ToString();
                    }
                }
                division[i] = re;
            }
        }
        string result = string.Join("", division);
        return result;
    }
}
