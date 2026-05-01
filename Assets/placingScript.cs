using UnityEngine;
using System.Linq;

public class placingScript : MonoBehaviour
{
    public GameObject toPlaceTroop;
    private bool placeable = true;
    public bool placingPoop = false;

    private void Start()
    {
        GameObject[] objetosConNombre = GameObject.FindObjectsOfType<Transform>()
            .Where(t => t.name == "placeTropa(Clone)")
            .Select(t => t.gameObject)
            .ToArray();

        foreach (var item in objetosConNombre)
        {
            if(item != gameObject)
            {
                Destroy(item);
            }
        }
    }

    void Update()
    {
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            placeTroop();
        }
    }

    public void cantPlace()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        placeable = false;
    }

    public void canPlace()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        placeable = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("troop"))
        {
            cantPlace();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("troop"))
        {
            canPlace();
        }
    }

    private void placeTroop()
    {
        if (placeable)
        {
            Instantiate(toPlaceTroop, transform.position, Quaternion.identity);
            GameObject.Find("gameManager").GetComponent<gameManager>().GastarPlata(toPlaceTroop.GetComponentInChildren<troopClick>().totalCost);
            if(!GameObject.Find("gameManager").GetComponent<gameManager>().poopPlaced)
            GameObject.Find("gameManager").GetComponent<gameManager>().poopPlaced = placingPoop;
            GameObject.Find("gameManager").GetComponent<gameManager>().cantidadTropas++;
            Destroy(gameObject);
        }       
    }
}
