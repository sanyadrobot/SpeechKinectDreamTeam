using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;


public class DetectJoints : MonoBehaviour {
    public GameObject BodySourceManager;
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;
    public float speed = 10f;
	// Use this for initialization
	void Start () {
		
            bodyManager = BodySourceManager.GetComponent<BodySourceManager>();
    


    }
	
	// Update is called once per frame
	void Update () {
		if(bodyManager == null)
        {
            return;
        }
        
        bodies = bodyManager.GetData();

        if(bodies == null)
        {
            return;
        }
    
        foreach (var body in bodies)
        {
            if(body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
            
                var pos = body.Joints[TrackedJoint].Position;
                gameObject.transform.position = new Vector3(pos.X * speed, pos.Y * speed);

            }
        }




	}
}
