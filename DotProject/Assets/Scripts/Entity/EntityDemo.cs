using Dot.Asset;
using Dot.Entity;
using Dot.Entity.Controller;
using Dot.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDemo : MonoBehaviour
{
    private void Awake()
    {
        StartupProxy.Startup();
        AssetUtil.InitDatabaseLoader((result) => { Debug.Log("AssetManage::init=" + result); });
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Create Entity"))
        {
            EntityObject entity = new EntityObject(1);

            EntityGameObjectController goController = new EntityGameObjectController(new GameObject("Entity)"));
            entity.AddController(goController);

            EntityAvatarController avatarController = new EntityAvatarController();
            entity.AddController(avatarController);

            avatarController.LoadSkeleton("ch_pc_hou_006_skeleton");
            //avatarController.LoadPart

        }
    }
}
