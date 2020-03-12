using Dot.AI.XNodeFSM;
using AI.FSMDemo;
using UnityEngine;

public class TestFSM : MonoBehaviour
{
    public StateMachine stateMachine = null;

    private Agent agent = null;
    void Start()
    {
        agent = new Agent();
        agent.StartFSM(stateMachine);
    }

    private float delayTime = 0.0f;
    void Update()
    {
        delayTime += Time.deltaTime;
        if(delayTime>0.05f)
        {
            agent?.DoUpdate(0.05f);

            delayTime = 0.0f;
        }
    }
}
