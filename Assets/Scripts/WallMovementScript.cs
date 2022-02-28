using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovementScript : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    public Transform target;
    public float t;
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 a = transform.position;
        Vector3 b = target.position;
        transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            canvas.GetComponent<PauseMenuScript>().Lose();
        }
        if (collision.gameObject.tag == "Door")
        {
            canvas.GetComponent<PauseMenuScript>().Lose();
        }
        if (collision.gameObject.tag == "Player")
        {
            canvas.GetComponent<PauseMenuScript>().Lose();
        }
    }
}