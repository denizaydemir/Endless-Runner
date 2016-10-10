using UnityEngine;
using System.Collections;

public class EnemyOuterCollider : MonoBehaviour
{
    public GameObject ChildObject;

    void OnTriggerEnter(Collider PlayerCollider)
    {
        GameManager.SetRobotToKill(gameObject, ChildObject);
    }


    void OnTriggerExit(Collider PlayerCollider)
    {
        GameManager.ResetSelectedRobot();
    }




}
