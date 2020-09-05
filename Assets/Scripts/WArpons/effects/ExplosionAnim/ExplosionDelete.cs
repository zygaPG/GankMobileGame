using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ExplosionDelete : NetworkBehaviour
{
    float time;
    
    void Update()
    {
        
        time += Time.deltaTime;
        
        if (time > 0.35f)
        {
            Destroy(this.gameObject);
        }
    }
}
