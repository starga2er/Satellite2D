using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class SatelliteAgent : Agent
{
    public float speed;

    public float gravityScale;
    public bool isAlive;

    Rigidbody2D rb;
    GameObject[] stars;
    // Start is called before the first frame update

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        EnvironmentSetting.GetInstance().SettingEnv(); // 주위를 도는 행성을 만듬

        rb.velocity = Vector2.zero; // 인공위성의 속도 0으로 설정

        stars = GameObject.FindGameObjectsWithTag("Star");

        this.transform.position = new Vector2(7f, 7f); // 인공위성의 위치는 항상 7,7로 고정
        
        isAlive = true; // 인공 위성이 살아있다고 표시
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
        foreach (var i in GameObject.FindGameObjectsWithTag("Star")) // 별의 갯수 * 3 의 외부요인 (별과 인공위성의 상대적 위치와 별의 크기)
        {
            sensor.AddObservation(i.transform.localPosition - this.transform.localPosition);
            sensor.AddObservation(i.GetComponent<Star>().r);
        }
        
        sensor.AddObservation(rb.velocity); // 인공위성의 속도

        // 총합 별의 갯수 * 3 + 2의 외부 요인
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // 2개의 output을 받음
        var actionY = Mathf.Clamp(vectorAction[0], -1f, 1f);
        var actionX = Mathf.Clamp(vectorAction[1], -1f, 1f);
        
        Vector2 move = new Vector2(actionX, actionY);
        
        rb.AddForce(move.normalized * speed);
        
        stars = GameObject.FindGameObjectsWithTag("Star"); 
        foreach (var i in stars) // 별의 중력 크기 계산 및 반영
        {
            Star iStar = i.GetComponent<Star>();
            float length = Vector2.Distance(this.transform.position, i.transform.position);
            Vector2 objGravityPos = new Vector2(i.transform.position.x - this.transform.position.x, i.transform.position.y - this.transform.position.y).normalized;
            rb.AddForce(gravityScale * objGravityPos * Mathf.Pow(iStar.r, 3) / Mathf.Pow(length, 2));
        }

        if (isAlive)
        {
            SetReward(1f); // 1 step당 살아있으면 1 reward

        } else
        {
            stars = GameObject.FindGameObjectsWithTag("Star");

            foreach (var i in stars)
            {
                if (i.name != "Sun")
                    Destroy(i);
            }
            SetReward(-1000f);
            EndEpisode(); // 충돌시 종료 
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isAlive = false;
    }

}
