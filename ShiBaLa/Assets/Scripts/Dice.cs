using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

    private bool isStatic = false;
    private RollDiceManager mRollDiceManager;

	// Use this for initialization
    void Start()
    {
        mRollDiceManager = GameObject.Find("GameManager").GetComponent<RollDiceManager>();
    }

	// Update is called once per frame
	void Update () {

        if (this.gameObject.GetComponent<Rigidbody>().IsSleeping() && !isStatic)
        {
            int side = CalcSideUp();
            if (side > 0)
            {
                mRollDiceManager.DiceScore += side;
                if (GameManager.Instance.CurrentState == GameManager.GameState.DealerState)
                    GameManager.Instance.DealerDiceList.Add(side);
                else if (GameManager.Instance.CurrentState == GameManager.GameState.PlayerState)
                    GameManager.Instance.PlayerDiceList.Add(side);

                mRollDiceManager.StaticDiceNum++;
                isStatic = true;
            }
            else // 當骰子無效的時候重擲
           {
                // set dice collision and static number = 0
                mRollDiceManager.CollisionNum = 0;
                mRollDiceManager.StaticDiceNum = 0;
         
                // reset isChangeState, isRolled and set PrevState            
                if (GameManager.Instance.CurrentState == GameManager.GameState.DealerState)
                {    
                    GameObject.Find("GameManager").GetComponent<ReRollState>().isChangeState = false;
                    GameManager.Instance.PrevState = GameManager.GameState.DealerState;
                    GameObject.Find("GameManager").GetComponent<DealerState>().isRolled = false;
                    GameManager.Instance.NextState = GameManager.GameState.ReRollState;
                }       
                else if (GameManager.Instance.CurrentState == GameManager.GameState.PlayerState)
                {
                    Roll();
                }  
            }
            
        }

	}

    private int CalcSideUp() {
        
        float dotFwd = Vector3.Dot(transform.forward, Vector3.up);
        if (dotFwd > 0.75f) return 1;
        if (dotFwd < -0.75f) return 6;
        
        float dotRight = Vector3.Dot(transform.right, Vector3.up);
        if (dotRight > 0.75f) return 2;
        if (dotRight < -0.75f) return 5;
         
        float dotUp = Vector3.Dot(transform.up, Vector3.up);
        if (dotUp > 0.75f) return 4;
        if (dotUp < -0.75f) return 3;
         
        return 0;
     }

    private bool isCollision;
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bowl") && !isCollision)
        {
            mRollDiceManager.CollisionNum++;
            isCollision = true;  
        }
    }
    // 當player擲出的骰子無效時，則自動重擲
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
        }
    }

}
