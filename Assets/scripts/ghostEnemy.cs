using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class ghostEnemy : MonoBehaviour
{
    public GameObject player;
    Animator anim;

    public float speed;

    //FOV variables
    public float radius;
    [Range(0f, 360f)]
    public float angle;

    public bool canSeePlayer;


    public LayerMask targetMask;
    public LayerMask obstructionMask;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (canSeePlayer)
        {
            lookAtPlayer();
            moveTowardsPlayer();
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
    void moveTowardsPlayer()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            fieldOfViewCheck();
        }
    }
    private void fieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}