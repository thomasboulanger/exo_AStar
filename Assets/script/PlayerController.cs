using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int maxRaycastDistance = 1000;

    [SerializeField]
    private LayerMask layerMaskForSolid; 

    [SerializeField]
    private string groundTag;

    private GameController gameController;

    private void Awake()
    {
        var gc = GameObject.FindWithTag("GameController");
        if (null == gc)
        {
            Debug.LogError("[PlayerController] GameController missing");
        }
        else
        {
            gameController = gc.GetComponent<GameController>();
        }
    }

    void Update()
    {
        // Mouse click
        if (Input.GetButtonDown("Fire1") && null != gameController)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxRaycastDistance, layerMaskForSolid))
            {
                Debug.Log("Raycasting : " + hit.point + " on " + hit.collider.gameObject.tag);
                if (hit.collider.gameObject.CompareTag(groundTag))
                {
                    gameController.MovePlayer(hit.point);
                    
                }
            }
        }

        if (moving)
        {
            if (transform.position != nextPointInPath && !gameController.isPaused)
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


    private void FixedUpdate()
    {
        Vector3 center = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        CollisionDetection(center, .5f);
    }
    public void CollisionDetection(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("chest"))
            {
                //score +100 et spawn new chest
                Debug.Log("coucou");
                //Destroy(hitCollider.gameObject);
                //gameController.isChestActive = false;
            }
            if (hitCollider.gameObject.CompareTag("projectile"))
            {
                //game over
                Debug.Log("u ded");
            }
        }
    }
    
}