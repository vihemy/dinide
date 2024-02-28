using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float rotationSpeed = 30;
    private void OnEnable()
    {
        RotateGameObject(rotationSpeed);
    }
    public void RotateGameObject(float speed)
    {
        StartCoroutine(RotateCoroutine(speed));
    }

    private IEnumerator RotateCoroutine(float speed)
    {
        while (true)
        {
            transform.Rotate(Vector3.back * speed * Time.deltaTime);
            yield return null;
        }
    }

}
