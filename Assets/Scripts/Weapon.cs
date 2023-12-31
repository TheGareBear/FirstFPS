﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ObjectPool bulletPool;
    public Transform muzzle;            // spawn pos for the bullet

    public int curAmmo;                 // current amount of ammo
    public int maxAmmo;                 // maximum amount of ammo we can get
    public bool infiniteAmmo;           // do we have infinite ammo?

    public float bulletSpeed;           // initial velocity of the bullet

    public float shootRate;             // min time between shots
    private float lastShootTime;        // last time we shot a bullet
    private bool isPlayer;              // are we the player's weapon?

    public AudioClip shootSfx;
    private AudioSource audioSource;

    void Awake ()
    {
        // are we attached to the player?
        if(GetComponent<Player>())
            isPlayer = true;

        audioSource = GetComponent<AudioSource>();
    }

    // can we shoot a bullet?
    public bool CanShoot ()
    {
        if(Time.time - lastShootTime >= shootRate)
        {
            if(curAmmo > 0 || infiniteAmmo == true)
                return true;
        }

        return false;
    }

    // called when we want to shoot a bullet
    public void Shoot ()
    {
        lastShootTime = Time.time;
        curAmmo--;

        if(isPlayer)
            GameUI.instance.UpdateAmmoText(curAmmo, maxAmmo);

        audioSource.PlayOneShot(shootSfx);

        GameObject bullet = bulletPool.GetObject();

        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = muzzle.rotation;

        // set the velocity
        bullet.GetComponent<Rigidbody>().velocity = muzzle.forward * bulletSpeed;
    }
}