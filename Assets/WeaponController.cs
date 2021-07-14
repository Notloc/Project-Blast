using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class WeaponController : MonoBehaviour
{
    [SerializeField] WeaponBehaviour weaponPrefab = null;
    [SerializeField] Transform weaponParent = null;
    
    [SerializeField] Rigidbody aimBody = null;
    [SerializeField] Rigidbody rotationBody = null;
    [SerializeField] Rigidbody movementBody = null;

    [SerializeField] Vector3 aimPositionOffset = Vector3.zero;
    [SerializeField] float aimSmoothing = 8f;
    [SerializeField] float velocityModSmoothing = 4f;

    [SerializeField] AnimationCurve movementAimDriftStrength = null;

    private WeaponInstance activeWeaponInstance;
    private WeaponBehaviour weaponBehaviour;

    private Transform cameraT;

    private Quaternion previousVelocityMod = Quaternion.identity;

    private void Start()
    {
        cameraT = Camera.main.transform;
    }

    public void EquipWeapon(WeaponInstance weaponInstance)
    {
        if (weaponBehaviour != null)
        {
            Destroy(weaponBehaviour.gameObject);
        }

        this.activeWeaponInstance = weaponInstance;
        weaponBehaviour = Instantiate(weaponPrefab, weaponParent);
        weaponBehaviour.SetWeaponInstance(activeWeaponInstance);
    }




    public void PullTrigger()
    {

    }

    public void ReleaseTrigger()
    {

    }

    private void Update()
    {
        UpdateAimPosition();
    }

    private void UpdateAimPosition()
    {
        aimBody.position = movementBody.position + (rotationBody.transform.rotation * aimPositionOffset);
    }

    private void FixedUpdate()
    {
        UpdateAimRotation();
    }

    private void UpdateAimRotation()
    {
        Quaternion targetRotation = Quaternion.Lerp(aimBody.rotation, cameraT.rotation, aimSmoothing * Time.fixedDeltaTime);


        Vector3 localVelocity = Quaternion.Inverse(rotationBody.rotation) * movementBody.velocity;
        Quaternion velocityMod = Quaternion.Euler(movementAimDriftStrength.Evaluate(localVelocity.y), -movementAimDriftStrength.Evaluate(localVelocity.x), 0f);


        previousVelocityMod = Quaternion.Lerp(previousVelocityMod, velocityMod, Time.fixedDeltaTime * velocityModSmoothing);


        aimBody.MoveRotation(targetRotation * velocityMod);
    }

}
