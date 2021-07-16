using UnityEngine;
using System.Collections;

public class TutorialArrows : MonoBehaviour
{
    public GameObject Arrows;



    // Use this for initialization
    void Start()
    {
        Arrows.gameObject.SetActive(false);
        StartCoroutine(ShowReady());
    }

    IEnumerator ShowReady()
    {
        int count = 0;
        while (count < 30)
        {
            Arrows.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Arrows.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            count++;
        }
    }
}
