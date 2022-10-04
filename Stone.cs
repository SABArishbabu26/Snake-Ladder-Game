using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public Route route;
    public List<Node> nodeList = new List<Node>();

    int routePosition;

    int PlayerId;
    float speed = 8f;

    int stepsToMove;
    int doneSteps;

    bool isMoving;

    float cTime = 0;
    float amplitute = 0.5f;

    private bool showPopUpForSnake = false;
    private bool showPopUpForLadder = false;

    private int index = 0;

    [SerializeField] private AudioSource snakeSoundEffect;
    [SerializeField] private AudioSource ladderSoundEffect;

    string[] ladderMsg = new string[] { "Anything is possible!", "I get better every single day!", "I am an amazing person!", "I can ask for a hug when I’m sad.", "I am safe and cared for!", "I can ask for support.",
        "I have a big heart.","I engage in small acts of kindness to uplift other people.","I get better every single day!","My challenges help me grow!"};

    string[] snakeMsg = new string[] { "I let go of negative thoughts.", "Positive thinking is more useful than negative thinking.", "I release anxiety and worry!", "I release any fear of mistakes and imperfection.",
        "I let go of the expectations of others.","I am free from distractions and focused on my goals.","Today is a clean slate!","Obstacles are now falling away easily!","I abandon old habits and choose new, positive ones.",
        "Letting go of negative thinking brings peace."};


    void Start()
    {
        foreach (Transform c in route.nodeList)
        {
            Node n = c.GetComponentInChildren<Node>();
            if (n!=null)
            {
                nodeList.Add(n);
            }
        }
    }

    void OnGUI()
    {
        if (showPopUpForSnake)
        {
            GUI.color = Color.red;
            GUI.Window(0, new Rect((Screen.width / 2) - 300, (Screen.height / 2) - 160
            , 550, 250), ShowGUI, "<size=22><color=yellow>Message for You!</color></size>");
            Debug.Log("inside On showPopUpForSnake");           
        }else if (showPopUpForLadder)
        {
            GUI.color = Color.green;
            GUI.Window(0, new Rect((Screen.width / 2) - 300, (Screen.height / 2) - 160
            , 550, 250), ShowGUI, "<size=24><color=yellow>Message for You!</color></size>");
            Debug.Log("inside On showPopUpForLadder");
        }
    }

    void ShowGUI(int windowID)
    {
        
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 30;
        myStyle.fontStyle = FontStyle.BoldAndItalic;
        myStyle.normal.textColor = Color.white;
        myStyle.wordWrap = true;

        if (showPopUpForSnake)
        {
            //change the parameter value to adjust the font size
            GUI.Label(new Rect(30, 60, 500, 130), snakeMsg[index], myStyle);
            Debug.Log("inside On showgui showPopUpForSnake");
        }
        else if (showPopUpForLadder)
        {
            //GUI.contentColor = Color.white;
            GUI.Label(new Rect(30, 60, 500, 130), ladderMsg[index], myStyle);
            Debug.Log("inside On showgui showPopUpForLadder");
        }

        Debug.Log("inside On showgui");

        if (GUI.Button(new Rect(100, 200, 75, 30), "OK"))
        {
            Debug.Log("inside ok button");
            showPopUpForSnake = false;
            showPopUpForLadder = false;
        }
    }
    IEnumerator PopUpDelay(float waitTime)
    {
        yield return new WaitForSeconds(4);
    }

    IEnumerator Move()
    {
        if (isMoving)
        {
            yield break;
        }
        
        isMoving = true;

        //REMOVE THIS STONE FROM THE ACTUAL NODE
        nodeList[routePosition].RemoveStone(this);

        while (stepsToMove>0)
        {
            routePosition++;
            Vector3 nextPos = route.nodeList[routePosition].transform.position;
            //ARC MOVEMENT
            Vector3 startPos = route.nodeList[routePosition-1].transform.position;
            while (MoveInArcToNextNode(startPos, nextPos, 4f)) { yield return null; }

            //straight movement
            //while (MoveToNextNode(nextPos)) { yield return null; }

            yield return new WaitForSeconds(0.1f);

            cTime = 0;

            stepsToMove--;
            doneSteps++;
        }

        
        yield return new WaitForSeconds(0.1f);
        // Snake and ladder check
        if (nodeList[routePosition].connectedNode != null)
        {
            int conNodeId = nodeList[routePosition].connectedNode.nodeId;
            Vector3 nextPos = nodeList[routePosition].connectedNode.transform.position;
            Debug.Log("conNodeId"+conNodeId);
            Debug.Log("doneSteps"+doneSteps);
            Debug.Log("routePosition"+routePosition);
            while (MoveToNextNode(nextPos)) { yield return null; }
            if(doneSteps < conNodeId)
            {
                ladderSoundEffect.Play();
                index = Random.Range(1, 10);
                showPopUpForLadder = true;
            }
            else if(doneSteps > conNodeId)
            {
                snakeSoundEffect.Play();
                index = Random.Range(1, 10);
                showPopUpForSnake = true;
            }
            doneSteps = conNodeId;
            routePosition = conNodeId;
            Debug.Log("inside snake and ladder");
        }

        //ADD THIS STONE TO THE ACTUAL NODE
        nodeList[routePosition].AddStone(this);

        //CHECK FOR A WIN
        if (doneSteps == nodeList.Count-1)
        {
            //REPORT TO GAMEMANAGER
            GameManager.instance.ReportWinner();
            yield break;
        }

        //UPDATE THE GAMEMANAGER
        GameManager.instance.state = GameManager.States.SWITCH_PLAYER;

        isMoving = false;
       
    }
    
    bool MoveToNextNode(Vector3 nextPos)
    {
        return nextPos != (transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime));
    }

    bool MoveInArcToNextNode(Vector3 startPos, Vector3 nextPos, float _speed)
    {
        cTime += _speed * Time.deltaTime;
        Vector3 myPosition = Vector3.Lerp(startPos, nextPos, cTime);
        myPosition.y += amplitute * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);

        return nextPos != (transform.position = Vector3.Lerp(transform.position, myPosition, cTime));
    }
    public void MakeTurn(int diceNumber)
    {
        stepsToMove = diceNumber;
        if (doneSteps + stepsToMove < route.nodeList.Count)
        {
            StartCoroutine(Move());
        }
        else
        {
            //print("Number is to High");
            //UPDATE THE GAMEMANAGER
            GameManager.instance.state = GameManager.States.SWITCH_PLAYER;
        }
    }
}