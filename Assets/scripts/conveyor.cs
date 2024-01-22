using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class conveyor : MonoBehaviour
{

    public Vector3 speed;
    public float timebeforereversal;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(speedreverse());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + speed * Time.deltaTime;
    }


    public IEnumerator speedreverse()
    {
        yield return new WaitForSeconds(timebeforereversal);
        speed = -speed;
        StartCoroutine(speedreverse());
    }

}
