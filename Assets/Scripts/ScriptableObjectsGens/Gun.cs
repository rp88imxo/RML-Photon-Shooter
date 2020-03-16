using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string WeaponName;
    public float fireRate;
    public float maxShootDistance = 50f;
    public float aimSpeed;

    public bool EnableSpread = true;
    // Spreading in units at this distance
    public float DistanceSpread = 1000f;
    // Spreading is random pos on circle with R = spreading
    public float spreading;
    // Spreading is random pos on circle with R = spreading WHILE Aim
    public float AimSpreading;

    public float recoil;
    public float recoilAim;
    public float recoilRotate;
    public float recoilRotateInAim;
    public float recoilSpeed;
    public float recoilRandomDirectionRadius;
    public float recoilRandomDirectionRadiusAim;
    public float kickback;
    public float kickbackSpeed;

    public GameObject prefab;

    public GameObject bulletHole;
    public int MaxAmountBulletsHoles = 500;


    List<GameObject> bHoles;
    Queue<GameObject> bHolesQ = new Queue<GameObject>();

    Transform HIPAnch;
    Transform AimAnch;

    Ray rayHit;

    public  void setAnchor(WeaponAnchors anchor, GameObject prefab, Vector3 dist = default)
    {
        switch (anchor)
        {
            case WeaponAnchors.Hip:
                setAnchorHip(prefab);
                break;
            case WeaponAnchors.Aim:
                setAnchorAim(prefab, dist);
                break;
            default:
                setAnchorHip(prefab);
                break;
        }
    }

    public void setAnchorAim(GameObject prefab, Vector3 dist)
    {
        Transform getMainAnchor = prefab.transform.Find("Anchor");
        if (!getMainAnchor)
        {
            return;
        }

        Transform aimAnchor = prefab.transform.Find("States").Find("ADS");
        
        getMainAnchor.transform.localPosition =
            Vector3.Lerp(getMainAnchor.transform.localPosition, 
            aimAnchor.transform.localPosition + dist, 
            aimSpeed * Time.deltaTime);
    }

    public void setAnchorHip(GameObject prefab)
    {
        Transform getMainAnchor = prefab.transform.Find("Anchor");
        if (!getMainAnchor)
        {
            return;
        }

        Transform hipAnchor = prefab.transform.Find("States").Find("Hip");
        getMainAnchor.transform.position = 
            Vector3.Lerp(getMainAnchor.transform.position, hipAnchor.transform.position, aimSpeed * Time.deltaTime * 2f);
    }

    public Transform GetShootPoint(GameObject prefab)
    {
        Transform sPoint = prefab.transform.Find("Anchor/Resources/ShootPoint");

        return sPoint;
    }
    
    public void Shoot(GameObject prefab, bool isAim)
    {
        Transform sPoint = GetShootPoint(prefab);

        if (!sPoint || !bulletHole)
        {
            if (!sPoint)
            {
                Debug.Assert(false, "No ShootPoint On Weapon!");
            }
            else
            {
                Debug.Assert(false, "Bullet Hole Prefab Is NULL!");
            }
            return;
        }

        #region WEAPON_SPREAD
        Vector3 rndV = Vector3.zero;
        if (EnableSpread)
        {
            if (isAim)
            {
                rndV = UnityEngine.Random.insideUnitCircle * AimSpreading;
            }
            else
            {
                rndV = UnityEngine.Random.insideUnitCircle * spreading;
            }
        }


        //Spreading in meters at DistanceSpread units away
        rndV += Vector3.forward * DistanceSpread;
        #endregion

        rayHit.origin = sPoint.transform.position;
        rayHit.direction = sPoint.transform.TransformDirection(rndV);

        if (Physics.Raycast(rayHit, out RaycastHit hitInfo, maxShootDistance))
        {
            GameObject bulletH = 
                Instantiate<GameObject>
                (
                    bulletHole,
                    hitInfo.point,
                    Quaternion.LookRotation(hitInfo.normal)
                );

            bulletH.transform.position = hitInfo.point + (bulletH.transform.forward * 0.001f);
            bulletH.transform.SetParent(hitInfo.transform);

           // bulletH.transform.localRotation = Quaternion.LookRotation(hitInfo.normal);
           // bulletH.transform.localScale = bulletHole.transform.localScale;

            // bulletH.transform.localRotation = Quaternion.FromToRotation(hitInfo.point, hitInfo.normal);

            if (bHolesQ.Count >= MaxAmountBulletsHoles)
            {
               
               GameObject gameObject = bHolesQ.Dequeue();
                Destroy(gameObject.gameObject);
            }

            bHolesQ.Enqueue(bulletH);

        }

    }
}
