using UnityEngine;
using System.Collections;

public class RollDiceManager : MonoBehaviour
{
    // public
    public GameObject Dice;
    [HideInInspector]
    public int CollisionNum, StaticDiceNum, TotalDiceNum;
    [HideInInspector]
    public int DiceScore;
    [HideInInspector]
    public Camera mCamera;
    [HideInInspector]
    public float RollAreaLowerBound, RollAreaUpperBound;
    [HideInInspector]
    public float Bowl_R, Bowl_X, Bowl_Y;
    [HideInInspector]
    public Vector2 Bowl_Center;
    [HideInInspector]
    public Vector3 BowlLowerBound, BowlUpperBound;
    [HideInInspector]
    public Vector3 DicePos = new Vector3();

	// Use this for initialization
	void Start () {
        // initial
        mCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        GameObject.Find("Dice Score").GetComponent<UILabel>().text = "";
        TotalDiceNum = GameManager.Instance.TotalDiceNum;
        // set the bound of screen
        RollAreaLowerBound = 0.25f * Screen.height;
        RollAreaUpperBound = 0.80f * Screen.height;
        // set the bound of bowl
        Bowl_X = -0.81f;
        Bowl_Y = -0.81f;
        BowlLowerBound = mCamera.WorldToScreenPoint(new Vector3(Bowl_X, 0.38f, Bowl_Y)); // x:left, y:bottom
        BowlUpperBound = mCamera.WorldToScreenPoint(new Vector3(-Bowl_X, 0.38f, -Bowl_Y)); // x:right, y:top
        // bowl center point
        Bowl_Center.x = (BowlUpperBound.x + BowlLowerBound.x) / 2;
        Bowl_Center.y = (BowlUpperBound.y + BowlLowerBound.y) / 2; 
        Bowl_R = (BowlUpperBound.x - BowlLowerBound.x) / 2; // bowl R
	}
	
	// Update is called once per frame
	void Update () {
   
        // check to open the bowl top collision when all dice collide bowl
        if (CollisionNum == TotalDiceNum)
        {
            GameObject.FindGameObjectWithTag("Bowl").transform.FindChild("Bowl Top Collision").gameObject.SetActive(true); // open the bowl top collision
            CollisionNum = 0;
        }
        // score text
        if (StaticDiceNum == TotalDiceNum)
        {
            GameObject.Find("Dice Score").GetComponent<UILabel>().text = GameManager.Instance.ResultType;
            GameObject.Find("Dice Score").GetComponent<Animation>().CrossFade("DiceScore");
            StaticDiceNum = 0;
        }

        /*if (Input.GetMouseButtonDown(1))
        {
            Application.LoadLevel(Application.loadedLevel);
        }*/

	}

    public Vector3 CalDiceRollPos(float x, float y)
    {
        Vector3 newDicePos = mCamera.ScreenToWorldPoint(new Vector3(x, y, BowlLowerBound.z)); 
        newDicePos.z += 0.22f; // main camera has 60 drag, so we must mul height of roll to tan(30drag) and add to z
        // set x and y with sub bowl center (circle center = (0, 0)
        x = x - Bowl_Center.x; 
        y = y - Bowl_Center.y; 
        // outside bowl circle 
        if ((x * x + y * y) > Bowl_R * Bowl_R)
        {
            newDicePos.x = Mathf.Sqrt(Bowl_R * Bowl_R * (x * x) / (x * x + y * y));
            if (x < 0)
                newDicePos.x *= -1;

            newDicePos.y = (y / x * newDicePos.x);
            // add the offest of bowl center point value 
            newDicePos.x += Bowl_Center.x;
            newDicePos.y += Bowl_Center.y;
            // set screen point to world point
            newDicePos = mCamera.ScreenToWorldPoint(new Vector3(newDicePos.x, newDicePos.y, BowlLowerBound.z));
            newDicePos.z += 0.22f; // main camera has 60 drag, so we must mul height of roll to tan(30drag) and add to z
        }

        return newDicePos;
    }

    public void RollDice(Vector3 DicePos)
    {
        GameObject[] DiceClone = new GameObject[3];
        
        for (int i = 1; i < 4; i++)
        {
            // instantiate the dice
            if (DicePos.x < 0 && DicePos.z > 0)
                DiceClone[i-1] = (GameObject)Instantiate(Dice, new Vector3(DicePos.x + i * 0.15f, DicePos.y + i * 0.1f, DicePos.z - i * 0.15f), Quaternion.identity);
            else if (DicePos.x < 0 && DicePos.z < 0)
                DiceClone[i-1] = (GameObject)Instantiate(Dice, new Vector3(DicePos.x + i * 0.15f, DicePos.y + i * 0.1f, DicePos.z + i * 0.15f), Quaternion.identity);
            else if (DicePos.x > 0 && DicePos.z > 0)
                DiceClone[i-1] = (GameObject)Instantiate(Dice, new Vector3(DicePos.x - i * 0.15f, DicePos.y + i * 0.1f, DicePos.z - i * 0.15f), Quaternion.identity);
            else if (DicePos.x > 0 && DicePos.z < 0)
                DiceClone[i-1] = (GameObject)Instantiate(Dice, new Vector3(DicePos.x - i * 0.15f, DicePos.y + i * 0.1f, DicePos.z + i * 0.15f), Quaternion.identity);
            // calculate vector of mouse move
            Vector3 ForceVec = new Vector3(0, 0.15f, 0) - DicePos;
            ForceVec.y = -1; // set vector y to down (-1)
            // random create the dice rotate angle
            Vector3 RotateVec = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
            float maxAngVel = ForceVec.magnitude * 20; // rotate velocity
            // set the force to dice
            DiceClone[i-1].GetComponent<Rigidbody>().maxAngularVelocity = maxAngVel; // set the rotate velocity
            DiceClone[i-1].GetComponent<Rigidbody>().AddTorque(RotateVec); // set the rotate angle vector
            DiceClone[i-1].GetComponent<Rigidbody>().AddForce(ForceVec); //set the force
        }

    }

}
