using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    // 충돌이 시작할 때 발생하는 이벤트
    void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.tag == "BULLET")
        //if (collision.gameObject.tag == "BULLET")
        //if (collision.gameObject.tag.Equals("BULLET"))
        if (collision.collider.CompareTag("BULLET"))
        {
            Destroy(collision.gameObject);
        }
    }
}
