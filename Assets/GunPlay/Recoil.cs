
using UnityEngine;

public class Recoil : MonoBehaviour
{

    private AimDownSights ads;

    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [Header("HipFire Recoil")]
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    [Header("Aim Recoil")]
    [SerializeField] private float adsRecoilX;
    [SerializeField] private float adsRecoilY;
    [SerializeField] private float adsRecoilZ;
    [SerializeField] private float adsSnappiness;
    [SerializeField] private float adsReturnSpeed;

    public GameObject recoilCam;


    


    void Start()
    {
        

        ads = GetComponent<AimDownSights>();
    }

    
    void Update()
    {
       
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        recoilCam.transform.localRotation = Quaternion.Euler(currentRotation);

    }

    public void RecoilFire()
    {
        

        if (ads.aiming)
        {
             targetRotation += new Vector3(adsRecoilX, Random.Range(-adsRecoilY, adsRecoilY), Random.Range(-adsRecoilZ, adsRecoilZ));
        }
        else
        {
            targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        }
        
    }
}
