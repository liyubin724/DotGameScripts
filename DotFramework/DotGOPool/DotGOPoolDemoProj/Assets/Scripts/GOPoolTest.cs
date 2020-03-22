using Dot.Core.Proxy;
using Dot.Log;
using Dot.Pool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GOPoolTest : MonoBehaviour
{
    public TextAsset logConfig = null;
    public GameObject cubePrefab = null;
    void Start()
    {
        string configText = logConfig.text;
        configText = configText.Replace("#OUTPUT_DIR#", "./");
        LogUtil.Initalize(logConfig.text);
    }

    void Update()
    {
        UpdateProxy.GetInstance().DoUpdate(Time.deltaTime);
        UpdateProxy.GetInstance().DoUnscaleUpdate(Time.unscaledDeltaTime);
    }

    private void LateUpdate()
    {
        UpdateProxy.GetInstance().DoLateUpdate();
    }

    private List<GameObject> instanceList = new List<GameObject>();
    Vector3 pos = Vector3.zero;
    private void OnGUI()
    {
        if(GUILayout.Button("Create Pool"))
        {
            GameObjectPoolGroup poolGroup = GameObjectPoolManager.GetInstance().CreateGroup("Group");
            GameObjectPool pool = poolGroup.CreatePool("cube", cubePrefab, PoolTemplateType.Prefab);
        }
        if(GUILayout.Button("Get Item"))
        {
            GameObjectPoolGroup poolGroup = GameObjectPoolManager.GetInstance().CreateGroup("Group");
            GameObjectPool pool = poolGroup.GetPool("cube");
            GameObject instance = pool.GetPoolItem();
            instanceList.Add(instance);
            instance.transform.parent = null;

            SceneManager.MoveGameObjectToScene(instance, SceneManager.GetActiveScene());
            
            instance.transform.position = pos + new Vector3(1.1f, 0, 0);
            pos = instance.transform.position;
        }
        if(GUILayout.Button("Range Delete Item"))
        {
            if(instanceList.Count==0)
            {
                return;
            }
            GameObjectPoolGroup poolGroup = GameObjectPoolManager.GetInstance().CreateGroup("Group");
            GameObjectPool pool = poolGroup.GetPool("cube");

            GameObject instance = instanceList[instanceList.Count-1];
            instanceList.RemoveAt(instanceList.Count-1);
            pool.ReleasePoolItem(instance);
            if (instanceList.Count == 0)
            {
                pos = Vector3.zero;
            }else
            {
                pos = instanceList[instanceList.Count - 1].transform.position;
            }
        }
        if (GUILayout.Button("Change Scene"))
        {
            SceneManager.LoadScene("test2");
        }
    }
}
