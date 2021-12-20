using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Controller : MonoBehaviour
{
    private Animator animator;
    public bool isTarget;
    public List<Collider> RagdollParts = new List<Collider>();
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private Vector3 moveDestinationPosition;
    private GameManager gameManager;
    private bool animationInitiated = false;
    private int currentDestination = 0;
    private Vector3 []movePositions=new Vector3[2];

    public int type = 0; //0 enemies, 1 sitting, 2 sittinglaugh, 3 walking, 4 walking with briefcase;
    // Start is called before the first frame update
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameManager = FindObjectOfType<GameManager>();
        SetRagdollParts();
        if(navMeshAgent)
        {
            movePositions[1]=transform.GetChild(3).GetChild(1).transform.position;
            movePositions[0]=transform.GetChild(3).GetChild(0).transform.position;
        }
    }
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        if (transform.GetChild(3))
        {
            moveDestinationPosition = movePositions[1];
            currentDestination = 1;
        }



    }


    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStarted && !animationInitiated)
        {
            switch (type)
            {
                default: break;
                case 0:
                    animator.SetTrigger("Talking" + Random.Range(0, 3).ToString());
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
            animationInitiated = true;
            if(type==0)
            {
                StartCoroutine(StartPhaseOne());
            }
            if (navMeshAgent&&type!=0)
                CharacterNavigate();
        }
        if (navMeshAgent && Vector3.Distance(transform.position, moveDestinationPosition) < 0.5f&&type!=0)
        {
            //Debug.Log("ss");
            moveDestinationPosition =movePositions[currentDestination==1?0:1];
            currentDestination=currentDestination==1?0:1;
            CharacterNavigate();
        }
        else if(navMeshAgent && Vector3.Distance(transform.position, moveDestinationPosition) < 0.5f&&type==0)
        {
            animator.ResetTrigger("Talking");
            animator.ResetTrigger("Talking1");
            animator.ResetTrigger("Walking");
            animator.SetTrigger("Idle");
        }
    }
    IEnumerator StartPhaseOne()
    {
        yield return new WaitForSeconds(10);
        animator.SetTrigger("Walking");
        CharacterNavigate();

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
                        c.attachedRigidbody.AddForce((transform.up * Random.Range(0.35f, 0.6f) - transform.forward) * Random.Range(100, 50), ForceMode.Impulse);
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
    private void CharacterNavigate()
    {
        navMeshAgent.destination = moveDestinationPosition;
    }
}
