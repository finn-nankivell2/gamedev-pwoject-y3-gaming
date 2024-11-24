using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public Transform respawnPosition;
    public GameObject sceneTransition;
    private Animator animator;

    void Start()
    {
        animator = sceneTransition.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(ResetPlayer(other));
        }
    }

    IEnumerator ResetPlayer(Collider player)
    {
        animator.Play("SlideIn");
        yield return new WaitForSeconds(0.8f);
        player.transform.position = respawnPosition.position;
        Physics.SyncTransforms();
        animator.Play("SlideOut");
    }
}
