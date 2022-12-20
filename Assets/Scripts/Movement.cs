using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrust = 10;
    [SerializeField] float rotationAngle = 90;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainRocketParticles;
    [SerializeField] ParticleSystem rightSideParticles;
    [SerializeField] ParticleSystem leftSideParticles;

    Rigidbody rb;
    AudioSource audioSource;

    CollisionHandler collisionHandler;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        collisionHandler = GetComponent<CollisionHandler>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        NextLevelCheat();
        CollisionCheat();
    }

    void CollisionCheat()
    {
        if (Input.GetKey(KeyCode.C))
        {
            collisionHandler.collisionDisableCheat =
            !collisionHandler.collisionDisableCheat;
        }
        
    }

    void NextLevelCheat()
    {
        if (Input.GetKey(KeyCode.L))
        {
            collisionHandler.StartNextLevelSequence();
        }
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainRocketParticles.isPlaying)
        {
            mainRocketParticles.Play();
        }
    }
    void StopThrusting()
    {
        audioSource.Stop();
        mainRocketParticles.Stop();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            StartRotation(rotationAngle,rightSideParticles);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            StartRotation(-rotationAngle, leftSideParticles);
        }
        else
        {
            StopRotating();
        }
    }

    void StartRotation(float rotAngle,ParticleSystem thrustSide)
    {
        ApplyRotation(rotAngle);
        if (!thrustSide.isPlaying)
        {
            thrustSide.Play();
        }
    }
    void StopRotating()
    {
        rightSideParticles.Stop();
        leftSideParticles.Stop();
    }
    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freeze Rot so we can manually rotate
        transform.Rotate(Vector3.forward, rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //Unfreeze the Rot so Physics system can take over
    }
}
