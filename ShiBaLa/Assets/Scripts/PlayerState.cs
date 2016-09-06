using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour {

    // public
    public GameObject TapToRoll;
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
        if (GameManager.Instance.CurrentState == GameManager.GameState.PlayerState && GameManager.Instance.NextState != GameManager.GameState.ReRollState)
        {
            if (!isRolled)
            {
                TapToRoll.SetActive(true);
                TapToRoll.GetComponent<Animation>().CrossFade("TapToRoll");
                Roll(); // player roll dice
            }

            // when dice score label appear, and dealer score doesn't assign
            if (GameObject.Find("Dice Score").transform.localScale != new Vector3(0, 0, 0) && !isChangeState)
                StartCoroutine(DelayToChangeState());
        }
	}

    public void Roll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // input roll dice position
            float InputScreenX = Input.mousePosition.x;
            float InputScreenY = Input.mousePosition.y;
            // if inside the screen bound
            if (mRollDiceManager.RollAreaLowerBound < InputScreenY && InputScreenY < mRollDiceManager.RollAreaUpperBound)
            {
                // destory old dice
                if (GameObject.FindGameObjectWithTag("Dice"))
                    foreach (GameObject oldDice in GameObject.FindGameObjectsWithTag("Dice"))
                        Destroy(oldDice);

                TapToRoll.SetActive(false); // close the Tap To Roll text
                GameObject.FindGameObjectWithTag("Bowl").transform.FindChild("Bowl Top Collision").gameObject.SetActive(false); // close the bowl top collision
                mRollDiceManager.DicePos = mRollDiceManager.CalDiceRollPos(InputScreenX, InputScreenY); // the position of dice instantiate
                mRollDiceManager.RollDice(mRollDiceManager.DicePos);

                isRolled = true;
            }
        }
    }

    IEnumerator DelayToChangeState()
    {
        // lock the DelayToChangeState function
        isChangeState = true;

        yield return new WaitForSeconds(0.5f);

        /* Reset Dice, Score, Score texture */

        mRollDiceManager.DiceScore = 0; // reset dice score
        GameObject.Find("Dice Score").transform.localScale = new Vector3(0, 0, 0); // rescale the result text to 0

        /*********************************************/

        yield return new WaitForSeconds(0.5f);

        // Change state
        if (GameManager.Instance.ResultType.CompareTo("無點重擲") != 0)
            GameManager.Instance.NextState = GameManager.GameState.ResultState;
        else
        {
            GetComponent<ReRollState>().isChangeState = false;
            GameManager.Instance.NextState = GameManager.GameState.ReRollState;
            GameManager.Instance.PrevState = GameManager.GameState.PlayerState;
        }

        /* Reset isRoll, isErease*/
        isRolled = false;
    }


}
