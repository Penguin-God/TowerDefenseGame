using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintScript : MonoBehaviour
{
    public Image PaintButton;
    public Image SwordmanImage;
    public Image ArcherImage;
    public Image SpearmanImage;
    public Image MageImage;
    public Image SwordmanTextBackground;
    public Image ArcherTextBackground;
    public Image SpearmanTextBackground;
    public Image MageTextBackground;
    




    public void ClickRedPaintButton()
   {
        PaintButton.color = Color.red;
        SwordmanImage.color = Color.red;
        ArcherImage.color = Color.red;
        SpearmanImage.color = Color.red;
        MageImage.color = Color.red;
        SwordmanTextBackground.color = Color.red;
        ArcherTextBackground.color = Color.red;
        SpearmanTextBackground.color = Color.red;
        MageTextBackground.color = Color.red;
    }

    public void ClickBluePaintButton()
    {
        PaintButton.color = Color.blue;
        SwordmanImage.color = Color.blue;
        ArcherImage.color = Color.blue;
        SpearmanImage.color = Color.blue;
        MageImage.color = Color.blue;
        SwordmanTextBackground.color = Color.blue;
        ArcherTextBackground.color = Color.blue;
        SpearmanTextBackground.color = Color.blue;
        MageTextBackground.color = Color.blue;
    }

    public void ClickYellowPaintButton()
    {
        PaintButton.color = new Color32(230, 230, 0, 255);
        SwordmanImage.color = new Color32(230, 230, 0, 255);
        ArcherImage.color = new Color32(230, 230, 0, 255);
        SpearmanImage.color = new Color32(230, 230, 0, 255);
        MageImage.color = new Color32(230, 230, 0, 255);
        SwordmanTextBackground.color = new Color32(230, 230, 0, 255);
        ArcherTextBackground.color = new Color32(230, 230, 0, 255);
        SpearmanTextBackground.color = new Color32(230, 230, 0, 255);
        MageTextBackground.color = new Color32(230, 230, 0, 255);
    }

    public void ClickGreenPaintButton()
    {
        PaintButton.color = Color.green;
        SwordmanImage.color = Color.green;
        ArcherImage.color = Color.green;
        SpearmanImage.color = Color.green;
        MageImage.color = Color.green;
        SwordmanTextBackground.color = Color.green;
        ArcherTextBackground.color = Color.green;
        SpearmanTextBackground.color = Color.green;
        MageTextBackground.color = Color.green;
    }

    public void ClickOrangePaintButton()
    {
        PaintButton.color = new Color32(255, 80, 0, 255);
        SwordmanImage.color = new Color32(255, 80, 0, 255);
        ArcherImage.color = new Color32(255, 80, 0, 255);
        SpearmanImage.color = new Color32(255, 80, 0, 255);
        MageImage.color = new Color32(255, 80, 0, 255);
        SwordmanTextBackground.color = new Color32(255, 80, 0, 255);
        ArcherTextBackground.color = new Color32(255, 80, 0, 255);
        SpearmanTextBackground.color = new Color32(255, 80, 0, 255);
        MageTextBackground.color = new Color32(255, 80, 0, 255);
    }

    public void ClickVioletPaintButton()
    {
        PaintButton.color = new Color32(200, 0, 255, 255);
        SwordmanImage.color = new Color32(200, 0, 255, 255);
        ArcherImage.color = new Color32(200, 0, 255, 255);
        SpearmanImage.color = new Color32(200, 0, 255, 255);
        MageImage.color = new Color32(200, 0, 255, 255);
        SwordmanTextBackground.color = new Color32(200, 0, 255, 255);
        ArcherTextBackground.color = new Color32(200, 0, 255, 255);
        SpearmanTextBackground.color = new Color32(200, 0, 255, 255);
        MageTextBackground.color = new Color32(200, 0, 255, 255);
    }

}
