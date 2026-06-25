using System;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform weaponHold;
    [SerializeField] private Gun startingGun;
    [SerializeField] private GunConfigSO gunConfig;

    private Gun equippedGun;

    private void Start() {
        if (startingGun != null) {
            EquipGun(startingGun);
        }
    }

    public void EquipGun(Gun gunToEquip) {
        if (equippedGun != null) {
            Destroy(equippedGun.gameObject);
        }

        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);
        equippedGun.transform.SetParent(weaponHold);
        equippedGun.ConfigureGunParameter(gunConfig);
    }

    public void Shoot()
    {
        if (equippedGun != null) {
            equippedGun.Shoot();
        }
    }
}
