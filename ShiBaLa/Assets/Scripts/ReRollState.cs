using UnityEngine;
using System.Collections;

public class ReRollState : MonoBehaviour {

    // public
    [HideInInspector]
    public bool isChangeState;
    // private
   // private RollDiceManager mRollDiceManager;

	// Use this for initialization
	void Start () {
        //mRollDiceManager = GetComponent<RollDiceManager>();
	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.Instance.CurrentState == GameManager.GameState.ReRollState)
            if (!isChangeState)
                StartCoroutine(DelayToChangeState());
	}

    IEnumerator DelayToChangeState()
    {
        // lock the DelayToChangeState function
        isChangeState = true;
       
        // reset list
        if (GameManager.Instance.PrevState == GameManager.GameState.DealerState)
        {
            GameManager.Instance.DealerDiceList.Clear();
            GetComponent<DealerState>().isChangeState = false;
        }
        else if (GameManager.Instance.PrevState == GameManager.GameState.PlayerState)
        {
            GameManager.Instance.PlayerDiceList.Clear();
            GetComponent<PlayerState>().isChangeState = false;
        }

        GameManager.Instance.ResultType = "";

        yield return new WaitForSeconds(0.1f);

        // Change state
        GameManager.Instance.NextState = GameManager.Instance.PrevState;
    }
}
