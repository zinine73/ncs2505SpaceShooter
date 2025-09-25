using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateDemo : MonoBehaviour
{
    // delegate 선언
    delegate float SumHandler(float a, float b);
    // delegate type 변수 선언
    SumHandler sumHandler;

    // 덧셈 연산
    float Sum(float a, float b)
    {
        return a + b;
    }

    void Start()
    {
        // delegate변수에 메서드 연결(할당)
        sumHandler = Sum;
        // delagate 실행
        float sum = sumHandler(10.0f, 5.0f);
        // 결과값 출력
        Debug.Log($"Sum = {sum}");
    }
}
