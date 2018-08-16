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

    public enum PathPointDirection
    {
        PATH_POINT_DIRECTION_UP,
        PATH_POINT_DIRECTION_DOWN,
        PATH_POINT_DIRECTION_LEFT,
        PATH_POINT_DIRECTION_RIGHT,
    }

    [System.Serializable]
    public class PathPoint
    {
        public Vector2    pos = Vector2.zero;
        public Vector2    world_pos = Vector2.zero;
        public PathPointDirection direction;
        public PathEntity entity = null;

        public string uid = "no_uid";

        public Vector2 RealPos()
        {
            return world_pos + pos;
        }
    }

    [System.Serializable]
    public class PathPointConexions
    {
        public string p1 = "no_uid";
        public string p2 = "no_uid";

        public LineRenderer line = null;
    }

    public enum PathEntityType
    {
        PATH_ENTITY_TYPE_EMPTY,
        PATH_ENTITY_TYPE_PLAYER,
        PATH_ENTITY_TYPE_BASE_ENEMY,
    }

    [System.Serializable]
    public class PathEntity
    {
        public PathEntityType type = PathEntityType.PATH_ENTITY_TYPE_EMPTY;
        public GameObject     go = null;
    }

	void Start ()
    {
        if (Application.isPlaying)
        {
            InitPath();
        }
    }

	private void Update ()
    {
        UpdatePath();

        if (draw_path)
            DebugDrawPath();
    }

    public List<GameEntity> GetGameEntitiesByEntityType(PathEntityType type)
    {
        List<GameEntity> ret = new List<GameEntity>();

        for(int i = 0; i < path_points.Count; ++i)
        {
            PathPoint curr_tile = path_points[i];

            if (curr_tile.entity.type == type)
            {
                if(curr_tile.entity.go != null)
                {
                    GameEntity ge = curr_tile.entity.go.GetComponent<GameEntity>();

                    if(ge != null)
                        ret.Add(ge);
                }
            }
        }

        return ret;
    }

    public GameEntity GetGameEntityByEntityType(PathEntityType type)
    {
        GameEntity ret = null;

        for (int i = 0; i < path_points.Count; ++i)
        {
            PathPoint curr_tile = path_points[i];

            if (curr_tile.entity.type == type)
            {
                if (curr_tile.entity.go != null)
                {
                    GameEntity ge = curr_tile.entity.go.GetComponent<GameEntity>();

                    if (ge != null)
                        ret = ge;
                }
            }
        }

        return ret;
    }

    public List<PathPoint> GetPathPoints()
    {
        return path_points;
    }

    public List<PathPointConexions> GetPathPointsConections()
    {
        return path_point_conexions;
    }

    public void InitPath()
    {
        for (int i = 0; i < path_points.Count; ++i)
        {
            PathPoint curr_tile = path_points[i];

            GameObject inst = InstantiateEntityGoFromPathEntityType(curr_tile.entity.type);

            if (inst != null)
            {
                inst.name = "Entity: " + i;
                curr_tile.entity.go = inst;
                curr_tile.entity.go.transform.position = curr_tile.RealPos();
                curr_tile.entity.go.transform.parent = this.transform;
            }
        }

        for(int i = 0; i < path_point_conexions.Count; ++i)
        {
            PathPointConexions curr_con = path_point_conexions[i];

            GameObject go_con = new GameObject();
            go_con.name = "Connexion: " + i;
            go_con.transform.parent = this.transform;
            curr_con.line = go_con.AddComponent<LineRenderer>();

            PathPoint p1 = GetPathPointFromUid(curr_con.p1);
            PathPoint p2 = GetPathPointFromUid(curr_con.p2);

            Vector3 start_pos = new Vector3(p1.RealPos().x, p1.RealPos().y, 1);
            Vector3 end_pos = new Vector3(p2.RealPos().x, p2.RealPos().y, 1);

            curr_con.line.SetPosition(0, start_pos);
            curr_con.line.SetPosition(1, end_pos);

            curr_con.line.material = LevelCreatorEditor.Instance.GetLineMaterial();
            curr_con.line.numCapVertices = 60;
            curr_con.line.startWidth = 0.1f;
            curr_con.line.endWidth = 0.1f;
        }
    }

    GameObject InstantiateEntityGoFromPathEntityType(PathEntityType type)
    {
        GameObject ret = null;

        switch (type)
        {
            case PathEntityType.PATH_ENTITY_TYPE_PLAYER:
                ret = Instantiate(LevelCreatorEditor.Instance.GetPlayerPrefab(), Vector3.zero, Quaternion.identity);
                EntityPlayer e_p = ret.AddComponent<EntityPlayer>();
                e_p.Init(this);
                break;
            case PathEntityType.PATH_ENTITY_TYPE_BASE_ENEMY:
                ret = Instantiate(LevelCreatorEditor.Instance.GetBaseEnemyPrefab(), Vector3.zero, Quaternion.identity);
                EntityBaseEnemy e_b = ret.AddComponent<EntityBaseEnemy>();
                e_b.Init(this);
                break;
        }

        return ret;
    }

    Color PathEntityDebugColorFromPathEntityType(PathEntityType type)
    {
        Color ret = new Color();

        switch (type)
        {
            case PathEntityType.PATH_ENTITY_TYPE_EMPTY:
                ret = new Color(1, 1, 1, 1);
                break;
            case PathEntityType.PATH_ENTITY_TYPE_PLAYER:
                if(LevelCreatorEditor.Instance != null)
                    ret = LevelCreatorEditor.Instance.GetPlayerDebugColor();
                break;
            case PathEntityType.PATH_ENTITY_TYPE_BASE_ENEMY:
                if(LevelCreatorEditor.Instance != null)
                    ret = LevelCreatorEditor.Instance.GetBaseEnemyDebugColor();
                break;
        }

        return ret;
    }

    public PathPoint AddPathPoint(Vector2 pos)
    {
        PathPoint ret = new PathPoint();

        ret.pos = pos;
        ret.uid = System.Guid.NewGuid().ToString();
        ret.entity = new PathEntity();

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

    public PathPoint GetPathPointFromUid(string uid)
    {
        PathPoint ret = null;

        for (int i = 0; i < path_points.Count; ++i)
        {
            PathPoint curr_point = path_points[i];

            if(curr_point.uid == uid)
            {
                ret = curr_point;
                break;
            }
        }

        return ret;
    }

    public PathPoint GetPathPointFromIndex(int index)
    {
        PathPoint ret = null;

        for (int i = 0; i < path_points.Count; ++i)
        {
            PathPoint curr_point = path_points[i];

            if (i == index)
            {
                ret = curr_point;
                break;
            }
        }

        return ret;
    }

    public PathPoint GetPathPointFromEntityGo(GameObject go)
    {
        PathPoint ret = null;

        for (int i = 0; i < path_points.Count; ++i)
        {
            PathPoint curr_point = path_points[i];

            if (curr_point.entity.go == go)
            {
                ret = curr_point;
                break;
            }
        }

        return ret;
    }

    public int GetPathPointIndexFromUid(string uid)
    {
        int ret = -1;

        for (int i = 0; i < path_points.Count; ++i)
        {
            PathPoint curr_point = path_points[i];

            if (curr_point.uid == uid)
            {
                ret = i;
                break;
            }
        }

        return ret;
    }

    public void AddPathPointConexion(PathPoint p1, PathPoint p2)
    {
        if(p1 != null && p2 != null && p1.uid != p2.uid)
        {
            if (!PathPointsHaveConexion(p1, p2))
            {
                PathPointConexions con = new PathPointConexions();
                con.p1 = p1.uid;
                con.p2 = p2.uid;

                if(con.p1 != "no_uid" && con.p2 != "no_uid")
                    path_point_conexions.Add(con);
            }
        }
    }

    public void AddPathPointConexion(List<PathPoint> pp_l)
    {
        if(pp_l != null)
        {
            List<PathPoint> pp_left = new List<PathPoint>();
            pp_left.AddRange(pp_l);

            while(pp_left.Count > 0)
            {
                PathPoint to_connect = pp_left[0];

                for (int i = 0; i < pp_l.Count; ++i)
                {
                    AddPathPointConexion(to_connect, pp_l[i]);
                }

                pp_left.RemoveAt(0);
            }
        }
    }

    public void RemovePathPointFromConexions(PathPoint pp)
    {
        for (int i = 0; i < path_point_conexions.Count;)
        {
            PathPointConexions curr_con = path_point_conexions[i];

            if (curr_con.p1 == pp.uid || curr_con.p2 == pp.uid)
            {
                path_point_conexions.RemoveAt(i);
            }
            else
                ++i;
        }
    }

    public void RemovePathPointConexion(PathPoint pp, PathPoint pp2)
    {
        if (pp != null && pp2 != null)
        {
            for (int i = 0; i < path_point_conexions.Count; ++i)
            {
                PathPointConexions curr_con = path_point_conexions[i];

                if ((curr_con.p1 == pp.uid && curr_con.p2 == pp2.uid) ||
                    (curr_con.p1 == pp2.uid && curr_con.p2 == pp.uid))
                {
                    path_point_conexions.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public bool PathPointsHaveConexion(PathPoint p1, PathPoint p2)
    {
        bool ret = false;

        if (p1 != null && p2 != null)
        {
            for (int i = 0; i < path_point_conexions.Count; ++i)
            {
                PathPointConexions curr_con = path_point_conexions[i];

                bool found = false;

                if (curr_con.p1 == p1.uid && curr_con.p2 == p2.uid)
                    found = true;

                if (curr_con.p1 == p2.uid && curr_con.p2 == p1.uid)
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

    public List<PathPoint> GetPathPointConexions(PathPoint pp)
    {
        List<PathPoint> ret = new List<PathPoint>();

        if (pp != null)
        {
            for (int i = 0; i < path_point_conexions.Count; ++i)
            {
                PathPointConexions curr_con = path_point_conexions[i];

                if (curr_con.p1 == pp.uid)
                {
                    PathPoint curr = GetPathPointFromUid(curr_con.p2);

                    if(curr != null)
                        ret.Add(curr);
                }

                else if (curr_con.p2 == pp.uid)
                {
                    PathPoint curr = GetPathPointFromUid(curr_con.p1);

                    if(curr != null)
                        ret.Add(curr);
                }
            }
        }

        return ret;
    }

    public List<int> GetPathPointConexionsIndex(PathPoint pp)
    {
        List<int> ret = new List<int>();

        if (pp != null)
        {
            for (int i = 0; i < path_point_conexions.Count; ++i)
            {
                PathPointConexions curr_con = path_point_conexions[i];

                if (curr_con.p1 == pp.uid)
                {
                    int curr = GetPathPointIndexFromUid(curr_con.p2);

                    if (curr != -1)
                        ret.Add(curr);
                }

                else if (curr_con.p2 == pp.uid)
                {
                    int curr = GetPathPointIndexFromUid(curr_con.p1);

                    if (curr != -1)
                        ret.Add(curr);
                }
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

        for (int i = 0; i < path_point_conexions.Count; ++i)
        {
            PathPointConexions curr_con = path_point_conexions[i];

            if (curr_con.line != null)
            {
                PathPoint p1 = GetPathPointFromUid(curr_con.p1);
                PathPoint p2 = GetPathPointFromUid(curr_con.p2);

                Vector3 start_pos = new Vector3(p1.RealPos().x, p1.RealPos().y, 1);
                Vector3 end_pos = new Vector3(p2.RealPos().x, p2.RealPos().y, 1);

                curr_con.line.SetPosition(0, start_pos);
                curr_con.line.SetPosition(1, end_pos);
            }
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

            Color col = PathEntityDebugColorFromPathEntityType(curr_point.entity.type);

            Debug.DrawLine(p1, p2, col);
            Debug.DrawLine(p2, p3, col);
            Debug.DrawLine(p3, p4, col);
            Debug.DrawLine(p4, p1, col);

            for(int y = 0; y < path_point_conexions.Count; ++y)
            {
                PathPointConexions curr_con = path_point_conexions[y];

                PathPoint pp1 = GetPathPointFromUid(curr_con.p1);
                PathPoint pp2 = GetPathPointFromUid(curr_con.p2);

                if (pp1 != null && pp2 != null)
                {
                    Vector3 c1 = new Vector3(pp1.RealPos().x, pp1.RealPos().y, transform.position.z);
                    Vector3 c2 = new Vector3(pp2.RealPos().x, pp2.RealPos().y, transform.position.z);

                    Debug.DrawLine(c1, c2);
                }
            }
        }
    }
}
