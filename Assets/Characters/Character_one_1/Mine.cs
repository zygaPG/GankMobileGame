using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Mine : NetworkBehaviour
{
    public GameObject Father;
    public GameObject mainPref;
    public GameObject explosion;

    public float Damage = 20;

    public Vector3 endPosition;
    public Vector3 startParabola;

    public float mineJumpTime = 0.05f;
    private float animationTime;

    private void Start()
    {
        startParabola = this.transform.position;
    }

    private void Update()
    {
        animationTime += Time.deltaTime;
        animationTime = animationTime % mineJumpTime;

        transform.position = MathParabola.Parabola(startParabola, endPosition, 3f, animationTime);

        if(Vector3.Distance(transform.position, endPosition) <= 0.3f)
        {
            this.enabled = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.isServer)
        {
            if (other.gameObject.tag == "Player" && other.gameObject != Father)
            {
                GameObject explo = Instantiate(explosion, this.transform.position, this.transform.rotation);
                NetworkServer.Spawn(explo);
                this.transform.LookAt(other.transform.position);
                other.GetComponent<Hit>().GetHit(10, 0, 1, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
        Debug.Log(other.gameObject.name);
    }
}
