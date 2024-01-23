using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class healthManager : MonoBehaviour
{
    public GameObject player;
    public Image healthbar;
    public float healthamount;  
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        takedamage();
    }

    public void takedamage()
    {
        healthamount = PlayerMovement.health;
        healthbar.fillAmount = healthamount / 100f;
    }
}
