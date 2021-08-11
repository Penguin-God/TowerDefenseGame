using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    [SerializeField] GameObject paint;

    public void SetPaint()
    {
        if (paint.activeSelf) paint.SetActive(false);
        else paint.SetActive(true);
    }

    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void PlayClip()
    {
        audioSource.Play();
    }
}
