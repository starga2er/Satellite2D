using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSetting : MonoBehaviour
{
    private static EnvironmentSetting envSingleton = null;
    public int starNum;
    public float speed;
    public Sprite[] starSprites;
    public GameObject star;

    // Start is called before the first frame update
    void Awake()
    {
        if (envSingleton == null)
        {
            envSingleton = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static EnvironmentSetting GetInstance()
    {
        if (envSingleton == null)
        {
            return null;
        }
        return envSingleton;
    }

    public void SettingEnv() // 별을 생성하는 코드
    {
        int temp = starSprites.Length;

        for (int i = 0; i < starNum; i++)
        {
            GameObject gameObject = Instantiate(star);
            gameObject.GetComponent<SpriteRenderer>().sprite = starSprites[Random.Range(0, temp - 1)];
            gameObject.GetComponent<Star>().SetComponent(Random.Range(1f, 1.2f), 2f * i + 2, speed);
        }
    }
}
