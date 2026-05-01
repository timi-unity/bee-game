using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class gameManager : MonoBehaviour
{
    private bool lost = false;
    public float maxHealth = 100;
    public float towerHealth = 100;
    public int wave = 1;
    public int enemyAmount = 4;
    private float vidaPos = 75;
    //
    public int money=500;
    public bool yaSpawnearonTodosLosPibes = false;

    public TextMeshProUGUI waveText, moneyText;

    //public List<GameObject> enemyPrefabs;
    public List<GameObject> possibleEnemy;
    //
    private List<GameObject> aliveEnemies = new List<GameObject>();
    private bool fastTime = false;
    public GameObject buttonWave;
    public GameObject gameOver;
    //troops
    public GameObject prefabPlaceTroop;
    public List<GameObject> troopsPrefabs;
    public bool poopPlaced = false;
    public int cantidadTropas = 0;
    public GameObject queenAnt;
    //UI
    public Transform hpBar;
    public TextMeshProUGUI hpText;
    public GameObject progressBar;
    int aparecidos = 0;
    public TextMeshProUGUI x2;
    public GameObject textoMax;

    private System.Random rnd = new System.Random();
    private GameObject boss = null;
    private void Start()
    {
        //possibleEnemy.Add(enemyPrefabs[0]);
        //StartCoroutine(spawnEnemy());
        money = 500;
        moneyText.text = "Money: $" + money;
        waveText.text = "Wave: " + wave;
    }

    public void nextWave()
    {
        GameObject[] objetosConNombre = GameObject.FindObjectsOfType<Transform>()
            .Where(t => t.name == "tropa(Clone)")
            .Select(t => t.gameObject)
            .ToArray();

        progressBar.transform.localScale = new Vector3(1 - (aparecidos * 1f / enemyAmount), 1, 1);

        foreach (var item in objetosConNombre)
        {
            item.gameObject.GetComponent<abejaScript>().inRangeEnemies = new List<GameObject>();
        }

        if (lost)
        {
            return;
        }
        buttonWave.SetActive(false);
        if (wave != 0 && wave < 15)
        {
            enemyAmount = (int)(enemyAmount * 1.5f);           
        }              
        wave++;
        StartCoroutine(spawnEnemy());
        waveText.text = "Wave: "+wave;
        updateMoney();
        yaSpawnearonTodosLosPibes = false;

        if(wave == 10)
        {
            GameObject.Find("Canvas").transform.Find("X4").gameObject.SetActive(true);
        }
        else if (wave > 15)
        {
            vidaPos *= 1.2f;
        }
    }

    public void enemyDies(GameObject enemy,bool givesMoney)
    {
        aliveEnemies.Remove(enemy);
        Destroy(enemy);

        if(aliveEnemies.Count == 0 && yaSpawnearonTodosLosPibes)
        {
            buttonWave.SetActive(true);
            money += 200;
            if (poopPlaced)
            {
                GameObject.Find("Escarabajo(Clone)").GetComponent<beetlepoopScript>().giveMeMyMoney();
            }
            updateMoney();
        }

        if (givesMoney)
        {
            money += rnd.Next(3,9);
        }

        moneyText.text = "Money: $" + money;

        aparecidos++;
        progressBar.transform.localScale = new Vector3(1-(aparecidos * 1f / enemyAmount), 1, 1);

        if(wave == 12 && aparecidos >= (int)(4 * Math.Pow(1.5f, wave - 1))-30 && boss != null && aliveEnemies.Count == 1)
        {
            boss.SetActive(true);
        }

        if (wave % 6 == 0 && wave > 13 && aparecidos >= (int)(4 * Math.Pow(1.5f, wave - 1)) - 30 && boss != null && aliveEnemies.Count == 1)
        {
            boss.SetActive(true);
        }


    }

    public void enemyWins(GameObject enemy)
    {
        towerHealth -= enemy.GetComponent<enemyScript>().health;
        enemyDies(enemy,false);

        if(towerHealth <= 0)
        {
            lose();
        }

        hpBar.localScale = new Vector3(towerHealth/maxHealth,1,1);
        hpText.text = towerHealth + "/" + maxHealth;
    }

    IEnumerator spawnEnemy()
    {
        if(wave == 12)
        {
            boss = Instantiate(queenAnt, GameObject.Find("Path").transform.Find("spawn").gameObject.transform.position, Quaternion.identity);
            boss.SetActive(false);
        }
        else if (wave % 6 == 0 && wave > 13)
        {
            boss = Instantiate(queenAnt, GameObject.Find("Path").transform.Find("spawn").gameObject.transform.position, Quaternion.identity);
            boss.SetActive(false);
        }
        aparecidos = 0;
        bool spawnSpeedsters = wave >= 5;
        bool spawnKing = wave >= 8;
        yield return new WaitForSeconds(1);
        System.Random rnd = new System.Random();
        for (int i = 0; i < enemyAmount; i++)
        {
            int enemy = 0;
            int barsovia = rnd.Next(0, 100);

            if (barsovia <= 20)
            {
                if (spawnSpeedsters)
                    enemy = 1;
            }
            else if (barsovia <= 25)
            {
                if (spawnKing)
                    enemy = 2;
            }

            var p = Instantiate(possibleEnemy[enemy]);
            aliveEnemies.Add(p);

            
            if (wave > 1)
            {
                if(wave <= 15)
                {
                    p.GetComponent<enemyScript>().health = (p.GetComponent<enemyScript>().health * wave) / 2f;
                }
                else
                {
                    p.GetComponent<enemyScript>().health = vidaPos; 
                }
                
            }
            

            if(i == enemyAmount-1)
            {
                yaSpawnearonTodosLosPibes = true;
            }
            float timeToWait;
            if(wave<= 12)
            {
                timeToWait = 0.3f + 2f / (1 + (wave / 2));
            }
            else{
                timeToWait= 0.05f + 2.2f / (1 + (wave / 2));
            }
            yield return new WaitForSeconds(timeToWait);
        }        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            /*if(money >= troopsPrefabs[0].GetComponentInChildren<troopClick>().totalCost)
            {
                var p=Instantiate(prefabPlaceTroop);
                p.GetComponent<placingScript>().toPlaceTroop = troopsPrefabs[0];
            }   */
            spawnSpawner(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !poopPlaced)
        {
            /*if (money >= troopsPrefabs[1].GetComponentInChildren<troopClick>().totalCost)
            {
                var p = Instantiate(prefabPlaceTroop);
                p.GetComponent<placingScript>().toPlaceTroop = troopsPrefabs[1];
                p.GetComponent<placingScript>().placingPoop = true;
                p.GetComponentInChildren<SpriteRenderer>().sprite = troopsPrefabs[1].gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            }*/
            spawnSpawner(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            /*if (money >= troopsPrefabs[2].GetComponentInChildren<troopClick>().totalCost)
            {
                var p = Instantiate(prefabPlaceTroop);
                p.GetComponent<placingScript>().toPlaceTroop = troopsPrefabs[2];
                p.GetComponentInChildren<SpriteRenderer>().sprite = troopsPrefabs[2].gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            }*/
            spawnSpawner(2);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(GameObject.Find("placeTropa(Clone)"));
        }
    }

    public void spawnSpawner(int who)
    {
        if(cantidadTropas > 32)
        {
            Instantiate(textoMax,GameObject.Find("Canvas").transform);
            return;
        }
        if(who==1 && poopPlaced)
        {
            return;
        }
        if (money >= troopsPrefabs[who].GetComponentInChildren<troopClick>().totalCost)
        {
            var p = Instantiate(prefabPlaceTroop);
            p.GetComponent<placingScript>().toPlaceTroop = troopsPrefabs[who];
            p.GetComponentInChildren<SpriteRenderer>().sprite = troopsPrefabs[who].gameObject.GetComponentInChildren<SpriteRenderer>().sprite;

            if (who == 1)
            {
                p.GetComponent<placingScript>().placingPoop = true;
            }
        }
    }

    public void GastarPlata(int plata)
    {
        money -= plata;
        moneyText.text = "Money: $" + money;
    }

    public void doubleTime()
    {
        if(Time.timeScale != 1)
        {
            Time.timeScale = 1;
            x2.text = "X2";
        }
        else
        {
            Time.timeScale = 2;
            x2.text = "X1";
        }        
    }

    public void cuadrupleTime()
    {
        Time.timeScale = 4;
        x2.text = "X1";
    }

    private void lose()
    {
        gameOver.SetActive(true);
        lost = true;
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void updateMoney()
    {
        moneyText.text = "Money: $" + money;
    }

}
