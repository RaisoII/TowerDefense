using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraSoldier : MonoBehaviour
{

    private GameObject target;
    private void LateUpdate()
    {
        if(target != null)
        {
            transform.position = target.transform.position;
        }
        else
            Destroy(gameObject);
    }

    public void setTarget(GameObject gameObjectTarget) => target = gameObjectTarget;
}
