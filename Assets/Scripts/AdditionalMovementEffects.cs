using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum State
{
    Iddle,
    Move,
    Run,
    Crouch,
    Crouching
}

public class AdditionalMovementEffects : MonoBehaviourPun
{
    public Transform Weapon;

    PlayerInput playerInput;
    PlayerController playerController;

    public float DefaultOffset = 0.01f;
    public float freqBobbing = 1f;
    public float smoothBlob = 1f;

    public float IddleOffset = 1f;
    public float MoveOffset = 2f;
    public float RunOffset = 3f;
    public float CrouchOffset = 0.5f;

    Vector3 defaultWeaponPos;
    Vector3 targetWeaponPos;

    State state;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<PlayerController>();

        defaultWeaponPos = Weapon.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;
        UpdateState();

        WeaponBobbing();
    }

    void UpdateState()
    {
       
        if (playerInput.crouch)
        {
            state = State.Crouch;
        }
        if (playerInput.crouching)
        {
            state = State.Crouching;
        }

        if (playerController.status == Status.idle)
        {
            state = State.Iddle;
        }

        if (playerController.status == Status.moving)
        {
            state = State.Move;
        }
        if (playerInput.run)
        {
            state = State.Run;
        }
    }

    void WeaponBobbing()
    {
        float modifiedOffset = DefaultOffset;
        float modifiedFreq = freqBobbing;



        switch (state)
        {
            case State.Iddle:
                modifiedOffset += IddleOffset;
                modifiedFreq = freqBobbing + 3f;
                break;
            case State.Move:
                modifiedOffset += MoveOffset;
                modifiedFreq = freqBobbing + 5f;
                break;
            case State.Run:
                modifiedOffset += RunOffset;
                modifiedFreq = freqBobbing + 20f;
                break;
            case State.Crouch:
                modifiedOffset += CrouchOffset;
                modifiedFreq = freqBobbing - 0.5f;
                break;
            case State.Crouching:
                modifiedOffset += CrouchOffset;
                modifiedFreq = freqBobbing - 0.5f;
                break;
            default:
                Debug.Assert(false, "Not Implemented state of Bobbing!");
                break;
        }

        targetWeaponPos = defaultWeaponPos + 
            new Vector3
            (
                (modifiedOffset * Mathf.Cos(Time.time * modifiedFreq)),
                (modifiedOffset * Mathf.Sin(Time.time * modifiedFreq)),
                (modifiedOffset * Mathf.Sin(Time.time * modifiedFreq))
            );

        Weapon.localPosition = Vector3.Lerp(Weapon.localPosition, targetWeaponPos, Time.deltaTime * smoothBlob);
            //new Vector3
            //(
            //Weapon.localPosition.x,
            //defaultWeaponPos.y + (heightIdle * Mathf.Sin(Time.time * freqBobbing)),
            //Weapon.localPosition.z
            //);
    }
}
