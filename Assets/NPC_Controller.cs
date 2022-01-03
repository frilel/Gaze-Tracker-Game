using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Controller : MonoBehaviour
{
    //private 
    public NavMeshAgent agent;
    private Vector3 pos1;
    private Vector3 pos2;
    public bool isTarget = false;
    public bool isPedestrian = true;
    public int animationStatus;//0 talking, 1 taking02, 2 walking, 3 walking02, 4 walk briefcase, 5 sitting, 6 sitlaughing, 7 policeStanding
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponentInChildren<Animator>();
        pos1=transform.GetChild(2).GetChild(0).position;
        pos2=transform.GetChild(2).GetChild(1).position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPedestrian)
        {
            WalkBackAndForth();
        }

    }
    void WalkBackAndForth()
    {
        if (Vector3.Distance(transform.position, pos1) < 0.5f)
        {
            agent.SetDestination(pos2);
        }
        if (Vector3.Distance(transform.position, pos2) < 0.5f)
        {
            agent.SetDestination(pos1);
        }

    }
}
