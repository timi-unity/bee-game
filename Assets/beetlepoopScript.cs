using UnityEngine;

public class beetlepoopScript : MonoBehaviour
{
    private gameManager gm;
    private troopClick click;
    public int moneyGiven=0;
    public GameObject particlePrefab;

    void Start()
    {
        click = GetComponentInChildren<troopClick>();
        gm = GameObject.Find("gameManager").GetComponent<gameManager>();
    }

    public void Upgrade(int upg)
    {
        Debug.Log(upg);
        
        if (gm.money >= int.Parse(click.upgradePrice[upg]))
        {
            gm.money -= int.Parse(click.upgradePrice[upg]);
            gameObject.GetComponent<Animator>().SetTrigger("fase");
            switch (upg)
            {
                case 0:
                    moneyGiven = 50;
                    break;

                case 1:
                    moneyGiven = 100;
                    break;

                case 2:
                    moneyGiven = 150;
                    break;

                case 3:
                    moneyGiven = 250;
                    break;

                case 4:
                    moneyGiven = 1000;
                    break;
            }
            click.totalCost += int.Parse(click.upgradePrice[upg]);
            click.upgMenu.SetActive(false);
            gm.updateMoney();
            click.upgrade++;
        }
    }

    public void giveMeMyMoney()
    {
        gm.money += moneyGiven;
        var p = Instantiate(particlePrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1.01f), particlePrefab.transform.rotation);
        Destroy(p,4);
    }
}
