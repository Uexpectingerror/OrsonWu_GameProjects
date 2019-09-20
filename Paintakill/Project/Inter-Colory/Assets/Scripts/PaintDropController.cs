using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintDropController : MonoBehaviour
{
    public Transform targetTF;
    [SerializeField] private float rotateSpeed = 10.0f;
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float minDisRush = 10.0f;
    private Vector3 targetDir;
    public ColorState mColorState = ColorState.Clean;
    bool isRush = false;
    // Start is called before the first frame update
    void Start()
    {
        targetDir = (targetTF.position - transform.position).normalized;
        Quaternion lookTarget = Quaternion.LookRotation(targetTF.position - transform.position, Vector3.up);
        transform.rotation = lookTarget;
    }

    // Update is called once per frame
    void Update()
    {
        float dis = (targetTF.position - transform.position).magnitude;
        targetDir = (targetTF.position - transform.position).normalized;
        //if it is far away then rotate slowly
        if (dis > minDisRush && !isRush )
        {
            Quaternion lookTarget = Quaternion.LookRotation(targetTF.position - transform.position, Vector3.up);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, lookTarget, rotateSpeed * Time.deltaTime);
            transform.rotation = rotation;
            transform.Translate(transform.rotation * Vector3.forward * Time.deltaTime * moveSpeed, Space.World);
            print("rotate");
        }
        //if close enough go straight to the target
        else
        {
            isRush = true;
            transform.Translate(targetDir * Time.deltaTime * moveSpeed,Space.World);
            print("rush");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponentInChildren <SuperAttackController>())
            {
                other.gameObject.GetComponentInChildren<SuperAttackController>().TakePaintDrop();
                Destroy(gameObject);
            }
        }
    }
}
