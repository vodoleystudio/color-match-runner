using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    public static bool RnadomBolean()
    {
        var rnd = Random.Range(0, 2);
        if (rnd == 0)
        {
            return true;
        }
        if (rnd == 1)
        {
            return false;
        }
        else
        {
            throw new System.Exception();
        }
    }
}
