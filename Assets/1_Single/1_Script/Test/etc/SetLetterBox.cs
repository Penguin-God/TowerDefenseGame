using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLetterBox : MonoBehaviour
{
    [SerializeField] float delayTime;
    [SerializeField] Color color = new Color(0, 0, 0, 1);
    private void Start()
    {
        GetComponent<Image>().color = color;
        StartCoroutine(Co_SetBox());
    }

    IEnumerator Co_SetBox()
    {
        yield return new WaitForSeconds(delayTime);

        gameObject.SetActive(false);
    }
}
