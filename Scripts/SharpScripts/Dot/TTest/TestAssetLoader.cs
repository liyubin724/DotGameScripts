using Dot.Asset;
using Dot.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAssetLoader : MonoBehaviour
{
    private string SpawnName = "PrefabPool";
    void Start()
    {
        AssetManager.GetInstance().LoadAssetAsync("Cube", (address, uObj, userData) =>
        {
            AssetManager.GetInstance().InstantiateAsset("Cube",uObj);
        }, null);
        AssetManager.GetInstance().LoadAssetAsync("Cube", (address, uObj, userData) =>
        {
            AssetManager.GetInstance().InstantiateAsset("Cube", uObj);
        }, null);
        AssetManager.GetInstance().LoadAssetAsync("Cube", (address, uObj, userData) =>
        {
            AssetManager.GetInstance().InstantiateAsset("Cube", uObj);
        }, null);
        //SpawnPool spawn = PoolManager.GetInstance().GetSpawnPool(SpawnName, true);

        //AssetManager.GetInstance().LoadAssetAsync(new string[] { "Capsule", "Cube", "Plane" }, (address, uObj, userData) =>
        //  {
        //      spawn.CreateGameObjectPool(address, (GameObject)uObj);
        //  }, null);
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
        if(GUILayout.Button("Instance from pool"))
        {
            SpawnPool spawn = PoolManager.GetInstance().GetSpawnPool(SpawnName, true);
            GameObjectPool objectPool = spawn.GetGameObjectPool("Cube");
            GameObject cubeGO = objectPool.GetPoolItem();
            cubeGO.name = "Cube From Pool";
        }
        if(GUILayout.Button("Unload Unused"))
        {
            AssetManager.GetInstance().UnloadUnusedAsset();
        }
    }
}
