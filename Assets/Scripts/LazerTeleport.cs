using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTeleport : MonoBehaviour {

    public GameObject Player;
    public LazerPointer lazer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    {
        // speed should be 1 unit per second
        while (objectToMove.transform.position != end)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    public void TeleportMe(float speed)
    { // new implementation:
       // if (!lazer.Hidden)
        //StartCoroutine(MoveOverSpeed(Player, new Vector3(lazer.hit.point.x, Player.transform.position.y, lazer.hit.point.z), speed));

        Vector3 pos = Player.transform.position;
        pos.x = lazer.hit.point.x;
        pos.z = lazer.hit.point.z;
        Player.transform.position = pos;
        // added test for git
        // added second line
    }
}
