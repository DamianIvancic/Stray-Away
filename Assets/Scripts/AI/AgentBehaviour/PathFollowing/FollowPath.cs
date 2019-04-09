using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

    public List<GameObject> Nodes;

    private int _pathIdx = 0;

    public Vector3 SetPosition(Vector3 currentPos)
    {
        if ((Nodes[_pathIdx].transform.position - currentPos).magnitude < 0.1f)
        {
            _pathIdx++;
            if (_pathIdx > Nodes.Count - 1)
                _pathIdx = 0;
        }

        Vector3 targetPos = Nodes[_pathIdx].transform.position;
        return targetPos;
    }


}
