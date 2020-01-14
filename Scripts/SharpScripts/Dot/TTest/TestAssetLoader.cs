using Dot.Asset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAssetLoader : MonoBehaviour
{
    void Start()
    {
        AssetManager.GetInstance().InstanceAssetAsync(new string[] { "Capsule","Cube","Plane" }, (address, uObj, userData) =>
        {
            ((GameObject)uObj).name = address + " instance";
        },null);
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Chang scene"))
        {
            AssetManager.GetInstance().LoadSceneAsync("test1", (address,scene,userdata)=>
            {
                AssetManager.GetInstance().UnloadUnusedAsset();
            }, null);
        }
    }
}
