using Dot.Lua;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class TT
{

}

public class TestLuaAsset : MonoBehaviour
{
    public LuaAsset luaAsset;
    // Start is called before the first frame update
    void Start()
    {
        //AssetBundle ab = AssetBundle.LoadFromFile("E:/output/aa");
        //SceneManager.LoadScene("testscene");
        //ab.Unload(false);
        Type t = typeof(TT);
        Debug.Log(t.IsNotPublic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
