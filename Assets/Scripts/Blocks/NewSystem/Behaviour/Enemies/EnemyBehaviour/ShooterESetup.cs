using UnityEngine;

public class ShooterESetup : MonoBehaviour, IBlockSetup
{
    [SerializeField] ShooterEBehaviour shooterEBehaviour;

    public void SetupVariant(string _variant, MapContext _context)
    {
        Debug.Log(_variant);
        switch (_variant)
        {
            case "RAD":
                shooterEBehaviour.SetUpShooter(ShootingMode.Radial, ShootingDirection.None);
                break;
            case "DIRF":
                shooterEBehaviour.SetUpShooter(ShootingMode.Directional, ShootingDirection.Front);
                break;
            case "DIRB":
                shooterEBehaviour.SetUpShooter(ShootingMode.Directional, ShootingDirection.Back);
                break;
            case "DIRR":
                shooterEBehaviour.SetUpShooter(ShootingMode.Directional, ShootingDirection.Right);
                break;
            case "DIRL":
                shooterEBehaviour.SetUpShooter(ShootingMode.Directional, ShootingDirection.Left);
                break;
        }
                
    }

}
