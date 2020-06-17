using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Maps
{ 
    public static string[] GetRandomMap()
    {
        return map6;
        int rnd = Random.Range(1, 5);
        switch(rnd)
        {
            case 1: return map1;
            case 2: return map2;
            case 3: return map3;
            case 4: return map4;
            case 5: return map5;
            default: return map1;
        }
    }

    static string[] map1 =
    {
        "xxxxxxxxxx",
        "xxooooxoox",
        "sooxxooxoe",
        "xoxxooxxox",
        "xxxxooooox"
    };

    static string[] map2 =
    {
        "eoooooooox",
        "xxxxxxxxox",
        "xxxxxxxxox",
        "xxxxxxxxox",
        "soooooooox",
    };

    static string[] map3 =
    {
        "sxoxxxoe",
        "ooxxxxox",
        "xooxxxox",
        "xxooooox",
    };

    static string[] map4 =
    {
        "sooxoxoe",
        "xooxooox",
        "ooxooxox",
        "xoooxoox",
    };

    static string[] map5 =
    {
        "xxoooooxxxxx",
        "xxoxxxoxxxxx",
        "xxoxxxoxxxoe",
        "sooxxxooxxox",
        "xxxxxxoxxxox",
        "xxxxxxooooox",
    };

    static string[] map6 =
{
        "sooxoxox",
        "xxoooxoe",
        "oooxooox",
        "xxooxxox",
    };

    static string[] debug1 =
{
        "xox",
        "soe",
        "xox",
    };

}
