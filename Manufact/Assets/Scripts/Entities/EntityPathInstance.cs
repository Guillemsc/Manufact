using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EntityPathInstance : MonoBehaviour
{
    [SerializeField] private float points_size = 1.0f;

    [SerializeField] private bool draw_path = true;

    [SerializeField] [HideInInspector] List<PathPoint> path_points = new List<PathPoint>();

    [SerializeField] [HideInInspector] List<PathPointConexions> path_point_conexions = new List<PathPointConexions>();

    [System.Serializable]
    public class PathPoint
    {
        public Vector2    pos = Vector2.zero;
        public Vector2    world_pos = Vector2.zero;
        public PathEntity entity = null;

        public Vector2 RealPos()
        {
            return world_pos + pos;
        }
    }

    [System.Serializable]
    public class PathPointConexions
    {
        public int p1 = 0;
        public int p2 = 0;
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

    public List<PathPoint> GetPathPoints()
    {
        return path_points;
    }

    public List<PathPointConexions> GetPathPointsConections()
    {
        return path_point_conexions;
    }

    public PathPoint AddPathPoint(Vector2 pos)
    {
        PathPoint ret = new PathPoint();

        ret.pos = pos;

        path_points.Add(ret);

        return ret;
    }

    public void RemovePathPoint(PathPoint pp)
    {
        if (pp != null)
        {
            for (int i = 0; i < path_points.Count; ++i)
            {
                PathPoint curr_point = path_points[i];

                if (curr_point == pp)
                {
                    RemovePathPointFromConexions(pp);
                    path_points.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public int GetPathPointIndex(PathPoint pp)
    {
        int ret = -1;

        for(int i = 0; i < path_points.Count; ++i)
        {
            if(pp == path_points[i])
            {
                ret = i;
                break;
            }
        }

        return ret;
    }

    public void AddPathPointConexion(PathPoint p1, PathPoint p2)
    {
        if(p1 != null && p2 != null)
        {
            if (!PathPointsHaveConexion(p1, p2))
            {
                PathPointConexions con = new PathPointConexions();
                con.p1 = GetPathPointIndex(p1);
                con.p2 = GetPathPointIndex(p2);

                if(con.p1 != -1 && con.p2 != -1)
                    path_point_conexions.Add(con);
            }
        }
    }

    public void RemovePathPointFromConexions(PathPoint pp)
    {
        int index = GetPathPointIndex(pp);

        for (int i = 0; i < path_point_conexions.Count; ++i)
        {
            PathPointConexions curr_con = path_point_conexions[i];

            if (curr_con.p1 == index || curr_con.p2 == index)
            {
                path_point_conexions.RemoveAt(i);
            }
            else
                ++i;
        }
    }

    public bool PathPointsHaveConexion(PathPoint p1, PathPoint p2)
    {
        bool ret = false;

        int index1 = GetPathPointIndex(p1);
        int index2 = GetPathPointIndex(p2);

        if (index1 != -1 && index2 != -1)
        {
            for (int i = 0; i < path_point_conexions.Count; ++i)
            {
                PathPointConexions curr_con = path_point_conexions[i];

                bool found = false;

                if (curr_con.p1 == index1 && curr_con.p2 == index2)
                    found = true;

                if (curr_con.p1 == index2 && curr_con.p2 == index1)
                    found = true;

                if (found)
                {
                    ret = true;
                    break;
                }
            }
        }

        return ret;
    }

    public List<int> GetPathPointConexions(PathPoint p1)
    {
        List<int> ret = new List<int>();

        if (p1 != null)
        {
            int index = GetPathPointIndex(p1);

            for (int i = 0; i < path_point_conexions.Count; ++i)
            {
                PathPointConexions curr_con = path_point_conexions[i];

                if (curr_con.p1 == index)
                    ret.Add(curr_con.p2);

                else if (curr_con.p2 == index)
                    ret.Add(curr_con.p1);
            }
        }

        return ret;
    }

    private void UpdatePath()
    {
        for (int i = 0; i < path_points.Count; ++i)
        {
            PathPoint curr_point = path_points[i];

            curr_point.world_pos = transform.position;
        }
    }

    private void DebugDrawPath()
    {
        for (int i = 0; i < path_points.Count; ++i)
        {
            PathPoint curr_point = path_points[i];

            Vector3 p1 = new Vector3(curr_point.RealPos().x - (points_size * 0.5f), curr_point.RealPos().y + (points_size * 0.5f), transform.position.z);
            Vector3 p2 = new Vector3(curr_point.RealPos().x - (points_size * 0.5f), curr_point.RealPos().y - (points_size * 0.5f), transform.position.z);
            Vector3 p3 = new Vector3(curr_point.RealPos().x + (points_size * 0.5f), curr_point.RealPos().y - (points_size * 0.5f), transform.position.z);
            Vector3 p4 = new Vector3(curr_point.RealPos().x + (points_size * 0.5f), curr_point.RealPos().y + (points_size * 0.5f), transform.position.z);

            Debug.DrawLine(p1, p2);
            Debug.DrawLine(p2, p3);
            Debug.DrawLine(p3, p4);
            Debug.DrawLine(p4, p1);

            for(int y = 0; y < path_point_conexions.Count; ++y)
            {
                PathPointConexions curr_con = path_point_conexions[y];

                Vector3 c1 = new Vector3(path_points[curr_con.p1].RealPos().x, path_points[curr_con.p1].RealPos().y, transform.position.z);
                Vector3 c2 = new Vector3(path_points[curr_con.p2].RealPos().x, path_points[curr_con.p2].RealPos().y, transform.position.z);

                Debug.DrawLine(c1, c2);
            }
        }
    }
}
