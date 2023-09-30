using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rotater : MonoBehaviour
{
    public class Container
    {
        [System.NonSerialized] public float movementSpeed;

        //[System.NonSerialized] public float xRadius = 1;
        [System.NonSerialized] public float xRadius = 1;
        [System.NonSerialized] public float zRadius = 1;

        [System.NonSerialized] public float rotationSpeed;

        public Container(float movementSpeed, float xRadius, float zRadius, float rotationSpeed)
        {
            this.movementSpeed = movementSpeed;
            //this.xRadius = xRadius;
            this.xRadius = xRadius;
            this.zRadius = zRadius;
            this.rotationSpeed = rotationSpeed;
        }
    }

    public Transform thisTransform;
    public Transform childTransform;

    public TextMeshPro textText;
    public string text;

    [System.NonSerialized] public Container container;

    [System.NonSerialized] public float poppingTextSpeed;
    [System.NonSerialized] public bool usePoppingText;

    [System.NonSerialized] public float timeToDestroy = 10f;
    private float timeCount = 0;
    private float popTimeCount = 0;
    private int textPlaced;
    private void Start()
    {
        if (usePoppingText)
        {
            textPlaced = 0;
        }
        else
        {
            textText.SetText(text);
        }

    }

    private void Update()
    {
        if (!(usePoppingText && textPlaced < text.Length))
        {
            thisTransform.localPosition -= thisTransform.right * container.movementSpeed * Time.deltaTime;
        }

        timeCount += Time.deltaTime;
        if (timeCount > timeToDestroy)
        {
            Destroy(gameObject);
        }
        if (usePoppingText)
        {
            popTimeCount += Time.deltaTime;
            if (popTimeCount > poppingTextSpeed)
            {
                popTimeCount = 0;
                if (textPlaced < text.Length)
                {
                    textText.SetText($"{textText.text}{text[textPlaced]}");
                    textPlaced++;
                }
            }
        }
    }
}
