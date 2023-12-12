using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class enemy : MonoBehaviour
{

    Vector3 rot = Vector3.zero;
    float rotSpeed = 40f;
    Animator anim;
    public GameObject player;
    public Transform playerpos;
    public LayerMask playermask;

    public bool isAttacking = false;
    bool playerRadius = false;
    public bool target = false;
    public Vector3 playertarget;
    bool forward;
    bool back;
    Vector3 originalpos;
    IEnumerator attack()
    {
        isAttacking = true;

        yield return new WaitForSeconds(3);

        if (target == false)
        {
            playertarget = playerpos.position;
            target = true;
        }
        originalpos = transform.position;
        forward = true;
        yield return new WaitForSeconds(1);
        forward = false;
        back = true;

        target = false;
        isAttacking = false;
        yield return null;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, 5f);
    }

    // Use this for initialization
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        gameObject.transform.eulerAngles = rot;

    }

    // Update is called once per frame
    void Update()
    {
        CheckKey();
        gameObject.transform.eulerAngles = rot;
        lookAtPlayer();
        avoidPlayer();
        if (isAttacking == false)
        {
            StartCoroutine(attack());
        }
        if (forward == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, playertarget, 0.025f);

        }
        if (back == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalpos, 0.025f);

        }
    }
    void lookAtPlayer()
    {
        if (player != null)
        {
            var lookDir = player.transform.position;
            lookDir.y = transform.position.y;
            transform.LookAt(lookDir, Vector3.up);
        }
    }
    void avoidPlayer()
    {
        playerRadius = Physics.CheckSphere(transform.position, 5, playermask);
        if (playerRadius == true && isAttacking == false)
        {

        }

    }
    void CheckKey()
    {
        // Walk
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("Walk_Anim", true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("Walk_Anim", false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            isAttacking = false;
        }

        // Roll
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (anim.GetBool("Roll_Anim"))
            {
                anim.SetBool("Roll_Anim", false);
            }
            else
            {
                anim.SetBool("Roll_Anim", true);
            }
        }

        // Close
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!anim.GetBool("Open_Anim"))
            {
                anim.SetBool("Open_Anim", true);
            }
            else
            {
                anim.SetBool("Open_Anim", false);
            }

        }

    }
}
