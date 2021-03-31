using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimagePool : MonoBehaviour
{
    public static AfterimagePool instance;
    public GameObject imagePrefab;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private void Awake() {
        instance = this;
        FillPoll(10);
    }
    public void FillPoll(int num) {
        for (int i = 0; i < num; i++) {
            GameObject newImage = Instantiate(imagePrefab);
            newImage.transform.SetParent(transform);
            ReturnToPool(newImage);
        }
    }
    public void ReturnToPool(GameObject objectIn) {
        objectIn.SetActive(false);
        pool.Enqueue(objectIn);
    }
    public GameObject TakeFromPool() {
        if (pool.Count <= 0) {
            FillPoll(5);
        }
        GameObject activeimage = pool.Dequeue();
        activeimage.SetActive(true);
        return activeimage;
    }
}
