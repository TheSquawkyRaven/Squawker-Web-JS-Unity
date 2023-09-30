using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomPopup : MonoBehaviour
{

    public RectTransform posTransform;
    public RectTransform overlayTransform;  //Stretch Width then height
    public TextMeshProUGUI displayText;

    public RectTransform testerTextTransform;
    public TextMeshProUGUI testerText;
    public CanvasGroup canvasGroup;

    public Animator animator;

    public float maximumWidth;
    public float startingWidth;

    public float minimumHeight; //Minimum width = minimum height, minimum width always same as height
    public float maximumHeight;

    public Vector4 containingAddingOffset;
    public float textMeshMarginWidth;
    public float textMeshMarginHeight;

    private float intendedWidth;
    private float intendedHeight;

    private bool isStretching = false;
    private bool isShrinking = false;
    private float frames = 0;

    private float currentFrame = 0;

    private string targetText;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isStretching)
        {
            overlayTransform.sizeDelta = new Vector2(startingWidth + (intendedWidth - startingWidth) * (currentFrame / frames), intendedHeight);

            if (currentFrame == frames)
            {
                isStretching = false;
                return;
            }

            currentFrame++;

            return;
        }
        if (isShrinking)
        {
            overlayTransform.sizeDelta = new Vector2(startingWidth + (intendedWidth - startingWidth) * (1 - currentFrame / frames), intendedHeight);

            if (currentFrame == frames)
            {
                isShrinking = false;
                return;
            }

            currentFrame++;

            return;
        }

    }

    private void OnDisable()
    {
        overlayTransform.sizeDelta = new Vector2(startingWidth, minimumHeight);
        canvasGroup.alpha = 0;
    }


    public void NewRandomPopup(string text)
    {
        targetText = text;
        if (gameObject.activeSelf)
        {
            animator.SetTrigger("QuickEnd");
            return;
        }

        CalculateWidth();
        overlayTransform.sizeDelta = new Vector2(startingWidth, intendedHeight);
        gameObject.SetActive(true);
    }
    private void ExecuteQuickEndPopup()
    {
        CalculateWidth();
        overlayTransform.sizeDelta = new Vector2(startingWidth, intendedHeight);
        gameObject.SetActive(true);
    }

    public void ClearRandomPopup()
    {
        animator.SetTrigger("End");
    }
    public void FinishAnimate()
    {
        gameObject.SetActive(false);
    }

    public void QuickEnded()
    {
        gameObject.SetActive(false);
        ExecuteQuickEndPopup();
    }

    public void Shrink(int frames)
    {
        isShrinking = true;
        this.frames = frames;
        currentFrame = 0;
    }

    public void EndShrink()
    {
        overlayTransform.sizeDelta = new Vector2(startingWidth, intendedHeight);
        isShrinking = false;
    }

    public void Stretch(int frames)
    {
        isStretching = true;
        this.frames = frames;
        currentFrame = 0;

        overlayTransform.sizeDelta = new Vector2(startingWidth, intendedHeight);
        displayText.SetText(targetText);
    }

    public void EndStretch()
    {
        overlayTransform.sizeDelta = new Vector2(intendedWidth, intendedHeight);
        isStretching = false;
    }

    private void CalculateWidth()
    {
        testerText.text = targetText;
        testerTextTransform.sizeDelta = new Vector2(maximumWidth, maximumHeight);

        testerText.ForceMeshUpdate();
        Bounds b = testerText.bounds;
        Vector2 size = b.size;
        float width = size.x;
        float height = size.y;

        //Debug.Log(size);

        width += textMeshMarginWidth + containingAddingOffset.x + containingAddingOffset.z;
        height += textMeshMarginHeight + containingAddingOffset.y + containingAddingOffset.w;
        if (height < minimumHeight)
        {
            height = minimumHeight;
        }
        if (width < height)
        {
            width = height;
        }
        intendedWidth = width;
        intendedHeight = height;

    }

}
