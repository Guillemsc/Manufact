using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(BasicLevel))]
public class BasicLevelExt : Editor
{
    public override void OnInspectorGUI()
    {
        BasicLevel myScript = (BasicLevel)target;

        if (!Application.isPlaying)
        {
            DrawDefaultInspector();

            List<EntityBullet.EntityBulletType> bullets = myScript.GetBulletsList();
            int bullets_number = bullets.Count;

            GUILayout.Label("Bullets elements ========");

            bullets_number = EditorGUILayout.IntField("Bullets number", bullets_number);

            if(bullets_number != bullets.Count)
            {
                myScript.SetBulletsNumber(bullets_number);

                EditorUtility.SetDirty(target);
            }

            bullets = myScript.GetBulletsList();

            for(int i = 0; i < bullets.Count; ++i)
            {
                EntityBullet.EntityBulletType curr_bullet = bullets[i];

                string name = "Bullet " + (i + 1);

                EntityBullet.EntityBulletType obj = (EntityBullet.EntityBulletType)EditorGUILayout.EnumPopup(name, curr_bullet);

                if(obj != curr_bullet)
                {
                    myScript.SetBulletType(i, obj);

                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
}

#endif
