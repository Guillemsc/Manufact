using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : GameEntity
{
    private List<GameObject> visual_bullets = new List<GameObject>();

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKey("q"))
        {
            Shoot();
        }
    }

    protected override void OnEventCall(EventManager.Event ev)
    {
        switch(ev.Type())
        {
            case EventManager.EventType.ENTITY_BULLETS_CHANGE:
                SetVisualBullets(ev.entity_bullets_change.bullets_now);
                break;
        }
    }

    private void SetVisualBullets(int set)
    {
        if (set != visual_bullets.Count)
        {
            for (int i = 0; i < visual_bullets.Count; ++i)
            {
                Destroy(visual_bullets[i]);
            }

            visual_bullets.Clear();

            for(int i = 0; i < set; ++i)
            {
                GameObject new_bull = Instantiate(LevelCreatorEditor.Instance.GetBaseBulletAmmo(), new Vector3(0, 0, 0), Quaternion.identity);

                float start_pos = 0.46f;

                float to_add = 0.15f;

                if (i == 0)
                    to_add = 0.16f;

                new_bull.transform.parent = this.transform;
                new_bull.transform.localPosition = new Vector3((start_pos + (i * to_add)),
                    -0.07f, 0);

                visual_bullets.Add(new_bull);
            }
        }
    }
}
