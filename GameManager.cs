using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Dice dice;
    public WinPanel winPanel;

    int activePlayer;
    int diceNumber;

    //[SerializeField] AudioSource music;

    [System.Serializable]
    public class Player
    {
        public string playerName;
        public Stone stone;

        public GameObject rollDiceButton;

        public enum PlayerTypes
        {
            CPU,
            HUMAN
        }
        public PlayerTypes playerType;
    }

    public List<Player> playerList = new List<Player>();

    public enum States
    {
        WAITING,
        ROLL_DICE,
        SWITCH_PLAYER
    }
    public States state;

    void Awake()
    {
        instance = this;

        for (int i = 0; i < playerList.Count; i++)
        {
            if (SaveSettings.players[i] == "HUMAN")
            {
                playerList[i].playerType = Player.PlayerTypes.HUMAN;
            }
            if (SaveSettings.players[i] == "CPU")
            {
                playerList[i].playerType = Player.PlayerTypes.CPU;
            }
        }
    }

    void Start()
    {
        //ActivateButton(false);
        DeactivateAllButtons();
        winPanel.gameObject.SetActive(false);

        activePlayer = Random.Range(0, playerList.Count);
        InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + " starts first!");
    }

    // for music
   /* public void OnMusic()
    {
        music.Play();

    }

    public void OffMusic()
    {
        music.Stop();
    }*/

    void Update()
    {
        if (playerList[activePlayer].playerType == Player.PlayerTypes.CPU)
        {
            switch (state)
            {
                case States.WAITING:
                    {
                        //IDLE
                    }
                    break;
                case States.ROLL_DICE:
                    {
                        StartCoroutine(RollDiceDelay());
                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    {
                        if (diceNumber != 6)
                        {
                            activePlayer++;
                            activePlayer %= playerList.Count;
                            Debug.Log("activePlayer " + activePlayer);
                        }
                        //INFO BOX
                        InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + " 's Turn! ");

                        state = States.ROLL_DICE;
                    }
                    break;
            }
        }

        if (playerList[activePlayer].playerType == Player.PlayerTypes.HUMAN)
        {
            switch (state)
            {
                case States.WAITING:
                    {
                        //IDLE
                    }
                    break;
                case States.ROLL_DICE:
                    {
                        //ActivateButton(true);
                        ActivateSpecificButton(true);
                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    {
                        if (diceNumber != 6)
                        {
                            activePlayer++;
                            activePlayer %= playerList.Count;
                            Debug.Log("activePlayer " + activePlayer);
                        }
                        //INFO BOX
                        InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + " 's Turn! ");

                        state = States.ROLL_DICE;
                    }
                    break;
            }
        }
    }
    
    IEnumerator RollDiceDelay()
    {
        yield return new WaitForSeconds(2);
        //diceNumber = Random.Range(1, 7);

        //ROLL THE PHYSICAL DICE
        dice.RollDice();
    }

    public void RolledNumber(int _diceNumber)//CALLED FROM THE DICE
    {
        diceNumber = _diceNumber;
        //INFO BOX
        InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + " has rolled a " + diceNumber);

        // make a turn
        playerList[activePlayer].stone.MakeTurn(diceNumber);
    }

    //void ActivateButton(bool on)
    //{
    //    rollDiceButton.SetActive(on);
    //}

    //THIS FUNCTION IS ON OUR BUTTON
    public void HumanRollDice()
    {
        //ActivateButton(false);
        ActivateSpecificButton(false);
        StartCoroutine(RollDiceDelay());
    }

    void ActivateSpecificButton(bool on)
    {
        playerList[activePlayer].rollDiceButton.SetActive(on);
    }

    void DeactivateAllButtons()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].rollDiceButton.SetActive(false);
        }
    }

    public void ReportWinner()
    {
        //SHOW WINNING STUFF    
        winPanel.gameObject.SetActive(true);
        winPanel.ShowWinMessage(playerList[activePlayer].playerName, playerList[activePlayer].playerType.ToString());
        //Debug.Log(playerList[activePlayer].playerName + " has won this Game!");
    }
}

