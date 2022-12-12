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


    private void OnEnable()
    {
        GameManager.Inst.player.levelupCount--;
        Time.timeScale = 0.0f;
        GameManager.Inst.player.input.Disable();

        List<ItemData> items = new();
        if(GameManager.Inst.player.getItems.Count < 12)
        {
            items = GameManager.Inst.player.itemDatas.ToList();
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
            if (GameManager.Inst.player.itemLevels[items[i].id] == items[i].itemUp_ko.Length)
            {
                items.Remove(items[i]);
            }
        }
        if(items.Count == 0)
        {
            GameObject obj = Instantiate(levelupItemPrefab, list);
            ItemData data = GameManager.Inst.player.StardustData;
            LevelupItem tempData = obj.GetComponent<LevelupItem>();
            tempData.iconImage.sprite = data.icon;
            tempData.titleText.text = data.title_ko;
            tempData.descText.text = data.itemUp_ko[0];
            tempData.GetComponent<Button>().onClick.AddListener(() => { GameManager.Inst.player.StarDust += 50; gameObject.SetActive(false); });
        }
        else
        {
            List<ItemData> t = new();
            foreach(ItemData temp in items)
            {
                t.Add(temp);
            }

            for(int i = 0; i < Mathf.Min(items.Count, 4); i++)
            {
                GameObject obj = Instantiate(levelupItemPrefab, list);
                int index = Random.Range(0, t.Count);
                ItemData data = t[index];
                LevelupItem tempData = obj.GetComponent<LevelupItem>();
                tempData.iconImage.sprite = data.icon;
                switch(GameManager.Inst.player.itemLevels[data.id])
                {
                    case 0:
                        tempData.iconImage.GetComponentInChildren<TextMeshProUGUI>().text = "¥°";
                        break;
                    case 1:
                        tempData.iconImage.GetComponentInChildren<TextMeshProUGUI>().text = "¥±";
                        break;
                    case 2:
                        tempData.iconImage.GetComponentInChildren<TextMeshProUGUI>().text = "¥²";
                        break;
                    case 3:
                        tempData.iconImage.GetComponentInChildren<TextMeshProUGUI>().text = "¥³";
                        break;
                    case 4:
                        tempData.iconImage.GetComponentInChildren<TextMeshProUGUI>().text = "¥´";
                        break;
                }
                tempData.titleText.text = data.title_ko;
                tempData.descText.text = data.itemUp_ko[GameManager.Inst.player.itemLevels[data.id]];
                tempData.GetComponent<Button>().onClick.AddListener(() => { GameManager.Inst.player.AddItem(data.id); gameObject.SetActive(false); });

                t.Remove(t[index]);
            }
        }
    }
    private void OnDisable()
    {
        Time.timeScale = 1.0f;
        GameManager.Inst.player.input.Enable();
        foreach (Transform item in list)
        {
            Destroy(item.gameObject);
        }
    }
}
