using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPopupOverlay : MonoBehaviour
{

    public RandomPopup randomPopup;

    private void StartStretch(int frames)
    {
        randomPopup.Stretch(frames);
    }
    private void EndStretch()
    {
        randomPopup.EndStretch();
    }

    private void StartShrink(int frames)
    {
        randomPopup.Shrink(frames);
    }
    private void EndShrink()
    {
        randomPopup.EndShrink();
    }

    private void FinishAnimate()
    {
        randomPopup.FinishAnimate();
    }

    private void QuickEnded()
    {
        randomPopup.QuickEnded();
    }

}
