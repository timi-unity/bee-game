using UnityEngine;

public class killBees : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "circle_0")
        {
            if (collision.gameObject.transform.parent.gameObject.TryGetComponent<beetlepoopScript>(out var n))
            {
                GameObject.Find("gameManager").GetComponent<gameManager>().poopPlaced = false;
            }
            GameObject.Find("gameManager").GetComponent<gameManager>().cantidadTropas--;
            Destroy(collision.gameObject.transform.parent.gameObject);
        }
    }
}
