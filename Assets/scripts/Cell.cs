using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector2 pos;
    public bool walkable;

    public Cell(Vector2 pos, bool walkable)
    {
        this.pos = pos;
        this.walkable = walkable;
    }

    public Vector2 GetPos() => pos;
}
