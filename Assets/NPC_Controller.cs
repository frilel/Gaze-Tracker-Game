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
    private Vector3 pos3;
    public bool isTarget = false;
    public bool isPedestrian = true;
    public bool isCluePerson = false;
    [Header("0 talking, 1 sitting, 2 sitlaughing, 3 walk01, 4 walkbrief, 5 idle")]
    //[Tooltip("0 talking, 1 sitting, 2 sitlaughing, 3 walk01, 4 walkbrief, 5 idle")]
    public int animationStatus;//0 talking, 1 sitting, 2 sitlaughing, 3 walk01, 4 walkbrief, 5 idle
    private Animator animator;
    public List<Collider> RagdollParts = new List<Collider>();
    private GameManager gameManager;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player");
        if (isPedestrian)
        {
            pos1 = transform.GetChild(2).GetChild(0).position;
            pos2 = transform.GetChild(2).GetChild(1).position;
        }
        if (isCluePerson)
        {
            pos1 = transform.GetChild(2).GetChild(0).position;
            pos2 = transform.GetChild(2).GetChild(1).position;
            pos3 = transform.GetChild(2).GetChild(2).position;
        }

        switch (animationStatus)
        {
            default: break;
            case 0:
                animator.SetTrigger("Talking" + Random.Range(0, 2).ToString());
                break;
            case 1:
                animator.SetTrigger("Sitting");
                break;
            case 2:
                animator.SetTrigger("SittingLaughing");
                break;
            case 3:
                animator.SetTrigger("Walking1");
                break;
            case 4:
                animator.SetTrigger("WalkWithBriefcase");
                break;
        }
    }
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        SetRagdollParts();

    }
    public void SwitchAnimSuspect(int type)//0 oldmanidle, 1 draw shoot
    {
        if (type == 0)
        {
            animator.SetTrigger("OldManIdle");
        }
        else
        {
            animator.SetTrigger("DrawGun");
        }
    }
    public void TurnOnRagdoll(int option)//0 bodyshot
    {
        animator.enabled = false;
        if (GetComponent<CapsuleCollider>())
        {
            GetComponent<CapsuleCollider>().enabled = false;
        }
        foreach (Collider c in RagdollParts)
        {
            if (c.name != "hitPoint")
            {
                c.isTrigger = false;
                if (c.attachedRigidbody)
                {
                    c.attachedRigidbody.isKinematic = false;
                    c.attachedRigidbody.velocity = Vector3.zero;
                }
                if (option == 0)
                    if (c.name == "mixamorig:Hips")
                        c.attachedRigidbody.AddForce((transform.up * Random.Range(0.35f, 0.6f) + player.transform.forward) * Random.Range(100, 50), ForceMode.Impulse);
            }

        }
    }
    void SetRagdollParts()
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            if (!c.CompareTag("Enemy"))
            {
                c.isTrigger = true;
                RagdollParts.Add(c);
                if (c.attachedRigidbody)
                    c.attachedRigidbody.isKinematic = true;

            }

        }
        //GetComponent<Rigidbody>().useGravity=false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPedestrian && gameManager.gameStarted)
        {
            WalkBackAndForth();
        }

    }
    void WalkBackAndForth()
    {
        if (!isCluePerson)
        {
            if (Vector3.Distance(transform.position, pos1) < 0.25f)
            {
                agent.SetDestination(pos2);
            }
            if (Vector3.Distance(transform.position, pos2) < 0.25f)
            {
                agent.SetDestination(pos1);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, pos1) < 0.25f)
            {
                agent.SetDestination(pos2);
            }
            if (Vector3.Distance(transform.position, pos2) < 0.25f)
            {
                agent.SetDestination(pos3);
            }
            if(Vector3.Distance(transform.position, pos3) < 0.6f)
            {
                animator.ResetTrigger("WalkWithBriefcase");
                animator.SetTrigger("Stop");
            }
        }


    }
}
