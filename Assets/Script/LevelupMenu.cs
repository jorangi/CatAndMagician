using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class LevelupMenu : MonoBehaviour
{
    public GameObject levelupItemPrefab;
    public Transform list;
    public int LevelUpItemCount = 0;

    private void OnEnable()
    {

        if (GameManager.Inst.player.GoldCubeItems)
            LevelUpItemCount = 3;
        else
            LevelUpItemCount = 4;

        GameManager.Inst.player.levelupCount--;
        GameManager.Inst.isStopped[2] = true;

        List<ItemData> items = new();
        if(GameManager.Inst.player.getItems.Count < 12)
        {
            foreach(KeyValuePair<string, Item> i in GameManager.Inst.player.Items)
            {
                items.Add(i.Value.data);
            }
        }
        else
        {
            foreach(ItemData data in GameManager.Inst.player.getItems)
            {
                items.Add(data);
            }
        }
        for(int i = items.Count-1; i>=0; i--)
        {
            if (GameManager.Inst.player.itemLevels[items[i].value["id"].ToString()] == 5)
            {
                items.Remove(items[i]);
                continue;
            }
            if (items[i].value["id"].ToString() == "GoldCube" && GameManager.Inst.player.itemLevels[items[i].value["id"].ToString()] == 0)
            {
                items.Remove(items[i]);
                continue;
            }
            if (items[i].value["id"].ToString().IndexOf("Inlaid") == 0 && GameManager.Inst.player.itemLevels[items[i].value["id"].ToString()] == 0)
            {
                items.Remove(items[i]);
                continue;
            }
            if (GameManager.Inst.player.itemLevels["MagicKnife"] == 0 && items[i].value["id"].ToString() == "OrnamentMagicKnife")
            {
                items.Remove(items[i]);
                continue;
            }
            if (GameManager.Inst.player.itemLevels["Carrot"] == 0 && items[i].value["id"].ToString() == "Carrot")
            {
                items.Remove(items[i]);
                continue;
            }
        }
        if(items.Count == 0)
        {
            GameObject obj = Instantiate(levelupItemPrefab, list);
            ItemData data = GameManager.Inst.player.StardustData;
            LevelupItem tempData = obj.GetComponent<LevelupItem>();
            tempData.iconImage.sprite = data.icon;
            tempData.titleText.text = "별의 모래";
            tempData.descText.text = "메인메뉴에서 스킨을 구매할 수 있습니다.";
            tempData.GetComponent<Button>().onClick.AddListener(() => { GameManager.Inst.player.StarDust += 50; gameObject.SetActive(false); });
        }
        else
        {
            List<ItemData> t = new();
            foreach(ItemData temp in items)
            {
                t.Add(temp);
            }
            for (int i = 0; i < Mathf.Min(items.Count, LevelUpItemCount); i++)
            {
                GameObject obj = Instantiate(levelupItemPrefab, list);
                int index = Random.Range(0, t.Count);
                ItemData data = t[index];
                LevelupItem tempData = obj.GetComponent<LevelupItem>();
                tempData.iconImage.sprite = data.icon;
                TextMeshProUGUI _t = tempData.iconImage.GetComponentInChildren<TextMeshProUGUI>();
                switch (GameManager.Inst.player.itemLevels[data.value["id"].ToString()])
                {
                    case 0:
                        _t.text = "Ⅰ";
                        break;
                    case 1:
                        _t.text = "Ⅱ";
                        break;
                    case 2:
                        _t.text = "Ⅲ";
                        break;
                    case 3:
                        _t.text = "Ⅳ";
                        break;
                    case 4:
                        _t.text = "Ⅴ";
                        break;
                }
                tempData.titleText.text = ItemManager.ConvertJToken<string>(data.value["name"])[0];
                tempData.descText.text = ItemManager.Interpret(data.value["id"].ToString(), ItemManager.ConvertJToken<string>(data.value["desc"][0])[Mathf.Min(GameManager.Inst.player.itemLevels[data.value["id"].ToString()], ItemManager.ConvertJToken<string>(data.value["desc"][0]).Length - 2)]);
                Canvas.ForceUpdateCanvases();
                tempData.GetComponent<RectTransform>().sizeDelta = new(1000, tempData.descText.GetComponent<RectTransform>().rect.size.y + 125.0f);
                tempData.GetComponent<Button>().onClick.AddListener(() => { GameManager.Inst.player.AddItem(data.value["id"].ToString()); gameObject.SetActive(false); });

                t.Remove(t[index]);
            }
        }
    }
    private void OnDisable()
    {
        GameManager.Inst.player.GoldCubeItems = false;
        GameManager.Inst.isStopped[2] = false;
        foreach (Transform item in list)
        {
            Destroy(item.gameObject);
        }
    }
}
