using UnityEngine;
using System.Collections;

public class Storm : MonoBehaviour {

    float rotationSpeed;

    public void Start()
    {
        rotationSpeed = Random.Range(-180, 180);
        float sizeMutation = Random.Range(-.2f, .2f);
        transform.localScale *= sizeMutation;
    }

    public void Update()
    {
        transform.Rotate(new Vector3(0,0,1),Time.deltaTime*rotationSpeed);
    }
}
