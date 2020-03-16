using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviourPun
{
    public List<Gun> loadout;

    public Transform weaponParent;

    public float WeaponThrowPower = 1f;


    GameObject curentWeapon;


    private int currentIndWeapon;

    float distAim;
    public Vector2 limitsOffset = new Vector2(-0.2f, 0.2f);
    Vector3 distanceInAim = Vector3.forward;
    float distDer = 0.065f;
    float scroll;
    bool isAiming;

    Vector3 weaponOriginLPosition;
    Quaternion weaponOriginLRotation;
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Equip(0);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropWeapon();
        }
        if (isAiming = Input.GetMouseButton(1))
        {
            if ((scroll = Input.mouseScrollDelta.y) != 0)
            {
                if (scroll > 0)
                {
                    if (distAim < limitsOffset.y)
                    {
                        distAim += distDer;
                    }
                    
                }
                else
                {
                    if (distAim > limitsOffset.x)
                    {
                        distAim -= distDer;
                    }

                }
            }

            SetAim(curentWeapon, distanceInAim * distAim);
        }
        else
        {
            SetHip(curentWeapon);
        }
        if (Input.GetMouseButtonDown(0))
        {
            WeaponShoot();
        }

        kickBack();
    }

    private void WeaponShoot()
    {
        if (currentIndWeapon != int.MaxValue && curentWeapon)
        {
            loadout[currentIndWeapon].Shoot(curentWeapon, isAiming);
            ApplyRecoil();
        }
    }

    void ApplyRecoil()
    {
        Vector3 recV;
        if (isAiming)
        {
            Vector3 dirRecoil = 
                UnityEngine.Random.insideUnitCircle * loadout[currentIndWeapon].recoilRandomDirectionRadiusAim;
            dirRecoil += Vector3.forward;

            curentWeapon.transform.Rotate(loadout[currentIndWeapon].recoilRotateInAim, 0f, 0f);
            recV = Vector3.Lerp
            (curentWeapon.transform.localPosition,
            curentWeapon.transform.localPosition - (dirRecoil * loadout[currentIndWeapon].recoilAim),
            Time.deltaTime * loadout[currentIndWeapon].recoilSpeed
            );
        }
        else
        {
            Vector3 dirRecoil =
                  UnityEngine.Random.insideUnitCircle * loadout[currentIndWeapon].recoilRandomDirectionRadius;
            dirRecoil += Vector3.forward;
            
            recV = Vector3.Lerp
            (curentWeapon.transform.localPosition,
            curentWeapon.transform.localPosition - (dirRecoil * loadout[currentIndWeapon].recoil),
            Time.deltaTime * loadout[currentIndWeapon].recoilSpeed
            );

            curentWeapon.transform.Rotate(loadout[currentIndWeapon].recoilRotate, 0f, 0f);

        }

        curentWeapon.transform.localPosition = recV;

    }

    void kickBack()
    {
        if (currentIndWeapon != int.MaxValue && curentWeapon)
        {
            curentWeapon.transform.localPosition =
          Vector3.Lerp
          (curentWeapon.transform.localPosition,
          weaponOriginLPosition,
          Time.deltaTime * loadout[currentIndWeapon].kickbackSpeed
          );
        }
       
    }

    private void SetAim(GameObject weapon, Vector3 dist)
    {
        if (currentIndWeapon == int.MaxValue || !weapon)
        {
            return;
        }
        loadout[currentIndWeapon].setAnchorAim(weapon, dist);
        
    }
    private void SetHip(GameObject weapon)
    {
        if (currentIndWeapon == int.MaxValue || !weapon)
        {
            return;
        }
        loadout[currentIndWeapon].setAnchorHip(weapon);

    }

    private void DropWeapon()
    {
        if (currentIndWeapon == int.MaxValue)
        {
            return;
        }
       GameObject weapon = curentWeapon;

       bool res = weapon.TryGetComponent<Rigidbody>(out Rigidbody comp);

        Vector3 throwDir = weapon.transform.forward;

        if (res)
        {
            comp.AddForce
            (throwDir.normalized * WeaponThrowPower, ForceMode.Impulse);
        }
        else
        {
            weapon.AddComponent<Rigidbody>().AddForce
            (throwDir.normalized * WeaponThrowPower, ForceMode.Impulse);
        }

        curentWeapon= null;
        currentIndWeapon = int.MaxValue;
        weapon.transform.SetParent(null);
        
    }

    void Equip(int eq_slot)
    {
        if (curentWeapon == null)
        {
            if (loadout[eq_slot])
            {
                GameObject equip =
          Instantiate
          (
              loadout[eq_slot].prefab,
              weaponParent
              );

                currentIndWeapon = eq_slot;
                curentWeapon = equip;
                weaponOriginLPosition = curentWeapon.transform.localPosition;
                weaponOriginLRotation = curentWeapon.transform.localRotation;
            }

        }
        else
        {
            if (curentWeapon.activeSelf)
            {
                // Спрятать оружие
                curentWeapon.SetActive(false);
                currentIndWeapon = int.MaxValue;
            }
            else
            {
                // Достать оружие
                curentWeapon.SetActive(true);
                currentIndWeapon = eq_slot;
            }
           
        }


    }
}
