using UnityEngine;
using System.Collections;

public class enemyScript : MonoBehaviour
{
    public int eslabonActual=0;
    pathScript ps;
    public float speed = 1;
    public float health = 10;
    private float maxHealth = 10;
    public GameObject handle;
    public GameObject healthUI;

    public bool slowed=false;
    public bool poisoned = false;
    public bool girar = true;

    private void Start()
    {
        ps = GameObject.Find("Path").GetComponent<pathScript>();
        gameObject.transform.position = ps.eslabones[0].position;
        maxHealth = health;
    }

    void changeEslabon()
    {
        Vector3 direction = transform.position - ps.eslabones[eslabonActual].position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(girar)
        transform.rotation = Quaternion.Euler(0f, 0f, angle-90);


        Transform destino = ps.eslabones[eslabonActual];
        Vector3 dir = (destino.position - transform.position).normalized;
        transform.position += dir * speed*0.05f;

        if (Vector3.Distance(transform.position, destino.position) < 0.1f)
        {
            if (eslabonActual == 21)
            {
                GameObject.Find("gameManager").GetComponent<gameManager>().enemyWins(gameObject);
            }
            else
            {
                eslabonActual++;
            }            
        }
    }

    public void takeDmg(float dmg)
    {
        Debug.Log("dolor");
        health -= dmg;

        if(health <= 0)
        {
            die();
        }

        handle.transform.localScale = new Vector3((health/maxHealth), 1, 1);
    }
    private void die()
    {
        GameObject.Find("gameManager").GetComponent<gameManager>().enemyDies(gameObject,true);
    }

    private void FixedUpdate()
    {
        changeEslabon();
    }

    void OnMouseEnter()
    {
        healthUI.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        healthUI.gameObject.SetActive(false);
    }

    public IEnumerator poison(float dmg)
    {
        if (poisoned) { yield break; }
        Debug.Log("fui envenenado");
        poisoned = true;
        GetComponent<SpriteRenderer>().color = Color.green;
        float damage = dmg / 4;
        for (int i = 0; i < 20; i++)
        {
            Debug.Log("ay"+i);
            takeDmg(damage);
            yield return new WaitForSeconds(0.5f);
        }
        GetComponent<SpriteRenderer>().color = Color.white;
        poisoned = false;
        Debug.Log("no fui envenenado");
    }

    public void autoEnvenenarme(float dmg)
    {
        StartCoroutine(poison(dmg));
    }

}
