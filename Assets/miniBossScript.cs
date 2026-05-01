using UnityEngine;

public class miniBossScript : MonoBehaviour
{
    public GameObject prefabAtaque;
    float last = 0;
    private Animator an;
    public void atacar()
    {
        Destroy(Instantiate(prefabAtaque,gameObject.transform.position,Quaternion.identity),3f);
    }

    private void Start()
    {
        an = gameObject.GetComponent<Animator>();
        GetComponent<enemyScript>().health = 3000;
    }

    private void Update()
    {
        if(Time.time > last + 40f)
        {
            an.SetTrigger("attack");
            last = Time.time;
        }
    }
}
