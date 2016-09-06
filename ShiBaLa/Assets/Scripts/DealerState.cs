using UnityEngine;
using System.Collections;

public class DealerState : MonoBehaviour
{
    // public
    [HideInInspector]
    public bool isRolled, isChangeState;
    // private
    private RollDiceManager mRollDiceManager;

	// Use this for initialization
	void Start () {
        mRollDiceManager = GetComponent<RollDiceManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.CurrentState == GameManager.GameState.DealerState && GameManager.Instance.NextState != GameManager.GameState.ReRollState)
        {
            if (!isRolled)
                Roll();
            // when dice score label appear, and dealer score doesn't assign
            if (GameObject.Find("Dice Score").transform.localScale != new Vector3(0, 0, 0) && !isChangeState)
                StartCoroutine(DelayToChangeState()); 
        }
	}
  
    public void Roll()
    {
        // random roll dice position
        float InputScreenX = Random.Range(0, Screen.width);
        float InputScreenY = Random.Range(mRollDiceManager.RollAreaLowerBound + 1, mRollDiceManager.RollAreaUpperBound - 1);
        // if inside the screen bound
        if (mRollDiceManager.RollAreaLowerBound < InputScreenY && InputScreenY < mRollDiceManager.RollAreaUpperBound)
        {
            // destory old dice
            if (GameObject.FindGameObjectWithTag("Dice"))
                foreach (GameObject oldDice in GameObject.FindGameObjectsWithTag("Dice"))
                    Destroy(oldDice);

            GameObject.FindGameObjectWithTag("Bowl").transform.FindChild("Bowl Top Collision").gameObject.SetActive(false); // close the bowl top collision
            mRollDiceManager.DicePos = mRollDiceManager.CalDiceRollPos(InputScreenX, InputScreenY); // the position of dice instantiate
            mRollDiceManager.RollDice(mRollDiceManager.DicePos);

            // set rolled dice to lock Roll function
            isRolled = true;
        }  
    }

    
    IEnumerator DelayToChangeState()
    {
        // lock the DelayToChangeState function
        isChangeState = true;

        yield return new WaitForSeconds(0.5f);

        /* Reset Dice, Score, Score texture, isRolled */

        mRollDiceManager.DiceScore = 0; // reset dice score
        GameObject.Find("Dice Score").transform.localScale = new Vector3(0, 0, 0); // rescale the result text to 0
 
        /*********************************************/

        yield return new WaitForSeconds(0.5f);

        // Change state
        if (GameManager.Instance.ResultType.CompareTo("無點重擲") == 0)
        {
            GetComponent<ReRollState>().isChangeState = false;
            GameManager.Instance.NextState = GameManager.GameState.ReRollState;
            GameManager.Instance.PrevState = GameManager.GameState.DealerState;
        }     
        else if (GameManager.Instance.DealerResultType == 8 || GameManager.Instance.DealerResultType == 7 || GameManager.Instance.DealerResultType == 0)
            GameManager.Instance.NextState = GameManager.GameState.ResultState;
        else
            GameManager.Instance.NextState = GameManager.GameState.PlayerState;

            
        /* Reset isRoll, isErease*/ 
        isRolled = false;
    }


}
