using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[System.Serializable]
public class UIText
{
    public TextMeshProUGUI AD;
    public TextMeshProUGUI AP;
    public TextMeshProUGUI AS;
    public TextMeshProUGUI Speed;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI Coin;
}
[System.Serializable]
public class EquipData
{
    public GameObject [] EquipmentPlace = new GameObject[4];
    public GameObject [] EquipmentInfo = new GameObject[4];
}
public enum Equipment
{
    Weapon,
    Helmet,
    Armor,
    Shoes
}
public class EquipController : MonoBehaviour
{
    public static EquipController instance;
    public UIText texts;
    public EquipData equipData;
    public CharacterDatas characterDatas;
    public static Action SaveAndReferesh;

    [SerializeField]
    private GameObject presetItem;
    public Transform Inventory;

    // Start is called before the first frame update
    public void Awake()
    {
        instance = this;
        SaveAndReferesh = () => { SaveData(); };
        characterDatas =DataManager.instance.Load();
        RefreshState();
        SetItems();
    }
    public void GoHome()
    {
        SoundManager.instance.PlayBTNSound("Menu_Select_00");
        SceneManager.LoadScene("MainScene");
        //LoadingSceneController.Instance.LoadScene("MainScene");
    }
    public void SaveData(){
        characterDatas = DataManager.instance.Load();
        RefreshState();
        DataManager.instance.Save(characterDatas); 
    }
    public void RefreshState() {
        texts.AD.text = characterDatas.AD.ToString("0.##");
        texts.AP.text = characterDatas.AP.ToString("0.##");
        texts.AS.text = characterDatas.AS.ToString("0.##");
        texts.Speed.text = characterDatas.Speed.ToString("0.##");
        texts.HP.text = characterDatas.HP.ToString("0.##");
        texts.Coin.text = characterDatas.Coin.ToString();
    }
    public void SetItems()
    {
        UserItemDatas items = DataManager.instance.ItemDatasLoad();
        for (int i = 0; i < items.ItemRows.Count; i++)
        {
            var item = Instantiate(presetItem, Inventory);
            item.GetComponent<MyItems>().itemData = items.ItemRows[i];
            item.GetComponent<MyItems>().ItemIndex = i;
            item.GetComponent<MyItems>().Check.SetActive(items.ItemRows[i].isEquip);
            item.GetComponent<MyItems>().ItemImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", items.ItemRows[i].type, items.ItemRows[i].ItemName));
            item.GetComponent<MyItems>().reinForceLevelText.text = "LV."+items.ItemRows[i].reinForceLevel.ToString();
            //인벤토리 아이템 장착
            if (items.ItemRows[i].isEquip) { 
                int varNum = 0;
                switch (items.ItemRows[i].type)
                {
                    case "Weapon":
                        varNum = (int)Equipment.Weapon;
                        break;
                    case "Helmet":
                        varNum = (int)Equipment.Helmet;
                        break;
                    case "Armor":
                        varNum = (int)Equipment.Armor;
                        break;
                    case "Shoes":
                        varNum = (int)Equipment.Shoes;
                        break;
                    default:
                        Debug.Log("NoneType");
                        break;
                }
                /*if (null != equipData.EquipmentInfo[varNum])
                    equipData.EquipmentInfo[varNum].GetComponent<MyItems>().Equip(); // 같은 타입 장착해제*/
                equipData.EquipmentPlace[varNum].GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", items.ItemRows[i].type, items.ItemRows[i].ItemName));
                equipData.EquipmentPlace[varNum].SetActive(true);
                equipData.EquipmentInfo[varNum] = item;
            }
        }
    }
    public void EquipPlayer()
    {
        // 텍스쳐 변경
        
    }
    public void onClickEquipPlace(int num)
    {
        SoundManager.instance.PlayBTNSound("Menu_Select_00");
        equipData.EquipmentInfo[num].GetComponent<MyItems>().OpenEquipDeatilCanvas();
    }
}
