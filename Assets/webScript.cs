using UnityEngine;
using System.Collections.Generic;

public class webScript : MonoBehaviour
{
    public List<GameObject> inRangeEnemies = new List<GameObject>();
    float appearance;
    public float amountOfSlowness;

    private void Awake()
    {
        appearance = Time.time;
        System.Random rnd = new System.Random();
        transform.rotation = Quaternion.Euler(new Vector3(0,0, rnd.Next(0, 361)));
        Destroy(gameObject, 1.5f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            bool yaEsta = false;
            foreach (var item in inRangeEnemies)
            {
                if (item == collision.gameObject) // comparar contra el enemigo
                {
                    yaEsta = true;
                    break; // opcional: salí del bucle si ya lo encontraste
                }
            }
            if (!yaEsta)
            {
                inRangeEnemies.Add(collision.gameObject); // agregar el enemigo, no la telaraña
            }
        }
    }

    private void Update()
    {
        if (Time.time > appearance + 0.05f)
        {
            foreach (var item in inRangeEnemies)
            {
                var enemy = item.GetComponent<enemyScript>();
                if (!enemy.slowed)
                {
                    Debug.Log("lento");
                    enemy.speed /= amountOfSlowness;
                    enemy.slowed = true;
                }
            }
        }
    }
}
