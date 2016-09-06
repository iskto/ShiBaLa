using UnityEngine;
using System.Collections;

public class StartState : MonoBehaviour {

    [HideInInspector]
    public bool isInitial, isNotFirst;
    public GameObject StartButton;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.CurrentState == GameManager.GameState.StartState)
        {
            if (!isInitial)
            {
                isInitial = true;
                // set the initial Sausage number in BoardArea
                GameObject.Find("SausageBoardBackground").transform.Find("Sausage Number").GetComponent<UILabel>().text = GameManager.Instance.SausageNum.ToString();
                GameObject.Find("RiceSausageBoardBackground").transform.FindChild("Rice Sausage Number").GetComponent<UILabel>().text = GameManager.Instance.RiceSausageNum.ToString();
                // set bet number
                GameObject.Find("Bet Sausage").transform.Find("Bet Sausage Num").GetComponent<UILabel>().text = GameManager.Instance.BetSausageNum.ToString();
                GameObject.Find("Bet Rice Sausage ").transform.FindChild("Bet Rice Sausage Num").GetComponent<UILabel>().text = GameManager.Instance.BetRiceSausageNum.ToString();
                // the start button after speaking 
                if (!isNotFirst)
                {
                    isNotFirst = true;
                    StartButton.SetActive(true);
                }
                else
                    StartCoroutine(DelayToChangeState());
            }
        }
	}

    public void StartButtonClick()
    {
        StartCoroutine(DelayToChangeState());
    }

    IEnumerator DelayToChangeState()
    {
        StartButton.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        GetComponent<ResultState>().isChangeState = false;
        
        GameManager.Instance.NextState = GameManager.GameState.DealerState;    
    }
}
