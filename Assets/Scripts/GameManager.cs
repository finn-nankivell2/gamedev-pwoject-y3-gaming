using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public Transform respawnPosition;
    public GameObject sceneTransition;
    private static Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = sceneTransition.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator ResetPlayer(Collider player)
    {
        animator.Play("SlideIn");
        yield return new WaitForSeconds(0.8f);
        player.transform.position = respawnPosition.position;
        Physics.SyncTransforms();
        animator.Play("SlideOut");
    }
}