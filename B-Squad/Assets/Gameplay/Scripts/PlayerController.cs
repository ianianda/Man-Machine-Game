using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerCharacter character; // get the PlayerCharacter class
    private void Start()
    {
        // MonoBehaviour is asset, so cannot use new ().
        character = GetComponent<PlayerCharacter>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            character.Attack();
        }

        // gets move position.
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        // players do not have any movement happens on y axis, so pass 0.
        character.Move(new Vector3(h, 0, v));

        // Vector3 is world matrix, forward is z axis, right is x axis.
        var lookDir = Vector3.forward * v + Vector3.right * h;

        // check if needs to rotate
        // magnitude = sqrt(x*x+z*z)
        if(lookDir.magnitude != 0)
        {
            character.Rotate(lookDir);
        }
    }
}
