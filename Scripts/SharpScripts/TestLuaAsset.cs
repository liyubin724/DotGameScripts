using Dot.Lua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class TT
{

}

public class TestLuaAsset : MonoBehaviour
{
    public LuaAsset luaAsset;
    // Start is called before the first frame update
    List<int> list = new List<int>();
    void Start()
    {
        //AssetBundle ab = AssetBundle.LoadFromFile("E:/output/aa");
        //SceneManager.LoadScene("testscene");
        //ab.Unload(false);

        Type tt = typeof(Dictionary<,>);

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach(var a in assemblies)
        {
            Type[] types = a.GetTypes();
            foreach(var t in types)
            {
                if(t.FullName == typeof(List<>).FullName)
                {
                    Debug.LogError(t.FullName);
                }
            }
        }

        Debug.Log(typeof(int).FullName);

        Type newType = tt.MakeGenericType(typeof(int), typeof(string));
        Debug.Log(newType.FullName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
