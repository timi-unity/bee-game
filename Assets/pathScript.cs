using UnityEngine;
using System.Collections.Generic;

public class pathScript : MonoBehaviour
{
    public List<Transform> eslabones;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("placing"))
        {
            collision.gameObject.GetComponent<placingScript>().cantPlace();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("placing"))
        {
            collision.gameObject.GetComponent<placingScript>().canPlace();
        }
    }
}
