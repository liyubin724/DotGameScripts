using Dot.Asset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAssetLoader : MonoBehaviour
{
    void Start()
    {
        AssetManager.GetInstance().LoadAssetAsync(new string[] { "Capsule","Cube","Plane" }, (address, uObj, userData) =>
        {
            Object.Instantiate(uObj);
        },null);
    }
}
