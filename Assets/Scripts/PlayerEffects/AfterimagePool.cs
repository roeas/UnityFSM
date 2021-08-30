using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimagePool : MonoBehaviour
{
    public static AfterimagePool instance;
    public GameObject imagePrefab;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        FillPoll(10);
    }
    public void FillPoll(int num) {
        for (int i = 0; i < num; i++) {
            GameObject newImage = Instantiate(imagePrefab);
            newImage.transform.SetParent(transform);
            ReturnToPool(newImage);
        }
    }
    public void ReturnToPool(GameObject objectIn) {//在对象自身脚本中调用
        objectIn.SetActive(false);
        pool.Enqueue(objectIn);
    }
    public void TakeFromPool() {//Player为冲刺状态时在FixedUpdate中反复调用
        if (pool.Count <= 0) {//对象池不够用时多生成几个预制件
            FillPoll(5);
        }
        GameObject crtImage = pool.Dequeue();
        crtImage.SetActive(true);
    }
}
