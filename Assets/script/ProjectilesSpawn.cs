using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesSpawn : MonoBehaviour
{
    public GameObject center;
    public GameObject projectile_1;
    public GameObject projectile_2;
    public GameObject projectile_3;
    [Space]
    [SerializeField] private float distance = 25f;

    private Vector3 _centerOfMap;
    private int _nbProjectile = 0;
    private float _projectileSpeed;
    private float _XPos;
    private float _YPos;
    private Vector3 _projectileDirection;
    private float _projectileViewDirection;

    void Start()
    {
        _centerOfMap = center.transform.position;
    }

    public void SpawnProjectile()
    {
        _projectileSpeed = Random.Range(5f, 20f);
        int switchCase = Random.Range(0, 3);
        float randomAngle = Random.Range(0f, (float)2 * Mathf.PI);
        _XPos = Mathf.Cos(randomAngle)* distance + _centerOfMap.x;
        _YPos = -Mathf.Sin(randomAngle) * distance + _centerOfMap.z;
        Vector3 spawnPos = new Vector3(_XPos, 1f, _YPos);
        _projectileViewDirection = Random.Range(-30f,30f);
        
        
        _nbProjectile++;
        GameObject go = null;

        switch (switchCase)
        {
            case 0:
                go = Instantiate(projectile_1, spawnPos, Quaternion.LookRotation(_centerOfMap - spawnPos));
                break;
            case 1:
                go = Instantiate(projectile_2, spawnPos, Quaternion.LookRotation(_centerOfMap - spawnPos));
                break;
            case 2:
                go = Instantiate(projectile_3, spawnPos, Quaternion.LookRotation(_centerOfMap - spawnPos));
                break;
        }
        go.transform.Rotate(new Vector3(0f, _projectileViewDirection, 0f));
        Destroy(go, 15f);
    }
}
