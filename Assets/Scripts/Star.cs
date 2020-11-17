using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update

    public float r;
    public float rotateRound;   // 행성의 회전 둘레
    public float speed;         // 행성 회전 속도
    public float nowAngle;             // 현재 행성 위치

    // Update is called once per frame
    
    void FixedUpdate()
    {
        nowAngle = (nowAngle + speed) % (2* Mathf.PI);
        this.transform.position = new Vector3(this.rotateRound * Mathf.Cos(this.nowAngle), this.rotateRound * Mathf.Sin(this.nowAngle));
    }
    
    public void SetComponent(float r, float rotateRound, float speed)
    {
        this.r = r;

        this.transform.localScale = new Vector3(r, r, r);

        this.rotateRound = rotateRound;

        this.speed = speed / Mathf.Pow(rotateRound, 3 / 2f);

        this.nowAngle = Random.Range(0, Mathf.PI * 2);

        this.transform.position = new Vector3(this.rotateRound * Mathf.Cos(this.nowAngle), this.rotateRound * Mathf.Sin(this.nowAngle));
    }
}
