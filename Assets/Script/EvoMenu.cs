using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class EvoMenu : MonoBehaviour
{
    public GameObject levelupItemPrefab;
    public Transform list;


    private void OnEnable()
    {
        GameManager.Inst.isStopped[3] = true;
        List<ItemData> items = new();
        foreach (var item in GameManager.Inst.player.getItems)
        {
            if(GameManager.Inst.player.itemLevels[item.value["id"].ToString()] == 5 && !(FindObjectOfType(Type.GetType(item.value["id"].ToString())) as Item).Evo)
            {
                items.Add(item);
            }
        }
        if (items.Count == 0)
        {
            GameObject obj = Instantiate(levelupItemPrefab, list);
            ItemData data = GameManager.Inst.player.StardustData;
            LevelupItem tempData = obj.GetComponent<LevelupItem>();
            tempData.iconImage.sprite = data.icon;
            tempData.titleText.text = ItemManager.ConvertJToken<string>(data.value["name"])[0];
            tempData.descText.text = ItemManager.ConvertJToken<string>(data.value["desc"][0])[0];
            tempData.GetComponent<Button>().onClick.AddListener(() => { GameManager.Inst.player.StarDust += 50; gameObject.SetActive(false); });
        }
        else
        {
            List<ItemData> t = new();
            foreach (ItemData temp in items)
            {
                t.Add(temp);
            }

            for (int i = 0; i < Mathf.Min(items.Count, 4); i++)
            {
                GameObject obj = Instantiate(levelupItemPrefab, list);
                int index = UnityEngine.Random.Range(0, t.Count);
                ItemData data = t[index];
                LevelupItem tempData = obj.GetComponent<LevelupItem>();
                tempData.iconImage.sprite = data.icon;
                tempData.iconImage.GetComponentInChildren<TextMeshProUGUI>().text = "¡Ú";
                tempData.titleText.text = ItemManager.ConvertJToken<string>(data.value["name"])[0];
                tempData.descText.text = ItemManager.ConvertJToken<string>(data.value["desc"][0])[^1];
                tempData.GetComponent<Button>().onClick.AddListener(() => { GameManager.Inst.player.EvoItem(data.value["id"].ToString()); gameObject.SetActive(false); });

                t.Remove(t[index]);
            }
        }
    }
    private void OnDisable()
    {
        GameManager.Inst.isStopped[3] = false;
        foreach (Transform item in list)
        {
            Destroy(item.gameObject);
        }
    }
}
