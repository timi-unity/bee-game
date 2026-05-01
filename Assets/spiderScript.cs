using UnityEngine;
using System.Collections.Generic;

public class spiderScript : MonoBehaviour
{
    public List<GameObject> inRangeEnemies = new List<GameObject>();
    public float range = 2.5f;
    public float rate = 1;
    public float dmg = 10;
    public float slowering = 1.45f;
    public Vector2 size = new Vector2(1,1);
    //
    public GameObject prefabDisparo;
    private float lastAttack = 0;
    private troopClick click;
    private gameManager gm;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
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

        disparo.GetComponent<bulletScript>().sizeAfter = size;
        disparo.GetComponent<bulletScript>().effectStrenght= slowering;

        disparo.GetComponent<bulletScript>().dmg = dmg;

        // Aplicar fuerza al Rigidbody2D
        Rigidbody2D rb = disparo.GetComponent<Rigidbody2D>();

        float fuerza = 8f; // Ajustá este valor según lo que quieras
        rb.AddForce(direction * fuerza, ForceMode2D.Impulse);

        GetComponentInChildren<SpriteRenderer>().gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);


    }

    private GameObject whoToAttack()
    {
        int a = 40;
        int iDeA = 0;
        for (int i = 0; i < inRangeEnemies.Count; i++)
        {
            if (inRangeEnemies[i].GetComponent<enemyScript>().eslabonActual < a)
            {
                a = inRangeEnemies[i].GetComponent<enemyScript>().eslabonActual;
                iDeA = i;
            }
        }
        return inRangeEnemies[iDeA];
    }

    void Start()
    {
        click = GetComponentInChildren<troopClick>();
        gm = GameObject.Find("gameManager").GetComponent<gameManager>();
    }


    void FixedUpdate()
    {
        if (inRangeEnemies.Count != 0 && Time.time > lastAttack + 2 / rate)
        {
            attack();
        }
    }

    public void Upgrade(int upg)
    {
        Debug.Log(upg);
        if (gm.money >= int.Parse(click.upgradePrice[upg]))
        {
            gm.money -= int.Parse(click.upgradePrice[upg]);
            switch (upg)
            {
                case 0:
                    size = new Vector2(1.7f,1.7f);
                    break;

                case 1:
                    slowering += 0.6f;
                    break;             
            }
            click.totalCost += int.Parse(click.upgradePrice[upg]);
            click.upgMenu.SetActive(false);
            gm.updateMoney();
            click.upgrade++;
        }
    }





}
