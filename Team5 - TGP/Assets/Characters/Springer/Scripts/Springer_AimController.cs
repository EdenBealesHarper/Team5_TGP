using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AimData
{
    public GameObject Bone;
    public GameObject Object;
    public GameObject AnimatedCounterpart;
    public Vector3 Offset;
}


public class Springer_AimController : MonoBehaviour
{

    public AimData[] AimObjects = new AimData[2];


    private Vector3 MousePos;

    public bool bActiveWeapon = false;



    // Start is called before the first frame update
    void Start()
    {
        if (bActiveWeapon)
        {
            for (int i = 0; i < AimObjects.Length; i++)
            {
                AimObjects[i].Object.SetActive(true);
                AimObjects[i].AnimatedCounterpart.SetActive(false);

            }
        }
        else if (!bActiveWeapon)
        {
            for (int i = 0; i < AimObjects.Length; i++)
            {
                AimObjects[i].Object.SetActive(false);
                AimObjects[i].AnimatedCounterpart.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bActiveWeapon)
        {
            
            ApplyAimRotations();
        }
    }

    public void ToggleWeaponState()
    {
        bActiveWeapon = !bActiveWeapon;
        if (bActiveWeapon)
        {
            for (int i = 0; i < AimObjects.Length; i++)
            {
                AimObjects[i].Object.SetActive(true);
                AimObjects[i].AnimatedCounterpart.SetActive(false);

            }
        }
        else if (!bActiveWeapon)
        {
            for (int i = 0; i < AimObjects.Length; i++)
            {
                AimObjects[i].Object.SetActive(false);
                AimObjects[i].AnimatedCounterpart.SetActive(true);
            }
        }
    }
    public void ToggleWeaponState(bool bNewState)
    {
        bActiveWeapon = bNewState;
        if (bActiveWeapon)
        {
            for (int i = 0; i < AimObjects.Length; i++)
            {
                AimObjects[i].Object.SetActive(true);
                AimObjects[i].AnimatedCounterpart.SetActive(false);

            }
        }
        else if (!bActiveWeapon)
        {
            for (int i = 0; i < AimObjects.Length; i++)
            {
                AimObjects[i].Object.SetActive(false);
                AimObjects[i].AnimatedCounterpart.SetActive(true);
            }
        }
    }

    void TrackMouse(Vector3 ObjectPosition)
    {
        Vector2 ScreenPos = Input.mousePosition;
        MousePos = Camera.main.ScreenToWorldPoint(ScreenPos);
        MousePos.Set(MousePos.x, MousePos.y, ObjectPosition.z);
    }

    void ApplyAimRotations()
    {
        for (int i = 0; i < AimObjects.Length; i++)
        {

            Vector3 BonePos = AimObjects[i].Bone.transform.position;
            Vector3 ObjectPos = BonePos + AimObjects[i].Offset;
            Vector3 TargetPos = new Vector3(ObjectPos.x, ObjectPos.y, 0);
            TrackMouse(ObjectPos);
            AimObjects[i].Object.transform.position = ObjectPos;
            AimObjects[i].Object.transform.LookAt(MousePos);
          
        }
    }

}
