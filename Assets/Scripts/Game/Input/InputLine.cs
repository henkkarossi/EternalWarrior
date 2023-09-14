using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLine 
{
    public Vector3 direction;
    public bool doubleClick;
    public InputLine(Vector3 direction, bool doubleClick)
    {
        this.direction = direction;
        this.doubleClick = doubleClick;
    }
    public InputLine(Vector3 direction)
    {
        this.direction = direction;
        this.doubleClick = false;
    }
    public InputLine(bool doubleClick)
    {
        this.direction = Vector3.zero;
        this.doubleClick = doubleClick;
    }
    public InputLine()
    {
        this.direction = Vector3.zero;
        this.doubleClick = false;
    }
}
