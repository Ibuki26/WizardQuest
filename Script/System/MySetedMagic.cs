using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySetedMagic : SingletonMonoBehaviour<MySetedMagic>
{
    [SerializeField] GameObject[] magics = new GameObject[2];
    public static GameObject[] myMagic = { null, null};
    public static Wand wand;

    protected override void Awake()
    {
        base.Awake();
        if(myMagic[0] == null )
            myMagic[0] = magics[0];
        if (myMagic[1] == null)
            myMagic[1] = magics[1];
        if (wand == null)
            wand = new HitPointWand(30);
    }

    public void SetMagic(GameObject m, int i)
    {
        myMagic[i] = m;
    }

    public void SetWand(Wand w)
    {
        wand = w;
    }

    public GameObject GetMagic(int i)
    {
        return myMagic[i];
    }

    public Wand GetWand()
    {
        return wand;
    }
}
