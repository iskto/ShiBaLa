using UnityEngine;
using System.Collections;

public class ResultState : MonoBehaviour {

    // public 
    [HideInInspector]
    public bool isChangeState;
    /* BetArea */
    public GameObject BetBoard;
    public UISlider SausageSlider, RiceSausageSlider;
    /**********/

    // private
    private bool isGetResult;
    private int SausageCurrentValue, RiceSausageCurrentValue;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.CurrentState == GameManager.GameState.ResultState)
        {
            if (!isGetResult)
                GameResult();
       
            // if not bet anymore, you can't play again
            if (BetBoard.activeSelf == true && SausageCurrentValue == 0 && RiceSausageCurrentValue == 0)
                GameObject.Find("OK").GetComponent<UIButton>().isEnabled = false;
            else if (BetBoard.activeSelf == true)
                GameObject.Find("OK").GetComponent<UIButton>().isEnabled = true;
        }
	}

    void WinState()
    {
        // lock the GameResult
        isGetResult = true;

        print("YOU WIN!");
        // add the sausage number you bet
        GameManager.Instance.SausageNum += GameManager.Instance.BetSausageNum;
        GameManager.Instance.RiceSausageNum += GameManager.Instance.BetRiceSausageNum;

        // create Bet Board and initial the slider value
        BetBoard.SetActive(true);
        SausageSlider.value = 0;
        RiceSausageSlider.value = 0; 
    }

    void LoseState()
    {
        // lock the GameResult
        isGetResult = true;

        print("YOU LOSE!");

        // reduce the sausage number you bet
        GameManager.Instance.SausageNum -= GameManager.Instance.BetSausageNum;
        GameManager.Instance.RiceSausageNum -= GameManager.Instance.BetRiceSausageNum;

        if (GameManager.Instance.SausageNum > 0 || GameManager.Instance.RiceSausageNum > 0)
        {
            // create Bet Board and initial the slider value
            BetBoard.SetActive(true);
            SausageSlider.value = 0;
            RiceSausageSlider.value = 0;
        }
        else
        {
            print("Exit Game!");
        }

    }

    void GameResult()
    {
        if (GameManager.Instance.DealerResultType == 8 || GameManager.Instance.DealerResultType == 7)
        {
            LoseState();
        }
        else if (GameManager.Instance.DealerResultType == 0)
        {
            WinState();
        }
        else if (GameManager.Instance.DealerResultType >= GameManager.Instance.PlayerResultType)
        {
            LoseState();
        }
        else
        {
            WinState();
        }
    }
    // OK button of Bet Board
    public void BetAreaClickOK()
    {
            // set the new bet number
            GameManager.Instance.BetSausageNum = SausageCurrentValue;
            GameManager.Instance.BetRiceSausageNum = RiceSausageCurrentValue;
            print("Play Again!");
            StartCoroutine(DelayToChangeState());         
    }
    // Exit button of Bet Board
    public void BetAreaClickExit()
    {
        print("Exit Game");
    }

    // Bet slider
    public void OnValueChange()
    {
        // Suasage 
        SausageCurrentValue = (int)(SausageSlider.value * GameManager.Instance.SausageNum);
        if (GameManager.Instance.SausageNum > 0) // check  div 0
            SausageSlider.value = (float)SausageCurrentValue / GameManager.Instance.SausageNum;
        
        // Rice Suasage
        RiceSausageCurrentValue = (int)(RiceSausageSlider.value * GameManager.Instance.RiceSausageNum);
        if (GameManager.Instance.RiceSausageNum > 0) // check div 0
            RiceSausageSlider.value = (float)RiceSausageCurrentValue / GameManager.Instance.RiceSausageNum;

        // Bet Board's label of sausage number 
        GameObject.Find("Sausage Num").GetComponent<UILabel>().text = SausageCurrentValue.ToString();
        GameObject.Find("Rice Sausage Num").GetComponent<UILabel>().text = RiceSausageCurrentValue.ToString();
    }

    IEnumerator DelayToChangeState()
    {
        isChangeState = true;

        BetBoard.SetActive(false);

        yield return new WaitForSeconds(2);

        GameManager.Instance.NextState = GameManager.GameState.StartState;  // change state

        // Reset --------
        /* ResultState */
        isGetResult = false; // unlock the GameResult
        /* DealerState */
        GameManager.Instance.DealerResultType = 0;
        GameManager.Instance.DealerDiceList.Clear();
        GetComponent<DealerState>().isChangeState = false;
        /* PlayerState */
        GameManager.Instance.PlayerResultType = 0;
        GameManager.Instance.PlayerDiceList.Clear();
        GetComponent<PlayerState>().isChangeState = false;
        /* StartState */
        GetComponent<StartState>().isInitial = false;
        // --------------
    }

}
