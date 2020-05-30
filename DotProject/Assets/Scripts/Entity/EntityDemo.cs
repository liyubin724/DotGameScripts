using Dot.Entity;
using Dot.Entity.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDemo : MonoBehaviour
{
    private void OnGUI()
    {
        if(GUILayout.Button("Create Entity"))
        {
            EntityObject entity = new EntityObject(1);

            EntityGameObjectController goController = new EntityGameObjectController(new GameObject("Entity)"));
            entity.AddController(goController);

            EntityAvatarController avatarController = new EntityAvatarController();
            entity.AddController(avatarController);


        }
    }
}
