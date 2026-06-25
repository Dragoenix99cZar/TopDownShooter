using UnityEngine;

[CreateAssetMenu(fileName = "GunConfig", menuName = "Scriptable Objects/GunConfig")]
public class GunConfigSO : ScriptableObject
{
    public float msBetweenShots = 100f;
    public float muzzleVelocty = 35f;
    public float lifeTime = 500f;

    public int poolSize = 20;
}
