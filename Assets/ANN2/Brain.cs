using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Replay
{

    public List<double> states;
    public double reward;

	public Replay(float fDist, float rDist, float lDist, float r45Dist, float l45Dist, double r)
	{
		states = new List<double>();

        //fDist = 200;
        //rDist = 200;
        //lDist = 200;
        //r45Dist = 200;
        //l45Dist = 200;

        states.Add(fDist);
        states.Add(rDist);
        states.Add(lDist);
        states.Add(r45Dist);
        states.Add(l45Dist);


        reward = r;
        
	}
}


public class Brain : MonoBehaviour {

    static readonly string CIRCUITO1 = "Circuito1";

    public string currentAleatoryCircuitName;
    public List<float> pointsCircuitName;

    public bool _isAleatoryCircuit;
    public static event Action Death;

    public float speed = 50.0f;
    public float rotationSpeed = 500.0f;
    public float visibleDistance = 200.0f;
    public LayerMask terrainLayer;

    public GameObject ball;                         //object to monitor
    private Manager manager;


	ANN ann;

	float reward = 0.0f;							//reward to associate with actions
	List<Replay> replayMemory = new List<Replay>();	//memory - list of past actions and rewards
	int mCapacity = 10000;							//memory capacity
	
	float discount = 0.99f;							//how much future states affect rewards
	float exploreRate = 100.0f;						//chance of picking random action
	float maxExploreRate = 100.0f;					//max chance value
    float minExploreRate = 0.01f;					//min chance value
    float exploreDecay = 0.0001f;					//chance decay amount for each update

	Vector3 ballStartPos;							//record start position of object
    Quaternion ballStartRot;
    int failCount = 0;								//count when the ball is dropped
	public float tiltSpeed = 0.5f;						    //max angle to apply to tilting each update
													//make sure this is large enough so that the q value
													//multiplied by it is enough to recover balance
													//when the ball gets a good speed up
	float timer = 0;								//timer to keep track of balancing
	float maxBalanceTime = 0;                       //record time ball is kept balanced	
                                                    // Use this for initialization
    private string weightsString;


    private void Awake()
    {
        manager = FindObjectOfType<Manager>();
    }

    public void Start () {
		ann = new ANN(5,2,1,10,0.5f);

        var aux = PlayerPrefs.GetString("Weights");
        //if (aux != null)
        //ann.LoadWeights(aux);
        if (_isAleatoryCircuit)
        {
            if (manager.GetDictionaryCreate() && manager.GetNameDictionary(currentAleatoryCircuitName))
            {
                Debug.Log(manager.GetDataDictionary(currentAleatoryCircuitName));
                ann.LoadWeights(manager.GetDataDictionary(currentAleatoryCircuitName));
            }
            
            /*
            if (SaveAndLoad.SaveExists(currentAleatoryCircuitName+".txt"))
            {
                Debug.Log("Cargar");
                List<string> loadContent = new List<string>();
                loadContent = SaveAndLoad.Load<List<string>>(currentAleatoryCircuitName+".txt");
                Debug.Log(loadContent[0] + "\n" + loadContent[1]);
                ann.LoadWeights(loadContent[1]);
            }*/
        }

        ballStartPos = ball.transform.position;
        ballStartRot = ball.transform.rotation;
        Time.timeScale = 25.0f;		//
    }

	GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		guiStyle.fontSize = 25;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup (new Rect (10, 10, 600, 150));
		GUI.Box (new Rect (0,0,140,140), "Stats", guiStyle);
		GUI.Label(new Rect (10,25,500,30), "Fails: " + failCount, guiStyle);
		GUI.Label(new Rect (10,50,500,30), "Decay Rate: " + exploreRate, guiStyle);
		GUI.Label(new Rect (10,75,500,30), "Last Best Balance: " + maxBalanceTime, guiStyle);
		GUI.Label(new Rect (10,100,500,30), "This Balance: " + timer, guiStyle);
        GUI.Label(new Rect(10, 125, 500, 30), "Reward: " + reward, guiStyle);
        GUI.EndGroup ();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            maxBalanceTime = 0;
            ResetBall();
        }

        if (Input.GetKeyDown("j"))
        {
           
            Debug.Log(ann.PrintWeights());
        }


        Debug.DrawRay(transform.position, this.transform.forward * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, this.transform.right * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, -this.transform.right * visibleDistance, Color.red);

        Debug.DrawRay(transform.position, (Quaternion.AngleAxis(-45, Vector3.up) * this.transform.right) * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, (Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right) * visibleDistance, Color.red);


        weightsString =  ann.PrintWeights();


            //

    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("Weights", weightsString);
    }

    void FixedUpdate () {
		timer += Time.deltaTime;
		List<double> states = new List<double>();
		List<double> qs = new List<double>();

        RaycastHit hit;

        float fDist = visibleDistance, rDist = visibleDistance, lDist = visibleDistance, r45Dist = visibleDistance, l45Dist = visibleDistance;



        if (Physics.Raycast(transform.position, this.transform.forward, out hit, visibleDistance, terrainLayer))
        {
            fDist = Vector3.Distance(transform.position, hit.point);
           
        }

        if (Physics.Raycast(transform.position, this.transform.right, out hit, visibleDistance, terrainLayer))
        {
            rDist = Vector3.Distance(transform.position, hit.point);

        }

        if (Physics.Raycast(transform.position, -this.transform.right, out hit, visibleDistance, terrainLayer))
        {
            lDist = Vector3.Distance(transform.position, hit.point);
        }

        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-45, Vector3.up) * this.transform.right, out hit, visibleDistance, terrainLayer))
        {
            r45Dist = Vector3.Distance(transform.position, hit.point);
        }

        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right, out hit, visibleDistance, terrainLayer))
        {
            l45Dist = hit.distance;
        }

       // Debug.Log("Frontal: " + fDist + ", Derecha: " + rDist + ", Izquierda: " + lDist + ", Derecha45: " + r45Dist + ", Izquierda45: " + l45Dist);

        states.Add(fDist);
        states.Add(rDist);
        states.Add(lDist);
        states.Add(r45Dist);
        states.Add(l45Dist);

        qs = SoftMax(ann.CalcOutput(states));
		double maxQ = qs.Max();
		int maxQIndex = qs.ToList().IndexOf(maxQ);

		//exploreRate = Mathf.Clamp(exploreRate - exploreDecay, minExploreRate, maxExploreRate);


       //if(Random.Range(0,100) < exploreRate) 
        //	maxQIndex = Random.Range(0,2);  


        float translation = speed * Time.deltaTime;

        this.transform.Translate(0, 0, translation);



        





        if (maxQIndex == 0)
			this.transform.Rotate(Vector3.up,tiltSpeed * (float)qs[maxQIndex]);
		else if (maxQIndex == 1)
			this.transform.Rotate(Vector3.up, -tiltSpeed * (float)qs[maxQIndex]);

        
        if (ball.GetComponent<BallState>().dropped)
        {
            reward = -1.0f;
            //reward = 0;
        }

        else if (ball.GetComponent<BallState>().point)
        {
            reward = 0.5f;

        }
        else
            reward = 0.1f;// + 0.01f;



        Replay lastMemory = new Replay(fDist,rDist,lDist,r45Dist,l45Dist,
								reward);

		if(replayMemory.Count > mCapacity)
			replayMemory.RemoveAt(0);
		
		replayMemory.Add(lastMemory);

		if(ball.GetComponent<BallState>().dropped) 
		{
            ResetBall();    //Para que no se quede pillado al no tener archivo.
             
			for(int i = replayMemory.Count - 1; i >= 0; i--)
			{
				List<double> toutputsOld = new List<double>();
				List<double> toutputsNew = new List<double>();
				toutputsOld = SoftMax(ann.CalcOutput(replayMemory[i].states));	

				double maxQOld = toutputsOld.Max();
				int action = toutputsOld.ToList().IndexOf(maxQOld);

			    double feedback;
				if(i == replayMemory.Count-1 || replayMemory[i].reward == -1)
					feedback = replayMemory[i].reward;
				else
				{
					toutputsNew = SoftMax(ann.CalcOutput(replayMemory[i+1].states));
					maxQ = toutputsNew.Max();
					feedback = (replayMemory[i].reward + 
						discount * maxQ);
				} 

				toutputsOld[action] = feedback;
				ann.Train(replayMemory[i].states,toutputsOld);
			}
		
			

			timer = 0;

			ball.GetComponent<BallState>().dropped = false;
			this.transform.rotation = Quaternion.identity;
			ResetBall();
			replayMemory.Clear();
			failCount++;
            //reward = 0;/////////////////////////////////
		}
        
        if (ball.GetComponent<BallState>().meta)
        {

            ball.GetComponent<BallState>().meta = false;
            //string pesos = PlayerPrefs.GetString("Weights");
            string pesos = ann.PrintWeights();

            if (_isAleatoryCircuit)
            {
                /*List<string> saveFileContent = new List<string>();
                saveFileContent.Add(currentAleatoryCircuitName);
                saveFileContent.Add(pesos);
                SaveAndLoad.Save(saveFileContent, currentAleatoryCircuitName + ".txt");*/

                manager.SaveNewDataDictionary(currentAleatoryCircuitName, pesos);          
            }
            /*
            if (!_isAleatoryCircuit && maxBalanceTime <= 0)
            {
                pesos = ann.PrintWeights();
                SaveAndLoad.Save(pesos, CIRCUITO1);
            }
            else if(!_isAleatoryCircuit && maxBalanceTime > timer)
            {
                pesos = ann.PrintWeights();
                SaveAndLoad.Save(pesos, CIRCUITO1);
            }*/
            maxBalanceTime = timer;
            Debug.Log(maxBalanceTime);
            timer = 0;

        }



    }

	void ResetBall()
	{
        /*
        Death();
        Destroy(this.gameObject);//QUITAR ESTO 
        */
        /*      Esto se puede utilizar cuando funcione perfectamente el cargado
         * if (manager.GetDictionaryCreate() && manager.GetNameDictionary(currentAleatoryCircuitName))
        {
            Debug.Log(manager.GetDataDictionary(currentAleatoryCircuitName));
            ann.LoadWeights(manager.GetDataDictionary(currentAleatoryCircuitName));
        }*/

        ball.transform.position = ballStartPos;
        ball.transform.rotation = ballStartRot;
        
		//ball.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
		//ball.GetComponent<Rigidbody>().angularVelocity = new Vector3(0,0,0);
	}

	List<double> SoftMax(List<double> values) 
    {
      double max = values.Max();

      float scale = 0.0f;
      for (int i = 0; i < values.Count; ++i)
        scale += Mathf.Exp((float)(values[i] - max));

      List<double> result = new List<double>();
      for (int i = 0; i < values.Count; ++i)
        result.Add(Mathf.Exp((float)(values[i] - max)) / scale);

      return result; 
    }
}
