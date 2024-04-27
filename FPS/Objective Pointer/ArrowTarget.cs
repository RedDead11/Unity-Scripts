using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ArrowTarget : MonoBehaviour
{
    public float rotationSpeed;

    public FirstPersonController FPS;

    public List<Transform> target;


    private void Update()
    {
        // Point to key

        if(!FPS.hasPassword)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target[0].position - transform.position), rotationSpeed * Time.deltaTime);

        }

        // Point to terminal

        else if (FPS.hasPassword && !FPS.hasOpenedDoor)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target[1].position - transform.position), rotationSpeed * Time.deltaTime);
        } 

        // Destroy arrow when doors open

        else if(FPS.hasOpenedDoor)
        {
            Destroy(gameObject);
        }
    }
}
