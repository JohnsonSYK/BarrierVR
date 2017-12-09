﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed = 1.0f;
    public int Damage = 3;
    public AlertPosition ap;
    public GameObject BlockedSound;
    public GameObject HitSound;
    private GameObject BlowUpParticles;
    public AudioClip HapticBlock;

    private OVRHapticsClip hapticClip;
    //public AttackAlert AA;

    void Start()
    {
        BlowUpParticles = (GameObject)Resources.Load("blowUp_effect");
        ap = GameObject.Find("OVRPlayerController").GetComponent<AlertPosition>();

        Vector3 target = GameObject.Find("CenterEyeAnchor").transform.position;
        target.y -= 0.1f;
        this.transform.rotation = Quaternion.LookRotation(target - this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        Vector3 p = this.transform.position;
        if (p.x > 15 || p.x < -15 || p.y > 10 || p.y < -2 || p.z > 10 || p.z < -12)
        {
            Object.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        if (other.tag == "Shield")
        {
            var blowup = Instantiate(BlowUpParticles, transform.position, transform.rotation);
            Destroy(blowup, 1);
            if (BlockedSound != null)
                Instantiate(BlockedSound, transform.position, transform.rotation);
            hapticClip = new OVRHapticsClip(HapticBlock);

            int n = 1;
            if (other.transform.name == "LeftShield" || other.transform.parent.name == "LeftShield")
            {
                n = 0;
            }
            OVRHaptics.Channels[n].Mix(hapticClip);

            Object.Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            other.GetComponent<PlayerInfo>().Damage(Damage);
            if (HitSound != null)
                Instantiate(HitSound, transform.position, transform.rotation);
            Object.Destroy(this.gameObject);
        }
    }
}

