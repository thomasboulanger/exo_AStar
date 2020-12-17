using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int maxRaycastDistance = 1000;
    [SerializeField] private LayerMask layerMaskForSolid;
    [SerializeField] private LayerMask collisionLayerMask; 
    [SerializeField] private string groundTag;

    private GameController gameController;
    private AudioController audioController;

    public static int score = 0;

    private void Awake()
    {
        var gc = GameObject.FindWithTag("GameController");
        var ac = GameObject.FindWithTag("AudioController");
        if (null == gc)
        {
            Debug.LogError("[PlayerController] GameController missing");
        }
        else
        {
            gameController = gc.GetComponent<GameController>();
        }
        if (null == ac)
        {
            Debug.LogError("[PlayerController] AudioController missing");
        }
        else
        {
            audioController = ac.GetComponent<AudioController>();
        }
    }

    private void Start()
    {
        score = 0;
    }

    void Update()
    {

        if (gameController == null || gameController.isPaused)
        {
            return;
        }
        // Mouse click
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxRaycastDistance, layerMaskForSolid))
            {
                //Debug.Log("Raycasting : " + hit.point + " on " + hit.collider.gameObject.tag);
                if (hit.collider.gameObject.CompareTag(groundTag))
                {
                    gameController.MovePlayer(hit.point);                  
                }
            }
        }

        if (moving)
        {
            if (transform.position != nextPointInPath && gameController.isPaused == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPointInPath, moveSpeed * Time.deltaTime);
            }
            else
            {
                moving = false;
            }
        }
        if (!moving && otherPointsInPath.Count > 0)
        {
            nextPointInPath = otherPointsInPath[0];
            otherPointsInPath.RemoveAt(0);
            moving = true;
        }

        Vector3 center = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        CollisionDetection(center, .5f);
    }

    [SerializeField]
    private float moveSpeed = 8;

    private bool moving = false;
    private Vector3 nextPointInPath;
    private List<Vector3> otherPointsInPath = new List<Vector3>();

    public void Move(List<Vector3> newPath)
    {
        otherPointsInPath.Clear();
        otherPointsInPath.AddRange(newPath);
    }

    public void CollisionDetection(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius,collisionLayerMask);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.enabled = false;
            if (hitCollider.gameObject.CompareTag("chest"))
            {
                //score +100 et spawn new chest
                score += 100;
                Destroy(hitCollider.gameObject);
                gameController.isChestActive = false;
                audioController.TouchChest();
            }
            if (hitCollider.gameObject.CompareTag("projectile"))
            {
                //game over
                gameController.gameOver = true;
                audioController.GetComponent<AudioSource>().Stop();
                audioController.DeathSound();
            }
        }
    }
    
}