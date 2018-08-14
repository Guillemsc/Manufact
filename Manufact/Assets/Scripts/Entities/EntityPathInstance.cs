using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EntityPathInstance : MonoBehaviour
{
    [SerializeField] private float points_size = 1.0f;

    [SerializeField] private bool draw_path = true;

    [SerializeField] [HideInInspector] List<PathPoint> path_points = new List<PathPoint>();

    [System.Serializable]
    public class PathPoint
    {
        public Vector2    pos = Vector2.zero;
        public PathEntity entity = null;
        public GameObject go = null;
       
        public List<PathPoint> connexions = new List<PathPoint>();

        public bool HasConnexion(PathPoint pp)
        {
            bool ret = false;

            if (pp != null)
            {
                for (int i = 0; i < connexions.Count; ++i)
                {
                    PathPoint curr_point = connexions[i];

                    if(curr_point == pp || (curr_point.go == pp.go && curr_point.go != null))
                    {
                        ret = true;
                        break;
                    }
                }
            }

            return ret;
        }
    }

    [System.Serializable]
    public class PathEntity
    {
        GameObject go = null;
    }

	void Start ()
    {
		
	}

	private void Update ()
    {
        UpdatePath();

        if (draw_path)
            DebugDrawPath();
	}

    public PathPoint AddPathPoint(Vector2 pos)
    {
        PathPoint ret = new PathPoint();

        ret.pos = pos;
        ret.go = new GameObject();

        ret.go.transform.parent = transform;

        path_points.Add(ret);

        return ret;
    }

    public void AddPathPointConexion(PathPoint p1, PathPoint p2)
    {
        if(p1 != null && p2 != null)
        {
            if (!p1.HasConnexion(p2))
                p1.connexions.Add(p2);

            if (!p2.HasConnexion(p1))
                p2.connexions.Add(p1);
        }
    }

    public PathPoint GetPathPointFromGo(GameObject go)
    {
        PathPoint ret = null;

        if (go != null)
        {
            for(int i = 0; i < path_points.Count; ++i)
            {
                PathPoint curr_point = path_points[i];

                if(curr_point.go == go)
                {
                    ret = curr_point;
                    break;
                }
            }
        }

        return ret;
    }

    private void UpdatePath()
    {

    }

    private void DebugDrawPath()
    {
        for (int i = 0; i < path_points.Count; ++i)
        {
            PathPoint curr_point = path_points[i];

            Vector3 p1 = new Vector3(curr_point.pos.x - (points_size * 0.5f), curr_point.pos.y + (points_size * 0.5f), transform.position.z);
            Vector3 p2 = new Vector3(curr_point.pos.x - (points_size * 0.5f), curr_point.pos.y - (points_size * 0.5f), transform.position.z);
            Vector3 p3 = new Vector3(curr_point.pos.x + (points_size * 0.5f), curr_point.pos.y - (points_size * 0.5f), transform.position.z);
            Vector3 p4 = new Vector3(curr_point.pos.x + (points_size * 0.5f), curr_point.pos.y + (points_size * 0.5f), transform.position.z);

            Debug.DrawLine(p1, p2);
            Debug.DrawLine(p2, p3);
            Debug.DrawLine(p3, p4);
            Debug.DrawLine(p4, p1);

            for(int y = 0; y < curr_point.connexions.Count; ++y)
            {
                PathPoint curr_con = curr_point.connexions[i];

                Vector3 c1 = new Vector3(curr_point.pos.x, curr_point.pos.y, transform.position.z);
                Vector3 c2 = new Vector3(curr_con.pos.x, curr_con.pos.y, transform.position.z);
            }
        }
    }
}
