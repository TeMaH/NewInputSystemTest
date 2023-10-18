using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] float range = 2.5f;
    public float force = 150.0f;
    public float explosionRange = 5.0f;

    float t;
    bool isActive;
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() 
    {
        if (isActive) t += Time.deltaTime;
        if (t >= time)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, 512);
            foreach (var collider in colliders)
            {
                collider.GetComponent<Rigidbody>().AddExplosionForce(150.0f, transform.position, range);
            }
            Destroy(this);
        }
    }

    void FixedUpdate() { }

    private void OnCollisionStay(Collision collision)
    {
        isActive = true;
    }
}
