using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Pathfinding : MonoBehaviour
{
    public bool debug;
    [SerializeField] private GridGraph graph;

    public delegate float Heuristic(Transform start, Transform end);

    public GridGraphNode startNode;
    public GridGraphNode goalNode;
    public GameObject openPointPrefab;
    public GameObject closedPointPrefab;
    public GameObject pathPointPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Node")))
            {
                if (startNode != null && goalNode != null)
                {
                    startNode = null;
                    goalNode = null;
                    ClearPoints();
                }

                if (startNode == null)
                {
                    startNode = hit.collider.gameObject.GetComponent<GridGraphNode>();
                }
                else if (goalNode == null)
                {
                    goalNode = hit.collider.gameObject.GetComponent<GridGraphNode>();

                    // TODO: use an admissible heuristic and pass it to the FindPath function
                    List<GridGraphNode> path = FindPath(startNode, goalNode);
                }
            }
        }
    }

    public List<GridGraphNode> FindPath(GridGraphNode start, GridGraphNode goal, Heuristic heuristic = null, bool isAdmissible = true)
    {
        if (graph == null) return new List<GridGraphNode>();

        // if no heuristic is provided then set heuristic = 0
        if (heuristic == null) heuristic = (Transform s, Transform e) => 0;

        List<GridGraphNode> path = null;
        bool solutionFound = false;

        // dictionary to keep track of g(n) values (movement costs)
        Dictionary<GridGraphNode, float> gnDict = new Dictionary<GridGraphNode, float>();
        gnDict.Add(start, default);

        // dictionary to keep track of f(n) values (movement cost + heuristic)
        Dictionary<GridGraphNode, float> fnDict = new Dictionary<GridGraphNode, float>();
        fnDict.Add(start, heuristic(start.transform, goal.transform) + gnDict[start]);

        // dictionary to keep track of our path (came_from)
        Dictionary<GridGraphNode, GridGraphNode> pathDict = new Dictionary<GridGraphNode, GridGraphNode>();
        pathDict.Add(start, null);

        List<GridGraphNode> openList = new List<GridGraphNode>();
        openList.Add(start);

        OrderedDictionary closedODict = new OrderedDictionary();

        while (openList.Count > 0)
        {
            // mimic priority queue and remove from the back of the open list (lowest fn value)
            GridGraphNode current = openList[openList.Count - 1];
            openList.RemoveAt(openList.Count - 1);

            closedODict[current] = true;

            // early exit
            if (current == goal && isAdmissible)
            {
                solutionFound = true;
                break;
            }
            else if (closedODict.Contains(goal))
            {
                // early exit strategy if heuristic is not admissible (try to avoid this if possible)
                float gGoal = gnDict[goal];
                bool pathIsTheShortest = true;

                foreach (GridGraphNode entry in openList)
                {
                    if (gGoal > gnDict[entry])
                    {
                        pathIsTheShortest = false;
                        break;
                    }
                }

                if (pathIsTheShortest) break;
            }

            List<GridGraphNode> neighbors = graph.GetNeighbors(current);
            foreach (GridGraphNode n in neighbors)
            {
                float movement_cost = 1;
                // TODO

                // if neighbor is in closed list then skip
                // ...

                // find gNeighbor (g_next)
                // ...

                // if needed: update tables, calculate fn, and update open_list using FakePQListInsert() function
                // ...
            }
        }

        // if the closed list contains the goal node then we have found a solution
        if (!solutionFound && closedODict.Contains(goal))
            solutionFound = true;

        if (solutionFound)
        {
            // TODO
            // create the path by traversing the previous nodes in the pathDict
            // starting at the goal and finishing at the start
            path = new List<GridGraphNode>();

            // ...

            // reverse the path since we started adding nodes from the goal 
            path.Reverse();
        }

        if (debug)
        {
            ClearPoints();

            List<Transform> openListPoints = new List<Transform>();
            foreach (GridGraphNode node in openList)
            {
                openListPoints.Add(node.transform);
            }
            SpawnPoints(openListPoints, openPointPrefab, Color.magenta);

            List<Transform> closedListPoints = new List<Transform>();
            foreach (DictionaryEntry entry in closedODict)
            {
                GridGraphNode node = (GridGraphNode) entry.Key;
                if (solutionFound && !path.Contains(node))
                    closedListPoints.Add(node.transform);
            }
            SpawnPoints(closedListPoints, closedPointPrefab, Color.red);

            if (solutionFound)
            {
                List<Transform> pathPoints = new List<Transform>();
                foreach (GridGraphNode node in path)
                {
                    pathPoints.Add(node.transform);
                }
                SpawnPoints(pathPoints, pathPointPrefab, Color.green);
            }
        }

        return path;
    }

    private void SpawnPoints(List<Transform> points, GameObject prefab, Color color)
    {
        for (int i = 0; i < points.Count; ++i)
        {
#if UNITY_EDITOR
            // Scene view visuals
            points[i].GetComponent<GridGraphNode>()._nodeGizmoColor = color;
#endif

            // Game view visuals
            GameObject obj = Instantiate(prefab, points[i].position, Quaternion.identity, points[i]);
            obj.name = "DEBUG_POINT";
            obj.transform.localPosition += Vector3.up * 0.5f;
        }
    }

    private void ClearPoints()
    {
        foreach (GridGraphNode node in graph.nodes)
        {
            for (int c = 0; c < node.transform.childCount; ++c)
            {
                if (node.transform.GetChild(c).name == "DEBUG_POINT")
                {
                    Destroy(node.transform.GetChild(c).gameObject);
                }
            }
        }
    }

    /// <summary>
    /// mimics a priority queue here by inserting at the right position using a loop
    /// not a very good solution but ok for this lab example
    /// </summary>
    /// <param name="pqList"></param>
    /// <param name="fnDict"></param>
    /// <param name="node"></param>
    private void FakePQListInsert(List<GridGraphNode> pqList, Dictionary<GridGraphNode, float> fnDict, GridGraphNode node)
    {
        if (pqList.Count == 0)
            pqList.Add(node);
        else
        {
            for (int i = pqList.Count - 1; i >= 0; --i)
            {
                if (fnDict[pqList[i]] > fnDict[node])
                {
                    pqList.Insert(i + 1, node);
                    break;
                }
                else if (i == 0)
                    pqList.Insert(0, node);
            }
        }
    }
}
