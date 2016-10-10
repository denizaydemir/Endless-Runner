using UnityEngine;
using System.Collections;

public class EnemyInnerCollider : MonoBehaviour
{

    void Awake()
    {
        transform.parent.GetComponent<EnemyOuterCollider>().ChildObject = gameObject;
    }

    void OnTriggerEnter(Collider PlayerCollider)
    {
        transform.parent.GetComponent<Animator>().SetBool("IsPlayerNear", true);
        GameManager.DeadByRobot();
        RestartPanelController.ShowPanel();
    }


}
