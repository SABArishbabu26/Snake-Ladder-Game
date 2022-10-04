using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public int nodeId; // Position onthe route
    public Text numberText;
    public Node connectedNode; // snake or ladder

    List<Stone> stoneList = new List<Stone>();

    public void SetNodeId(int _nodeId)
    {
        nodeId = _nodeId;
        if (numberText != null)
        {
            numberText.text = nodeId.ToString();
        }
    }

    void OnDrawGizmos()
    {
        if (connectedNode != null)
        {
            Color col = Color.white;
            col = (connectedNode.nodeId > nodeId) ? Color.blue : Color.red;
            Debug.DrawLine(transform.position, connectedNode.transform.position, col);
        }

    }

    public void AddStone(Stone stone)
    {
        stoneList.Add(stone);
        //REARRANGE
        ReArrangeStones();
    }

    public void RemoveStone(Stone stone)
    {
        stoneList.Remove(stone);
        //REARRANGE
        ReArrangeStones();
    }

    void ReArrangeStones()
    {
        if (stoneList.Count>1)
        {
            int squaresize = Mathf.CeilToInt(Mathf.Sqrt(stoneList.Count));
            int stone = -1;
            for (int x = 0; x < squaresize; x++)
            {
                for (int y = 0; y < squaresize; y++)
                {
                    stone++;
                    if (stone > stoneList.Count-1)
                    {
                        break;
                    }

                    Vector3 newPos = transform.position + new Vector3(-0.25f + x * 0.5f, 0, -0.25f + y * 0.5f);
                    stoneList[stone].transform.position = newPos;
                }
            }
        }
        else if (stoneList.Count == 1)
        {
            stoneList[0].transform.position = transform.position;
        }
    }
}
