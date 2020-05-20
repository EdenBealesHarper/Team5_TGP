using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
   private Transform PlayerObject;

    [SerializeField]
    private float MoveSpeed =  0.125f;      //How fast the camera can move.
    [SerializeField]
    private Vector3 CameraOffset;           //Stores the base offset from the player.




    [SerializeField]
    private Vector3 MaxAimOffset;

   
    private Vector3 AimOffset;               //Stores the camera offset determined by the mouse position



    [SerializeField]
    private float MinDistance;                  //this determines how far the character has to be from the camera before it starts to move.
    [SerializeField]
    private float MaxDistance;                  //The furthest away the character can be from the camera.

    public float CurDistance;


    public bool bFollowMouse;
    



    [SerializeField]
    private Vector3 MousePos;



  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

   public void ToggleCameraMode(bool bShouldFollowMouse)
    {
        bFollowMouse = bShouldFollowMouse;
    }

    private void LateUpdate()
    {
        TrackMouse();
        CameraFollow();
    }

    void CameraFollow()
    {
        Vector3 CameraPos = transform.position;
        CameraPos.z = 0;
        CurDistance = Vector3.Distance(PlayerObject.transform.position, CameraPos);

       


        
        
        Vector3 DesiredPosition = PlayerObject.transform.position + CameraOffset;
    

        Vector3 TargetPosition = Vector3.Lerp(transform.position, DesiredPosition, Time.deltaTime * GetCameraAcceleration());
        if (bFollowMouse)
        {
            Vector3 AimPos = PlayerObject.transform.position + CameraOffset + MousePos;
            TargetPosition = Vector3.Lerp(transform.position, AimPos, Time.deltaTime * GetCameraAcceleration());

        }

        transform.position = TargetPosition;



        // if (ScreenRangeX > CameraMin.x && ScreenRangeX < CameraMax.x) 
        // if (ScreenRangeX > CameraMin.x && ScreenRangeX < CameraMax.x)




    }

    void CameraMouseFollow()
    {

    }
    void CalculateCameraOffset()
    {
        float Height = Screen.height / 2;
        float Width = Screen.width / 2;




      
        
    }

    void TrackMouse()
    {
        Vector2 ScreenPos = Input.mousePosition;
        MousePos = Camera.main.ScreenToWorldPoint(ScreenPos);
        MousePos.Set(MousePos.x - PlayerObject.position.x, MousePos.y - PlayerObject.position.y, PlayerObject.transform.position.z);
      MousePos.x = Mathf.Clamp(MousePos.x, -MaxAimOffset.x, MaxAimOffset.x);
      MousePos.y =  Mathf.Clamp(MousePos.y, -MaxAimOffset.y, MaxAimOffset.y);
    }


    float GetCameraAcceleration()
    {

       if (bFollowMouse)
        {
            if (CurDistance < MinDistance) return MoveSpeed;
            else if (CurDistance > MinDistance && CurDistance < MaxDistance) return MoveSpeed * (CurDistance * 2.5f);
            else return MoveSpeed / CurDistance;
        }
       else
        {
            if (CurDistance < MinDistance) return MoveSpeed;
            else if (CurDistance > MinDistance && CurDistance < MaxDistance) return MoveSpeed * (CurDistance * 4);
            else return MoveSpeed / CurDistance;
        }





    }
}
