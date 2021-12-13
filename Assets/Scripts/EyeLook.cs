using System;
using UnityEngine;
using Tobii.Gaming;

// Token: 0x0200003E RID: 62
//[RequireComponent(typeof(GazePointDataComponent))]
public class EyeLook : MonoBehaviour
{

    [Range(-60f, 60f)]
    [SerializeField]
    public float minAngle;
    [SerializeField]
    [Range(-60f, 60f)]
    public float maxAngle;
    public static bool isActive = true;
    //private GazePointDataComponent _gazePointDataComponent;
    private GameObject player;
    private Vector3 gazePointScreen;
    private Vector3 gazePointWorld;
    private Quaternion targetRotation;
    private Quaternion lastRotation;
	private float xRotation;
	private float bodyTurnSpeed=1f;
    private float velocity;
    private float speedFactor = 0.65f;
    private float w;
    private float h;
    private float ratio;
    private Rect[] r;
    private int maxRect = 5;
    private int offset = 40;
    private int i;
    public void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.w = (float)Screen.width;
        this.h = (float)Screen.height;
        this.ratio = this.w / this.h;
        this.r = new Rect[this.maxRect];
        this.r[0] = new Rect(0f, 0f, this.w, this.h);
        this.i = 1;
        while (this.i < this.r.Length)
        {
            this.r[this.i] = new Rect((float)(this.i * this.offset) * this.ratio, (float)(this.i * this.offset), this.w - this.ratio * (float)this.offset * (float)this.i * 2f, this.h - (float)(this.offset * this.i * 2));
            this.i++;
        }
    }
    public void Update()
    {
        this.lastRotation = this.player.transform.rotation;
        //GamePadState state = GamePad.GetState(PlayerIndex.One);
        /*if (state.IsConnected)
		{
			this.speedFactor = state.Triggers.Left + 1f;
		}*/
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.speedFactor = 2f;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            this.speedFactor = 1f;
        }
    }
    public void LateUpdate()
    {
        if (EyeLook.isActive)
        {
            this.player.transform.rotation = this.lastRotation;
            //EyeXGazePoint lastGazePoint = this._gazePointDataComponent.LastGazePoint;
            //EyeXGazePoint lastGazePoin=TobiiAPI.GetGazePoint();
            //if (lastGazePoint.IsValid && lastGazePoint.IsWithinScreenBounds)
            //{
            if (TobiiAPI.GetGazePoint().IsValid)
                this.centerCamera(TobiiAPI.GetGazePoint().Screen);
            //}
        }
    }
    private void centerCamera(Vector2 point)
    {
        this.velocity = this.getVelocity(point, 0);
        this.gazePointScreen = new Vector3(point.x, point.y, Camera.main.nearClipPlane);
        this.gazePointWorld = Camera.main.ScreenToWorldPoint(this.gazePointScreen);
        this.targetRotation = Quaternion.LookRotation(this.gazePointWorld - base.transform.position);
        //player.transform.localEulerAngles=new Vector3(0,player.transform.localEulerAngles.y,0);
        //Vector3 playerRotationVector=Vector3.ProjectOnPlane(targetRotation.eulerAngles,Vector3.right);
        Vector3 cameraRotationVector = Vector3.ProjectOnPlane(targetRotation.eulerAngles, transform.right);
        Quaternion playerRotationVector = Quaternion.LookRotation(new Vector3(gazePointWorld.x, base.transform.position.y, gazePointWorld.z) - base.transform.position);
        //Quaternion cameraRotationVector=Quaternion.LookRotation((new Vector3(0f,gazePointWorld.z,0f)+transform.forward*(this.gazePointWorld - base.transform.position).magnitude));

        //Debug.DrawLine(player.transform.position,cameraRotationVector,Color.red);
        //this.player.transform.rotation = Quaternion.Slerp(this.player.transform.rotation, this.targetRotation, Time.deltaTime * this.velocity * this.speedFactor);


        
        /*if (point.y > h * 0.75f)
            xRotation = -Vector3.Angle(transform.forward, cameraRotationVector);
        else if (point.y < h * 0.25f)
            xRotation = Vector3.Angle(transform.forward, cameraRotationVector);
        else
            xRotation = 0;*/
		//xRotation=-Vector3.Angle(transform.forward, cameraRotationVector)*Mathf.Pow(((point.y-0.5f*h)/(h*0.5f)),2);
		xRotation=-10*Mathf.Pow(((point.y-0.5f*h)/(h*0.5f)),1);
		bodyTurnSpeed=Mathf.Pow(((point.x-0.5f*w)/(w*0.5f)),2);
        //float xRotation=point.y>h/2? -Vector3.Angle(transform.forward,cameraRotationVector):Vector3.Angle(transform.forward,cameraRotationVector);
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //float rot=Mathf.Lerp(rot,xRotation,0.6f);
        this.player.transform.rotation = Quaternion.Slerp(this.player.transform.rotation, playerRotationVector, Time.deltaTime * this.velocity * bodyTurnSpeed*this.speedFactor);
		float camXrot=(transform.localEulerAngles.x>180?(transform.localEulerAngles.x-360):(transform.localEulerAngles.x))+xRotation;
		camXrot = Mathf.Clamp(camXrot, -60f, 60f);
		Debug.Log("xRot:"+xRotation +", camXrot:"+camXrot+", localRotation:"+transform.localEulerAngles.x);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(camXrot,0f,0f)), Time.deltaTime*this.velocity*this.speedFactor);


        //this.transform.rotation=Quaternion.Slerp(this.transform.rotation, cameraRotationVector, Time.deltaTime * this.velocity * this.speedFactor);
        //this.transform.rotation = this.ClampRotationXAxis(this.transform.rotation);
    }
    private float getVelocity(Vector2 point, int n)
    {
        if (n < this.r.Length && this.r[n].Contains(point))
        {
            return this.getVelocity(point, n + 1);
        }
        return 2.5f - (float)n * 0.1f;
    }
    private Quaternion ClampRotationXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;
        float num = 114.59156f * Mathf.Atan(q.x);
        num = Mathf.Clamp(num, this.minAngle, this.maxAngle);
        q.x = Mathf.Tan(0.008726646f * num);
        return q;
    }

}
