using UnityEngine;
using System.Collections;

public class TutorialArrows : MonoBehaviour
{
    GameObject[] Arrows;


    void Start()
    {
        Arrows = GameObject.FindGameObjectsWithTag("Arrow");
        Arrows[0].gameObject.SetActive(false);
        StartCoroutine(ShowReady(1));
    }

    IEnumerator ShowReady(int ArrowCount)
    {
        int count = 0;
        if (ArrowCount == 1)
        {
            while (count < 100)
            {
                Arrows[0].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                Arrows[0].gameObject.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                count++;
            }
        }
        //else if (ArrowCount == 2)
        //{
        //    while (count < 100)
        //    {
        //        Arrows[0].gameObject.SetActive(true);
        //        Arrows[1].gameObject.SetActive(true);
        //        yield return new WaitForSeconds(0.5f);
        //        Arrows[0].gameObject.SetActive(false);
        //        Arrows[1].gameObject.SetActive(false);
        //        yield return new WaitForSeconds(0.5f);
        //        count++;
        //    }
        //}

        //while (count < 100)
        //{
        //    Arrows.gameObject.SetActive(true);
        //    yield return new WaitForSeconds(0.5f);
        //    Arrows.gameObject.SetActive(false);
        //    yield return new WaitForSeconds(0.5f);
        //    count++;
        //}
    }

    public void ArrowStart(int ArrowCount)
    {
        Arrows = GameObject.FindGameObjectsWithTag("Arrow");
        StopAllCoroutines();
        StartCoroutine(ShowReady(ArrowCount));
    }

    public void ArrowStop(int ArrowCount)
    {
        StopCoroutine(ShowReady(ArrowCount));
    }
}
