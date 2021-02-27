using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Creatuve
{
    public int baseSpeed;
    abstract public void move();
    public Creatuve(int bSpeed) { baseSpeed = bSpeed; }
}
class Fish:Creatuve
{
    public int vision { get; set; }
    public override void move()
    {

    }
    public Fish(int bSpeed, int Vision):base(bSpeed) { vision = Vision; }
}
class AgresiveFish:Fish
{
    
    public int attackRange { get; set; }
    public override void move()
    {
        Debug.Log(baseSpeed);
        Debug.Log(vision);
        Debug.Log(attackRange);
        
    }
    public AgresiveFish(int baseSpeed,int vision, int ARange):base(baseSpeed,vision) { attackRange = ARange;}
}

public class agresiveFish : MonoBehaviour
{   
    public GameObject SphereVision;
    
    void Start()
    {
        AgresiveFish f1 = new AgresiveFish(1, 2, 3);

        f1.move();

    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
