using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructBarraks : StructBase
{
    public override void  DestroyStructure()
    {
        parent.DestroyStructure(cost);
        Destroy(gameObject);
    }
}
