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
    private float _XPos;
    private float _YPos;
    private float _projectileViewDirection;

    void Start()
    {
        _centerOfMap = center.transform.position;
    }

    public void SpawnProjectile()
    {
        int whatPropsToSpawn = Random.Range(0, 3);
        float randomAngleToSpawn = Random.Range(0f, (float)2 * Mathf.PI);
        _XPos = Mathf.Cos(randomAngleToSpawn)* distance + _centerOfMap.x;
        _YPos = -Mathf.Sin(randomAngleToSpawn) * distance + _centerOfMap.z;
        Vector3 spawnPos = new Vector3(_XPos, 1f, _YPos);
        _projectileViewDirection = Random.Range(-30f,30f);
        GameObject projectile = null;

        switch (whatPropsToSpawn)
        {
            case 0:
                projectile = Instantiate(projectile_1, spawnPos, Quaternion.LookRotation(_centerOfMap - spawnPos));
                break;
            case 1:
                projectile = Instantiate(projectile_2, spawnPos, Quaternion.LookRotation(_centerOfMap - spawnPos));
                break;
            case 2:
                projectile = Instantiate(projectile_3, spawnPos, Quaternion.LookRotation(_centerOfMap - spawnPos));
                break;
        }
        projectile.transform.Rotate(new Vector3(0f, _projectileViewDirection, 0f));
    }
}
