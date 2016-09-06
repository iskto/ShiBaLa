using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    // GameManager instance
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    // Game State
    [HideInInspector]
    public enum GameState
    {
        StartState,
        DealerState,
        PlayerState,
        ResultState,
        ReRollState
    }
    [HideInInspector]
    public GameState PrevState, CurrentState, NextState;

    // private
    private bool isRuled;

    // public
    public int TotalDiceNum = 3;
    public int SausageNum, RiceSausageNum;
    public int BetSausageNum, BetRiceSausageNum;
    [HideInInspector]
    public List<int> DealerDiceList, PlayerDiceList;
    [HideInInspector]
    public string ResultType;
    [HideInInspector]
    public int DealerResultType, PlayerResultType;

	// Use this for initialization
	void Start () {
        // initial
        CurrentState = GameState.StartState;
        NextState = GameState.StartState;
        DealerDiceList = new List<int>();
        PlayerDiceList = new List<int>();
        BetSausageNum = 2;
        BetRiceSausageNum = 3; 
    }
	
	// Update is called once per frame
    void Update()
    {
        // check to change state
        if (NextState != CurrentState)
        {
            CurrentState = NextState;
            isRuled = false;
        }
        else 
            GameRule();    
	}

    // 遊戲規則
    void GameRule()
    {
        if (CurrentState == GameState.DealerState && DealerDiceList.Count == TotalDiceNum && !isRuled)
        {
            BubbleSort(DealerDiceList);

            threeDiceRule(DealerDiceList);

            isRuled = true;
        }
        else if (CurrentState == GameState.PlayerState && PlayerDiceList.Count == TotalDiceNum && !isRuled)
        {
            BubbleSort(PlayerDiceList);

            threeDiceRule(PlayerDiceList);

            isRuled = true;
        }
    }

    // 排列大小
    void BubbleSort(List<int> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = 0; j < list.Count - i - 1; j++)
            {
                if (list[j] > list[j + 1])
                {
                    int tmp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = tmp;
                }
            }
        }
    }

    // 三顆骰子的規則
    private string point;
    void threeDiceRule(List<int> list)
    {
        if (list[0] == list[1] && list[1] == list[2])
        {
            ResultType = "豹子";
            ResultTypeToValue(8);
        }  
        else if (list[0] == 4 && list[1] == 5 && list[2] == 6)
        {
            ResultType = "四五六";
            ResultTypeToValue(7);
        } 
        else if (list[0] == 1 && list[1] == 2 && list[2] == 3)
        {
            ResultType = "么二三";
            ResultTypeToValue(0);
        }         
        else if (list[0] == list[1])
        {
            switch(list[2])
            {
                case 1:
                    point = "一";
                    ResultTypeToValue(1);
                    break;
                case 2:
                    point = "二";
                    ResultTypeToValue(2);
                    break;
                case 3:
                    point = "三";
                    ResultTypeToValue(3);
                    break;
                case 4:
                    point = "四";
                    ResultTypeToValue(4);
                    break;
                case 5:
                    point = "五";
                    ResultTypeToValue(5);
                    break;
                case 6:
                    point = "六";
                    ResultTypeToValue(6);
                    break;
            }

            ResultType = point + "點";
        }
        else if (list[1] == list[2])
        {
            switch (list[0])
            {
                case 1:
                    point = "一";
                    ResultTypeToValue(1);
                    break;
                case 2:
                    point = "二";
                    ResultTypeToValue(2);
                    break;
                case 3:
                    point = "三";
                    ResultTypeToValue(3);
                    break;
                case 4:
                    point = "四";
                    ResultTypeToValue(4);
                    break;
                case 5:
                    point = "五";
                    ResultTypeToValue(5);
                    break;
                case 6:
                    point = "六";
                    ResultTypeToValue(6);
                    break;
            }

            ResultType = point + "點";
        }
        else if (list[2] == list[0])
        {
            switch (list[1])
            {
                case 1:
                    point = "一";
                    ResultTypeToValue(1);
                    break;
                case 2:
                    point = "二";
                    ResultTypeToValue(2);
                    break;
                case 3:
                    point = "三";
                    ResultTypeToValue(3);
                    break;
                case 4:
                    point = "四";
                    ResultTypeToValue(4);
                    break;
                case 5:
                    point = "五";
                    ResultTypeToValue(5);
                    break;
                case 6:
                    point = "六";
                    ResultTypeToValue(6);
                    break;
            }

            ResultType = point + "點";
        }
        else
            ResultType = "無點重擲";     
    }

    void ResultTypeToValue(int point)
    {
        if (CurrentState == GameState.DealerState)
            DealerResultType = point;
        else if (CurrentState == GameState.PlayerState)
            PlayerResultType = point;
    }

}


