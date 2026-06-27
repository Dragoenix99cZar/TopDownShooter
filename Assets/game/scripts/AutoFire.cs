using UnityEngine;

public class AutoFire : MonoBehaviour
{
    [SerializeField] int fireInterval = 60;
    [SerializeField] GunController gunController;

    int fireCounter = 0;

    private void Update()
    {
        fireCounter++;
        if(fireCounter % fireInterval == 0) gunController.Shoot();
    }
}
