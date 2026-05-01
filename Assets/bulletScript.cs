using UnityEngine;
using System.Collections;
public class bulletScript : MonoBehaviour
{
    public float dmg;
    public bool veneno=false;
    public bool strongVeneno = false;
    public Vector2 sizeAfter;
    public float effectStrenght;

    public GameObject after;

    private void Start()
    {
        Destroy(gameObject,5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            if (after != null)
            {
                var l=Instantiate(after, collision.gameObject.transform.position, Quaternion.identity);
                if(l.TryGetComponent<webScript>(out var g))
                {
                    g.amountOfSlowness = effectStrenght;
                    l.transform.localScale = sizeAfter;
                    
                }
            }
            collision.gameObject.GetComponent<enemyScript>().takeDmg(dmg);
            if (veneno)
            {
                float poisonDamage = dmg;

                if (strongVeneno)
                {
                    poisonDamage *= 2;
                }

                collision.gameObject.GetComponent<enemyScript>().autoEnvenenarme(poisonDamage);
            }            
             Destroy(gameObject);
            
           
            
        }       
    }

    
}
