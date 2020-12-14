using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Mine : NetworkBehaviour
{
    public GameObject Father;

    [SerializeField]
    private explosionScript explosion;

    [SerializeField]
    private Animator animtor;

    [SerializeField]
    private NetworkMatchChecker oneChecker;
    [SerializeField]
    private NetworkMatchChecker twoChecker;

    public void SetValues(System.Guid key, GameObject father)
    {
        Father = father;
        explosion.Father = father;
        oneChecker.matchId = key;
        twoChecker.matchId = key;
    }

    // public GameObject mainPref;
    // public GameObject explosion;

    public float Damage = 20;

    public Vector3 endPosition;
    public Vector3 startParabola;

    public float mineJumpTime = 0.5f;
    private float animationTime;

    private void Start()
    {
        explosion.Father = Father;
        mineJumpTime = 1f;
        startParabola = this.transform.position;
        StartCoroutine("JumpEndTime" , mineJumpTime);
    }

    private void Update()
    {
        animationTime += Time.deltaTime;
        animationTime = animationTime % mineJumpTime;

        transform.position = MathParabola.Parabola(startParabola, endPosition, 2f, animationTime);

        if (Vector3.Distance(transform.position, endPosition) <= 0.4f)
        {
            this.enabled = false;
        }

    }
    IEnumerator JumpEndTime(float time)
    {
        yield return new WaitForSeconds(time);
        transform.position = endPosition;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (this.isServer)
        {
            if (other.gameObject.tag == "EnemyPlayer" && other.gameObject != Father)
            {
                this.enabled = false;
                animtor.enabled = true;
                animtor.SetTrigger("jump");
                StopCoroutine("JumpEndTime");
                StartCoroutine(JumpTime());
            }
        }
       
    }

    IEnumerator JumpTime()
    {
        yield return new WaitForSeconds(0.12f);
        explosion.animator.SetTrigger("Explosion");
        yield return new WaitForSeconds(0.22f);
        Destroy(this.gameObject);
    }

    
}