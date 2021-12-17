using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    private Animator animator;
    public bool isTarget;
    public List<Collider> RagdollParts = new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }
    private void Awake()
    {
        //gameManager = FindObjectOfType<GameManager>();
        SetRagdollParts();

    }

    // Update is called once per frame
    void Update()
    {

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
}
