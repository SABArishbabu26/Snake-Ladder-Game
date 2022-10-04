using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    public Transform[] nodes;
    public List<Transform> nodeList = new List<Transform>();

    void Start()
    {
        FillNodes();
    }

    void FillNodes()
    {
        nodeList.Clear();
        nodes = GetComponentsInChildren<Transform>();

        int num = -1;
        foreach (Transform child in nodes)
        {
            Node n = child.GetComponent<Node>();
            if (child != this.transform && n != null)
            {
                num++;
                nodeList.Add(child);
                child.gameObject.name = "field" + num;
                //Fill in node Id in the node
                n.SetNodeId(num);
            }
        }
    }
    void OnDrawGizmos()
    {
        FillNodes();

        for (int i = 0; i < nodeList.Count; i++)
        {
            Vector3 start = nodeList[i].position;
            if (i > 0)
            {
                Vector3 prev = nodeList[i - 1].position;
                Debug.DrawLine(prev, start, Color.green);
            }
        }
    }
}
