using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    private float logTimer = 0;
    public GameObject ConsoleUI;
    public GameObject ConsoleButton;
    public GameObject InstanceConsole;
    public GameObject Preview;
    public GameObject PreviewButton;
    public TMP_InputField input;
    public TextMeshProUGUI Log;
    public StrStrDictionary Consoles = new();
    public StrStrDictionary ConsolesOrigin = new();
    public bool Autoclose = false;

    public void SwitchAutoclose()
    {
        Autoclose = !Autoclose;
    }
    private void Awake()
    {
        ConsoleInitialize();
        Preview = ConsoleUI.transform.Find("Preview").gameObject;
        Preview.SetActive(false);
    }
    private void Update()
    {
        if((Input.GetKey(KeyCode.L) && Input.GetKeyDown(KeyCode.LeftControl)) || (Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.LeftControl)))
        {
            VisibleConsole();
        }
        if(logTimer > 0)
        {
            logTimer -= Time.unscaledDeltaTime;
        }
        else
        {
            logTimer = 0;
            Log.gameObject.SetActive(false);
        }
    }
    private void VisibleConsole()
    {
        if (ConsoleUI.activeSelf)
        {
            input.text = "";
            GameManager.Inst.isStopped[1] = true;
        }
        else
        {
            GameManager.Inst.isStopped[1] = false;
        }
        ConsoleUI.SetActive(!ConsoleUI.activeSelf);
    }
    private void HideConsole()
    {
        GameManager.Inst.isStopped[1] = false;
        ConsoleUI.SetActive(false);
    }
    private void ConsoleInitialize()
    {
        string[] itemNamekor = { "캣츠아이", "로스러크", "매직볼", "매직애로우", "에어스트라이크", "마장도", "고장난시계", "공전보석","리본", "매직햇", "돋보기", "마법석", "얼어붙은이름표", "가속포탈", "프로텍트볼", "액상사료", "밤송이", "푸른별조각", "호신부", "보라색부적", "트램펄린", "꽃신", "관통탄환", "마장도장식", "당근" };
        string[] itemNameOrigin = { "CatsEye", "Rosruc", "MagicBall", "MagicArrow", "Airstrike", "MagicKnife", "BrokenWatch", "RevolutionJam", "Ribbon", "MagicHat", "MagnifyingGlass", "MagicStone", "FfrozenNameTag", "AcceleratePortal", "ProtectBall", "Churu", "SpikyBall", "FragmentBlueStar", "Talisman", "PurpleCharm", "Trampoline", "FlowerShoes", "PiercingBullet", "OrnamentMagicknife", "Carrot" };
        string[] itemName = new string[itemNameOrigin.Length];
        for (int i = 0; i < itemName.Length; i++)
        {
            itemName[i] = itemNameOrigin[i].ToLower();
        }
        string[] mobNamekor = { "노란별", "푸른별", "초록별", "붉은별", "하얀별" };
        string[] mobName = { "yellowexpstar", "blueexpstar", "greenexpstar", "redexpstar", "whiteexpstar" };
        string[] mobNameOrigin = { "YellowExpStar", "BlueExpStar", "GreenExpStar", "RedExpStar", "WhiteExpStar" };
        string[] monsterNamekor = { "테스트몬스터" };
        string[] monsterName = { "testenemy" };
        string[] monsterNameOrigin = { "TestEnemy" };

        for (int i = 0; i < itemName.Length; i++)
        {
            string lastChar = (itemNamekor[i][^1] - 0XAC00) % 28 > 0 ? "을" : "를";
            Consoles.Add($"increase{itemName[i]}", $"{itemNamekor[i]}의 레벨을 상승시킵니다");
            Consoles.Add($"decrease{itemName[i]}", $"{itemNamekor[i]}의 레벨을 하락시킵니다");
            Consoles.Add($"enable{itemName[i]}", $"{itemNamekor[i]}{lastChar} 활성화합니다");
            Consoles.Add($"disable{itemName[i]}", $"{itemNamekor[i]}{lastChar} 비활성화합니다");

            Consoles.Add($"setitemlv{itemName[i]}", $"{itemNamekor[i]}의 레벨을 변경합니다");

            //Origin Code
            ConsolesOrigin.Add($"increase{itemName[i]}", $"Increase {itemNameOrigin[i]}");
            ConsolesOrigin.Add($"decrease{itemName[i]}", $"Decrease {itemNameOrigin[i]}");
            ConsolesOrigin.Add($"enable{itemName[i]}", $"Enable {itemNameOrigin[i]}");
            ConsolesOrigin.Add($"disable{itemName[i]}", $"Disable {itemNameOrigin[i]}");

            ConsolesOrigin.Add($"setitemlv{itemName[i]}", $"Set ItemLv {itemNameOrigin[i]}");

            if (i < 5)
            {
                Consoles.Add($"evolution{itemName[i]}", $"{itemNamekor[i]}{lastChar} 진화시킵니다");
                Consoles.Add($"devolution{itemName[i]}", $"{itemNamekor[i]}{lastChar} 퇴화시킵니다");

                ConsolesOrigin.Add($"evolution{itemName[i]}", $"Evolution {itemNameOrigin[i]}");
                ConsolesOrigin.Add($"devolution{itemName[i]}", $"Devolution {itemNameOrigin[i]}");
            }
        }
        for (int i = 0; i < mobName.Length; i++)
        {
            string lastChar = mobNamekor[i][mobNamekor[i].Length - 1] - 0XAC00 % 28 > 0 ? "을" : "를";
            Consoles.Add($"spawn{mobName[i]}", $"{mobNamekor}{lastChar} 소환합니다");
            Consoles.Add($"remove{mobName[i]}", $"{mobNamekor}{lastChar} 모두 삭제합니다");

            //Origin Code
            ConsolesOrigin.Add($"spawn{mobName[i]}", $"Spawn {mobNameOrigin[i]}");
            ConsolesOrigin.Add($"remove{mobName[i]}", $"Remove {mobNameOrigin[i]}");
        }
        for (int i = 0; i < monsterName.Length; i++)
        {
            string lastChar = monsterNamekor[i][monsterNamekor[i].Length - 1] - 0XAC00 % 28 > 0 ? "을" : "를";
            Consoles.Add($"spawn{monsterName[i]}", $"{monsterNamekor}{lastChar} 소환합니다");
            Consoles.Add($"kill{monsterName[i]}", $"{monsterNamekor}{lastChar} 모두 처치합니다");
            Consoles.Add($"remove{monsterName[i]}", $"{monsterNamekor}{lastChar} 모두 삭제합니다");

            //Origin Code
            ConsolesOrigin.Add($"spawn{monsterName[i]}", $"Spawn {monsterNameOrigin[i]}");
            ConsolesOrigin.Add($"kill{monsterName[i]}", $"Kill {monsterNameOrigin[i]}");
            ConsolesOrigin.Add($"remove{monsterName[i]}", $"Remove {monsterNameOrigin[i]}");
        }

        Consoles.Add("clearall", "화면 내의 플레이어를 제외한 모든 오브젝트를 제거합니다");
        ConsolesOrigin.Add("clearall", "Clear All");

        Consoles.Add("clearbullet", "투사체를 모두 삭제합니다");
        ConsolesOrigin.Add("clearbullet", "Clear Bullet");

        Consoles.Add("addexp", "경험치를 획득합니다");
        ConsolesOrigin.Add("addexp", "Add Exp");

        Consoles.Add("subexp", "경험치를 감소시킵니다");
        ConsolesOrigin.Add("subexp", "Sub Exp");

        Consoles.Add("resetlv", "레벨을 초기화합니다");
        ConsolesOrigin.Add("resetlv", "Reset Lv");

        Consoles.Add("addlv", "레벨을 상승시킵니다");
        ConsolesOrigin.Add("addlv", "Add Lv");

        Consoles.Add("sublv", "레벨을 하락시킵니다");
        ConsolesOrigin.Add("sublv", "Sub Lv");

        Consoles.Add("collisionon", "플레이어의 충돌을 활성화합니다");
        ConsolesOrigin.Add("collisionon", "Collision On");

        Consoles.Add("collisionoff", "플레이어의 충돌을 비활성화합니다");
        ConsolesOrigin.Add("collisionoff", "Collision Off");

        Consoles.Add("setbulletspeed", "플레이어의 투사체의 속도를 변경합니다");
        ConsolesOrigin.Add("setbulletspeed", "Set BulletSpeed");

        Consoles.Add("setbulletdmg", "플레이어의 투사체의 데미지를 변경합니다");
        ConsolesOrigin.Add("setbulletdmg", "Set Bullet Dmg");

        Consoles.Add("setbulletsize", "플레이어의 투사체의 크기를 변경합니다");
        ConsolesOrigin.Add("setbulletsize", "Set Bullet Size");

        Consoles.Add("setdelay", "플레이어의 공격속도를 변경합니다");
        ConsolesOrigin.Add("setdelay", "Set Delay");

        Consoles.Add("sethp", "체력을 변경합니다");
        ConsolesOrigin.Add("sethp", "Set HP");

        Consoles.Add("setmaxhp", "최대 체력을 변경합니다");
        ConsolesOrigin.Add("setmaxhp", "Set MaxHP");

        Consoles.Add("setrecover", "체력재생을 변경합니다");
        ConsolesOrigin.Add("setrecover", "Set Recover");

        Consoles.Add("setspikedmg", "충돌데미지를 변경합니다");
        ConsolesOrigin.Add("setspikedmg", "Set SpikeDmg");

        Consoles.Add("setknockback", "모든 투사체의 넉백을 변경합니다");
        ConsolesOrigin.Add("setknockback", "Set Knockback");

        Consoles.Add("resetknockback", "모든 투사체의 넉백을 초기화합니다");
        ConsolesOrigin.Add("resetknockback", "Reset Knockback");

        Consoles.Add("setmagnet", "자력을 변경합니다");
        ConsolesOrigin.Add("setmagnet", "Set Magnet");

        Consoles.Add("resetmagnet", "자력을 초기화합니다");
        ConsolesOrigin.Add("resetmagnet", "Reset Magnet");

        Consoles.Add("createbutton", "콘솔버튼을 생성합니다");
        ConsolesOrigin.Add("createbutton", "Create Button");

        Consoles.Add("removebuttons", "콘솔버튼을 모두 삭제합니다");
        ConsolesOrigin.Add("removebuttons", "Remove Buttons");

        foreach(KeyValuePair<string, string> cs in Consoles)
        {
            GameObject obj = Instantiate(PreviewButton, Preview.transform);
            obj.name = cs.Key;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{ConsolesOrigin[cs.Key]} - {cs.Value}";
            obj.GetComponent<Button>().onClick.AddListener(() => { input.text = obj.name; });
        }
    }
    public void ConsolePreview()
    {
        if (string.IsNullOrEmpty(input.text))
        {
            Preview.SetActive(false);
        }
        else
        {
            foreach (Transform child in Preview.transform)
            {
                if (child.name.IndexOf(input.text.ToLower().Replace(" ", "")) > -1 && child.name != input.text.ToLower().Replace(" ", ""))
                {
                    child.gameObject.SetActive(true);
                    Preview.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
    public void InputConsole()
    {
        EnterConsole(input.text.ToLower().Replace(" ", ""));
    }
    public void EnterConsole(string text)
    {
        input.text = "";
        if(text.IndexOf("|")>-1)
        {
            string[] consoles = text.Split('|');
            foreach(string console in consoles)
            {
                EnterConsole(console);
            }
            return;
        }
        string[] weaponName = { "CatsEye", "Rosruc", "MagicBall", "MagicArrow", "Airstrike", "MagicKnife", "BrokenWatch", "RevolutionJam"};
        string[] itemNameOrigin = { "CatsEye", "Rosruc", "MagicBall", "MagicArrow", "Airstrike", "Ribbon", "MagicHat", "MagnifyingGlass", "MagicStone", "FrozenNameTag", "AcceleratePortal", "ProtectBall", "Churu", "SpikyBall", "FragmentBlueStar" };
        string[] mobName = { "yellowexpstar", "blueexpstar", "greenexpstar", "redexpstar", "whiteexpstar" };
        string[] monsterName = { "testenemy" };

        string[] numeralKeyword = { "setiemlv", "setknockback", "addexp", "subexp", "addlv", "sublv", "setbulletspeed", "setbulletdmg", "setbulletsize", "setdelay", "sethp", "setmaxhp", "setrecover", "setspikedmg", "setmagnet" };

        foreach (KeyValuePair<string, string> cs in Consoles)
        {
            if (text.IndexOf(cs.Key) > -1)
            {
                if(text.IndexOf("createbutton") > -1)
                {
                    GameObject obj = Instantiate(ConsoleButton, InstanceConsole.transform);
                    obj.name = "instancebutton";
                    if (text.IndexOf('"')>-1)
                    {
                        string tempName = text.Split('"')[1];
                        text = text.Split('"')[0] + text.Split('"')[2];
                        obj.GetComponent<Button>().onClick.AddListener(() => { EnterConsole(text.Replace("createbutton", "")); });
                        obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{tempName}";
                    }
                    else
                    {
                        obj.GetComponent<Button>().onClick.AddListener(() => { EnterConsole(text.Replace("createbutton", "")); });
                        obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{text.Replace("createbutton", "")}";
                    }
                    if(Autoclose)HideConsole();
                    return;
                }
                else if(text.IndexOf("removebuttons") > -1)
                {
                    foreach(Transform buttons in InstanceConsole.transform)
                    {
                        Destroy(buttons.gameObject);
                    }
                    if(Autoclose)HideConsole();
                    return;
                }
                foreach (string nk in numeralKeyword)
                {
                    if (text.IndexOf(nk) > -1)
                    {
                        string replaceText = text.Replace(nk, "");
                        int index = -1;
                        for (int i = 0; i < replaceText.Length; i++)
                        {
                            if (int.TryParse(replaceText[i].ToString(), out int dump))
                            {
                                index = i;
                                break;
                            }
                        }
                        if (index == -1)
                        {
                            SetLog("값이 존재하지 않습니다");
                            return;
                        }
                        string keyword = "";
                        if (!int.TryParse(replaceText, out int setvalue))
                        {
                            keyword = replaceText[..index];
                            setvalue = System.Convert.ToInt32(replaceText.Replace(keyword, ""));
                        }

                        switch (nk)
                        {
                            case "setitemlv":

                                foreach (string originKeyword in itemNameOrigin)
                                {
                                    if (originKeyword.ToLower() == keyword)
                                    {
                                        keyword = originKeyword;
                                        if (System.Array.IndexOf(weaponName, keyword) > -1)
                                        {
                                            keyword = $"{keyword}Spawner";
                                        }
                                    }
                                }
                                if (GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) != null)
                                {
                                    if(keyword.IndexOf("Spawner")>-1)
                                    {
                                        BulletSpawner item = GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) as BulletSpawner;
                                        item.Lv = setvalue;
                                        if (Autoclose) HideConsole();
                                        return;
                                    }
                                    else
                                    {
                                        Item item = GameManager.Inst.player.transform.Find("Items").GetComponent(System.Type.GetType(keyword)) as Item;
                                        item.SetLv(setvalue);
                                        if (Autoclose) HideConsole();
                                        return;
                                    }
                                }
                                else
                                {
                                    SetLog("해당 아이템이 존재하지 않습니다");
                                    return;
                                }
                            case "setknockback":
                                GameManager.Inst.player.Knockback = setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "addexp":
                                GameManager.Inst.player.Exp += setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "subexp":
                                GameManager.Inst.player.Exp -= setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "addlv":
                                for (int i = 0; i < setvalue; i++)
                                {
                                    GameManager.Inst.player.Exp += GameManager.Inst.player.maxExp;
                                }
                                if(Autoclose)HideConsole();
                                return;
                            case "sublv":
                                GameManager.Inst.player.Lv -= setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "setbulletspeed":
                                GameManager.Inst.player.BulletSpeedRatio -= setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "setbulletdmg":
                                GameManager.Inst.player.BulletDmgRatio -= setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "setbulletsize":
                                GameManager.Inst.player.BulletSize = setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "setdelay":
                                GameManager.Inst.player.DelayRatio -= setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "sethp":
                                GameManager.Inst.player.HP = setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "setmaxhp":
                                GameManager.Inst.player.maxhp = setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "setrecover":
                                GameManager.Inst.player.Recover = setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "setspikedmg":
                                GameManager.Inst.player.SpikeDmg = setvalue;
                                if(Autoclose)HideConsole();
                                return;
                            case "setmagnet":
                                GameManager.Inst.player.GetComponentInChildren<FragmentBluestar>().magnet.radius = setvalue;
                                if(Autoclose)HideConsole();
                                return;

                        }
                    }
                }
                if (text.IndexOf("increase") > -1)
                {
                    string keyword = text.Replace("increase", "");
                    foreach (string originKeyword in itemNameOrigin)
                    {
                        if (originKeyword.ToLower() == keyword)
                        {
                            keyword = originKeyword;
                            if (System.Array.IndexOf(weaponName, keyword) > -1)
                            {
                                keyword = $"{keyword}Spawner";
                            }
                        }
                    }
                    if (GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) != null)
                    {
                        if (keyword.IndexOf("Spawner") > -1)
                        {
                            BulletSpawner item = GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) as BulletSpawner;
                            item.Lv++;
                            if (Autoclose) HideConsole();
                            return;
                        }
                        else
                        {
                            Item item = GameManager.Inst.player.transform.Find("Items").GetComponent(System.Type.GetType(keyword)) as Item;
                            item.AddLv();
                            if (Autoclose) HideConsole();
                            return;
                        }
                    }
                    else
                    {
                        SetLog("해당 아이템이 존재하지 않습니다");
                        return;
                    }
                }
                if (text.IndexOf("decrease") > -1)
                {
                    string keyword = text.Replace("decrease", "");
                    foreach (string originKeyword in itemNameOrigin)
                    {
                        if (originKeyword.ToLower() == keyword)
                        {
                            keyword = originKeyword;
                            if (System.Array.IndexOf(weaponName, keyword) > -1)
                            {
                                keyword = $"{keyword}Spawner";
                            }
                        }
                    }
                    if (GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) != null)
                    {
                        if (GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) != null)
                        {
                            if (keyword.IndexOf("Spawner") > -1)
                            {
                                BulletSpawner item = GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) as BulletSpawner;
                                item.Lv--;
                                if (Autoclose) HideConsole();
                                return;
                            }
                            else
                            {
                                Item item = GameManager.Inst.player.transform.Find("Items").GetComponent(System.Type.GetType(keyword)) as Item;
                                item.SubLv();
                                if (Autoclose) HideConsole();
                                return;
                            }
                        }
                    }
                    else
                    {
                        SetLog("해당 아이템이 존재하지 않습니다");
                        return;
                    }
                }
                if (text.IndexOf("enable") > -1)
                {
                    string keyword = text.Replace("enable", "");
                    foreach (string originKeyword in itemNameOrigin)
                    {
                        if (originKeyword.ToLower() == keyword)
                        {
                            keyword = originKeyword;
                            if (System.Array.IndexOf(weaponName, keyword) > -1)
                            {
                                keyword = $"{keyword}Spawner";
                            }
                        }
                    }

                    if (GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) != null)
                    {
                        if (keyword.IndexOf("Spawner") > -1)
                        {
                            BulletSpawner item = GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) as BulletSpawner;
                            item.enabled = true;
                            if (Autoclose) HideConsole();
                            return;
                        }
                        else
                        {
                            Item item = GameManager.Inst.player.transform.Find("Items").GetComponent(System.Type.GetType(keyword)) as Item;
                            item.enabled = true;
                            if (Autoclose) HideConsole();
                            return;
                        }
                    }
                    else
                    {
                        SetLog("해당 아이템이 존재하지 않습니다");
                        return;
                    }
                }
                if (text.IndexOf("disable") > -1)
                {
                    string keyword = text.Replace("disable", "");
                    foreach (string originKeyword in itemNameOrigin)
                    {
                        if (originKeyword.ToLower() == keyword)
                        {
                            keyword = originKeyword;
                            if (System.Array.IndexOf(weaponName, keyword) > -1)
                            {
                                keyword = $"{keyword}Spawner";
                            }
                        }
                    }
                    if (GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) != null)
                    {
                        if(keyword.IndexOf("Spawner")>-1)
                        {
                            BulletSpawner item = GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) as BulletSpawner;
                            item.enabled = false;
                            if (Autoclose) HideConsole();
                            return;
                        }
                        else
                        {
                            Item item = GameManager.Inst.player.transform.Find("Items").GetComponent(System.Type.GetType(keyword)) as Item;
                            item.enabled = false;
                            if (Autoclose) HideConsole();
                            return;
                        }
                    }
                    else
                    {
                        SetLog("해당 아이템이 존재하지 않습니다");
                        return;
                    }
                }
                if (text.IndexOf("evolution") > -1)
                {
                    string keyword = text.Replace("evolution", "");
                    foreach (string originKeyword in itemNameOrigin)
                    {
                        if (originKeyword.ToLower() == keyword)
                        {
                            keyword = originKeyword;
                            if (System.Array.IndexOf(weaponName, keyword) > -1)
                            {
                                keyword = $"{keyword}Spawner";
                            }
                        }
                    }
                    if (GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) != null)
                    {
                        if (keyword.IndexOf("Spawner") > -1)
                        {
                            BulletSpawner item = GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) as BulletSpawner;
                            item.Evo = true;
                            if (Autoclose) HideConsole();
                            return;
                        }
                        else
                        {
                            SetLog("해당 아이템은 진화할 수 없습니다");
                            return;
                        }
                    }
                    else
                    {
                        SetLog("해당 아이템이 존재하지 않습니다");
                        return;
                    }
                }
                if (text.IndexOf("devolution") > -1)
                {
                    string keyword = text.Replace("devolution", "");
                    foreach (string originKeyword in itemNameOrigin)
                    {
                        if (originKeyword.ToLower() == keyword)
                        {
                            keyword = originKeyword;
                            if (System.Array.IndexOf(weaponName, keyword) > -1)
                            {
                                keyword = $"{keyword}Spawner";
                            }
                        }
                    }
                    if (GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) != null)
                    {
                        if (keyword.IndexOf("Spawner") > -1)
                        {
                            BulletSpawner item = GameManager.Inst.player.GetComponentInChildren(System.Type.GetType($"{keyword}")) as BulletSpawner;
                            item.Evo = false;
                            if (Autoclose) HideConsole();
                            return;
                        }
                        else
                        {
                            SetLog("해당 아이템은 퇴화할 수 없습니다");
                            return;
                        }
                    }
                    else
                    {
                        SetLog("해당 아이템이 존재하지 않습니다");
                        return;
                    }
                }
                if (text.IndexOf("spawn") > -1)
                {
                    string keyword = text.Replace("spawn", "");
                    if (System.Array.IndexOf(mobName, keyword) > -1)
                    {
                        GameObject obj = Instantiate(GameManager.Inst.Drops[keyword]);
                        obj.transform.position = new(Random.Range(-2, 3), 4, 0);
                        obj.name = keyword;
                        if(Autoclose)HideConsole();
                    }
                    else if (System.Array.IndexOf(monsterName, keyword) > -1)
                    {
                        GameManager.Inst.enemyManager.EnemySpawn(keyword);
                        if(Autoclose)HideConsole();
                    }
                    else
                    {
                        SetLog("해당 오브젝트가 존재하지 않습니다");
                        return;
                    }
                }
                if (text.IndexOf("kill") > -1)
                {
                    string keyword = text.Replace("kill", "");
                    if (System.Array.IndexOf(monsterName, keyword) > -1)
                    {
                        GameManager.Inst.enemyManager.KillEnemies(keyword);
                        if(Autoclose)HideConsole();
                    }
                    else
                    {
                        SetLog("해당 대상이 존재하지 않습니다");
                        return;
                    }
                }
                if (text.IndexOf("remove") > -1)
                {
                    string keyword = text.Replace("remove", "");
                    if (System.Array.IndexOf(mobName, keyword) > -1)
                    {
                        Item[] drops = GetComponents<Item>();
                        foreach (Item drop in drops)
                        {
                            if (drop.transform.parent == null)
                            {
                                Destroy(drop);
                            }
                        }
                        if(Autoclose)HideConsole();
                        return;
                    }
                    else if (System.Array.IndexOf(monsterName, keyword) > -1)
                    {
                        GameManager.Inst.enemyManager.RemoveEnemies(keyword);
                        if(Autoclose)HideConsole();
                        return;
                    }
                    else
                    {
                        SetLog("해당 오브젝트가 존재하지 않습니다");
                        return;
                    }
                }
                if (text.IndexOf("collisionoff") > -1)
                {
                    GameManager.Inst.player.GetComponent<BoxCollider2D>().enabled = false;
                    if(Autoclose)HideConsole();
                    return;
                }
                if (text.IndexOf("collisionon") > -1)
                {
                    GameManager.Inst.player.GetComponent<BoxCollider2D>().enabled = true;
                    if(Autoclose)HideConsole();
                    return;
                }
                if (text.IndexOf("resetknockback") > -1)
                {
                    GameManager.Inst.player.Knockback = 0;
                    if(Autoclose)HideConsole();
                    return;
                }
                if (text.IndexOf("clearall") > -1)
                {
                    Bullet[] bullets = FindObjectsOfType<Bullet>();
                    Enemy[] enemies = FindObjectsOfType<Enemy>();
                    Item[] drops = FindObjectsOfType<Item>();

                    foreach (Bullet bullet in bullets)
                    {
                        bullet.ReturnObject();
                    }
                    foreach (Enemy enemy in enemies)
                    {
                        Destroy(enemy.gameObject);
                    }
                    foreach (Item drop in drops)
                    {
                        if (drop.transform.parent == null)
                        {
                            Destroy(drop.gameObject);
                        }
                    }
                    if(Autoclose)HideConsole();
                    return;
                }
                if (text.IndexOf("clearbullet") > -1)
                {
                    Bullet[] bullets = FindObjectsOfType<Bullet>();
                    foreach (Bullet bullet in bullets)
                    {
                        bullet.ReturnObject();
                    }
                    if(Autoclose)HideConsole();
                    return;
                }
                if (text.IndexOf("resetmagnet") > -1)
                {
                    GameManager.Inst.player.GetComponentInChildren<FragmentBluestar>().magnet.radius = 0.6f;
                    GameManager.Inst.player.GetComponentInChildren<FragmentBluestar>().Lv = GameManager.Inst.player.GetComponentInChildren<FragmentBluestar>().Lv;
                }
            }
        }
        SetLog("해당 코드가 존재하지 않습니다");
    }
    private void SetLog(string text)
    {
        Log.gameObject.SetActive(true);
        Log.text = text;
        logTimer = 1.5f;
    }
}
