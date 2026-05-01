using UnityEngine;
using System.Collections.Generic;

public class abejaScript : MonoBehaviour
{
    public List<GameObject> inRangeEnemies = new List<GameObject>();
    public float range = 3;
    public float rate = 1;
    public float dmg = 4;

    bool poisoner = false;
    bool doublePoison = false;
    //
    public GameObject prefabDisparo;
    private float lastAttack = 0;
    private troopClick click;
    private gameManager gm;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            if (!inRangeEnemies.Contains(collision.gameObject))
            {
                inRangeEnemies.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            if (inRangeEnemies.Contains(collision.gameObject))
            {
                inRangeEnemies.Remove(collision.gameObject);
            }
        }
    }

    public void attack()
    {
        lastAttack = Time.time;
        GameObject enemy = whoToAttack();
        Vector3 direction = (enemy.transform.position - transform.position).normalized;

        // Calcular ángulo de rotación para que mire hacia el enemigo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instanciar el proyectil rotado
        GameObject disparo = Instantiate(prefabDisparo, transform.position, Quaternion.Euler(0f, 0f, angle + 90));

        disparo.GetComponent<bulletScript>().veneno = poisoner;

        disparo.GetComponent<bulletScript>().strongVeneno = doublePoison;

        disparo.GetComponent<bulletScript>().dmg = dmg;

        // Aplicar fuerza al Rigidbody2D
        Rigidbody2D rb = disparo.GetComponent<Rigidbody2D>();
        
        float fuerza = 14f; // Ajustá este valor según lo que quieras
        rb.AddForce(direction * fuerza, ForceMode2D.Impulse);

        GetComponentInChildren<SpriteRenderer>().gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle+180);


    }

    private GameObject whoToAttack()
    {
        int a= 0;
        int iDeA = 0;
        for (int i = 0; i < inRangeEnemies.Count; i++)
        {
            if(inRangeEnemies[i].GetComponent<enemyScript>().eslabonActual > a)
            {
                a = inRangeEnemies[i].GetComponent<enemyScript>().eslabonActual;
                iDeA = i;
            }
        }
        return inRangeEnemies[iDeA];
    }

    void Start()
    {
        click=GetComponentInChildren<troopClick>();
        gm = GameObject.Find("gameManager").GetComponent<gameManager>();
    }

    
    void FixedUpdate()
    {
        if(inRangeEnemies.Count != 0 && Time.time > lastAttack + 2/rate)
        {
            attack();
        }
    }

    public void Upgrade(int upg)
    {
        Debug.Log(upg);
        if(gm.money >= int.Parse(click.upgradePrice[upg]))
        {
            gm.money -= int.Parse(click.upgradePrice[upg]);
            switch (upg)
            {
                case 0:
                    range += 1;
                    rate += 0.5f;
                    GetComponent<CircleCollider2D>().radius = range;
                    gameObject.transform.Find("range").transform.localScale = new Vector3(range*3, range * 3, 1);
                    break;

                case 1:
                    poisoner = true;
                    break;

                case 2:
                    dmg += 2;
                    break;

                case 3:
                    doublePoison = true;
                    break;
            }
            click.totalCost += int.Parse(click.upgradePrice[upg]);            
            click.upgMenu.SetActive(false);
            gm.updateMoney();
            click.upgrade++;
        }        
    }
    




}
