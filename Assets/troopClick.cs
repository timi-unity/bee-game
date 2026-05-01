using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class troopClick : MonoBehaviour
{
    public GameObject rangeObj;
    bool selected = false;

    public string name;
    public int upgrade = 0;
    public int totalCost;
    public List<string> upgradeDescriptions;
    public List<string> upgradePrice;

    public GameObject upgMenu;

    private void Start()
    {
        upgMenu = GameObject.Find("Canvas").transform.Find("UpgradeMenu").gameObject;
    }

    void OnMouseEnter()
    {
        selected = true;        
    }

    private void OnMouseExit()
    {
        selected = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            rangeObj.SetActive(false);
            upgMenu.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && selected)
        {
            selectTroop();
        }

        /*if (Input.GetKeyDown(KeyCode.Mouse0) && !selected)
        {
            rangeObj.SetActive(false);
            upgMenu.SetActive(false);
        }*/
    }

    private void selectTroop()
    {
        connectWithUpg();
        rangeObj.SetActive(true);
    }

    private void connectWithUpg()
    {
        GameObject menu = upgMenu;
        menu.SetActive(true);
        menu.transform.Find("Name").GetComponent<TextMeshProUGUI>().text=name;        
        menu.transform.Find("del").GetComponentInChildren<TextMeshProUGUI>().text = "$"+(totalCost/2);
        menu.transform.Find("upgDesc").GetComponent<TextMeshProUGUI>().text = "Nothing to do here";
        menu.transform.Find("upg").GetComponentInChildren<TextMeshProUGUI>().text = "Max";
        menu.transform.Find("upgN").GetComponent<TextMeshProUGUI>().text = "Upgrade " + upgrade;
        menu.transform.Find("profile").GetComponent<Image>().sprite = gameObject.transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>().sprite;

        menu.transform.Find("del").GetComponent<Button>().onClick.RemoveAllListeners();
        menu.transform.Find("del").GetComponent<Button>().onClick.AddListener(sell);

        if (upgrade == upgradePrice.Count)
        {
            menu.transform.Find("upg").GetComponent<Button>().onClick.AddListener(null);
            return;
        }

        menu.transform.Find("upgDesc").GetComponent<TextMeshProUGUI>().text = upgradeDescriptions[upgrade];
        menu.transform.Find("upg").GetComponentInChildren<TextMeshProUGUI>().text = "$" + upgradePrice[upgrade];

        //if (gameObject.GetComponentInParent<abejaScript>() != null)
        //{
            menu.transform.Find("upg").GetComponent<Button>().onClick.RemoveAllListeners();
            //upgrade
            if (gameObject.transform.parent.TryGetComponent<abejaScript>(out var a))
            {
                menu.transform.Find("upg").GetComponent<Button>().onClick.AddListener(() => a.Upgrade(upgrade));
            }
            if (gameObject.transform.parent.TryGetComponent<beetlepoopScript>(out var b))
            {
                menu.transform.Find("upg").GetComponent<Button>().onClick.AddListener(() => b.Upgrade(upgrade));
            }
            if (gameObject.transform.parent.TryGetComponent<spiderScript>(out var c))
            {
                menu.transform.Find("upg").GetComponent<Button>().onClick.AddListener(() => c.Upgrade(upgrade));
            }

        //}
    }

    public void sell()
    {
        GameObject.Find("gameManager").GetComponent<gameManager>().money += totalCost / 2;
        GameObject.Find("gameManager").GetComponent<gameManager>().updateMoney();
        upgMenu.SetActive(false);
        if(gameObject.transform.parent.gameObject.TryGetComponent<beetlepoopScript>(out var n))
        {
            GameObject.Find("gameManager").GetComponent<gameManager>().poopPlaced = false;
        }
        GameObject.Find("gameManager").GetComponent<gameManager>().cantidadTropas--;
        Destroy(gameObject.transform.parent.gameObject);
    }



}
