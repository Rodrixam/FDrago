using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBodyController : MonoBehaviour
{
    [SerializeField] GameObject leftBody;
    [SerializeField] GameObject leftNeck;
    [SerializeField] GameObject rightBody;
    [SerializeField] GameObject rightNeck;

    FlameController flameController;
    GameObject leftHead;
    GameObject rightHead;

    //[SerializeField] float lerpSpeed = 1f;

    private void Awake()
    {
        flameController = GetComponent<FlameController>();
        leftHead = flameController.leftHead;
        rightHead = flameController.rightHead;
    }

    // Update is called once per frame
    void Update()
    {
        //Activate/Deactivate bodies alongside heads
        if (leftHead.activeInHierarchy != leftBody.activeInHierarchy)
        {
            leftBody.SetActive(leftHead.activeInHierarchy);
            leftNeck.transform.position = leftHead.transform.position;
        }
        if (rightHead.activeInHierarchy != rightBody.activeInHierarchy)
        {
            rightBody.SetActive(rightHead.activeInHierarchy);
            rightNeck.transform.position = rightHead.transform.position;
        }

        //Rotate body according to heads
        Vector3 bodyDirection = Vector3.zero;
        if (leftHead.activeInHierarchy)
        {
            bodyDirection += new Vector3(leftHead.transform.position.x, 0, leftHead.transform.position.z);
        }
        if (rightHead.activeInHierarchy)
        {
            bodyDirection += new Vector3(rightHead.transform.position.x, 0, rightHead.transform.position.z);
        }
        bodyDirection /= 2f;

        if(bodyDirection == Vector3.zero) { return; }
        leftBody.transform.rotation = Quaternion.LookRotation(bodyDirection, transform.up);
        rightBody.transform.rotation = leftBody.transform.rotation;

    }
}
