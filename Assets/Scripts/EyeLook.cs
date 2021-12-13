using System;
using UnityEngine;
using Tobii.Gaming;

// Token: 0x0200003E RID: 62
//[RequireComponent(typeof(GazePointDataComponent))]
public class EyeLook : MonoBehaviour
{
	// Token: 0x0600013B RID: 315 RVA: 0x00008374 File Offset: 0x00006574
	public void Start()
	{
		//this._gazePointDataComponent = base.GetComponent<GazePointDataComponent>();
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

	// Token: 0x0600013C RID: 316 RVA: 0x000084AC File Offset: 0x000066AC
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

	// Token: 0x0600013D RID: 317 RVA: 0x0000852C File Offset: 0x0000672C
	public void LateUpdate()
	{
		if (EyeLook.isActive)
		{
			this.player.transform.rotation = this.lastRotation;
			//EyeXGazePoint lastGazePoint = this._gazePointDataComponent.LastGazePoint;
            //EyeXGazePoint lastGazePoin=TobiiAPI.GetGazePoint();
			//if (lastGazePoint.IsValid && lastGazePoint.IsWithinScreenBounds)
			//{
                if(TobiiAPI.GetGazePoint().IsValid)
				this.centerCamera(TobiiAPI.GetGazePoint().Screen);
			//}
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000858C File Offset: 0x0000678C
	private void centerCamera(Vector2 point)
	{
		this.velocity = this.getVelocity(point, 0);
		this.gazePointScreen = new Vector3(point.x, point.y, Camera.main.nearClipPlane);
		this.gazePointWorld = Camera.main.ScreenToWorldPoint(this.gazePointScreen);
		this.targetRotation = Quaternion.LookRotation(this.gazePointWorld - base.transform.position);
        //player.transform.localEulerAngles=new Vector3(0,player.transform.localEulerAngles.y,0);
        //Vector3 playerRotationVector=Vector3.ProjectOnPlane(targetRotation.eulerAngles,Vector3.right);
        Vector3 cameraRotationVector=Vector3.ProjectOnPlane(targetRotation.eulerAngles,transform.right);
        Quaternion playerRotationVector=Quaternion.LookRotation(new Vector3(gazePointWorld.x,base.transform.position.y, gazePointWorld.z) - base.transform.position);
        //Quaternion cameraRotationVector=Quaternion.LookRotation((new Vector3(0f,gazePointWorld.z,0f)+transform.forward*(this.gazePointWorld - base.transform.position).magnitude));
                
        //Debug.DrawLine(player.transform.position,cameraRotationVector,Color.red);
		//this.player.transform.rotation = Quaternion.Slerp(this.player.transform.rotation, this.targetRotation, Time.deltaTime * this.velocity * this.speedFactor);
        
        
        float xRotation;
        if(point.y>h*0.75f)
            xRotation=-Vector3.Angle(transform.forward,cameraRotationVector);
        else if(point.y<h*0.25f)
            xRotation=Vector3.Angle(transform.forward,cameraRotationVector);
        else
            xRotation=0;
        //float xRotation=point.y>h/2? -Vector3.Angle(transform.forward,cameraRotationVector):Vector3.Angle(transform.forward,cameraRotationVector);
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //float rot=Mathf.Lerp(rot,xRotation,0.6f);
        this.player.transform.rotation = Quaternion.Slerp(this.player.transform.rotation, playerRotationVector , Time.deltaTime * this.velocity * this.speedFactor);
        transform.localRotation = Quaternion.Slerp(transform.localRotation,Quaternion.Euler(transform.localEulerAngles+new Vector3(xRotation,0f,0f)),0.004f) ;


        //this.transform.rotation=Quaternion.Slerp(this.transform.rotation, cameraRotationVector, Time.deltaTime * this.velocity * this.speedFactor);
		this.transform.rotation = this.ClampRotationXAxis(this.transform.rotation);
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00008668 File Offset: 0x00006868
	private float getVelocity(Vector2 point, int n)
	{
		if (n < this.r.Length && this.r[n].Contains(point))
		{
			return this.getVelocity(point, n + 1);
		}
		return 2.5f - (float)n * 0.1f;
	}

	// Token: 0x06000140 RID: 320 RVA: 0x000086B4 File Offset: 0x000068B4
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

	// Token: 0x0400015A RID: 346
	[Range(-60f, 60f)]
	[SerializeField]
	public float minAngle;

	// Token: 0x0400015B RID: 347
	[SerializeField]
	[Range(-60f, 60f)]
	public float maxAngle;

	// Token: 0x0400015C RID: 348
	public static bool isActive = true;

	// Token: 0x0400015D RID: 349
	//private GazePointDataComponent _gazePointDataComponent;

	// Token: 0x0400015E RID: 350
	private GameObject player;

	// Token: 0x0400015F RID: 351
	private Vector3 gazePointScreen;

	// Token: 0x04000160 RID: 352
	private Vector3 gazePointWorld;

	// Token: 0x04000161 RID: 353
	private Quaternion targetRotation;

	// Token: 0x04000162 RID: 354
	private Quaternion lastRotation;

	// Token: 0x04000163 RID: 355
	private float velocity;

	// Token: 0x04000164 RID: 356
	private float speedFactor = 1f;

	// Token: 0x04000165 RID: 357
	private float w;

	// Token: 0x04000166 RID: 358
	private float h;

	// Token: 0x04000167 RID: 359
	private float ratio;

	// Token: 0x04000168 RID: 360
	private Rect[] r;

	// Token: 0x04000169 RID: 361
	private int maxRect = 5;

	// Token: 0x0400016A RID: 362
	private int offset = 40;

	// Token: 0x0400016B RID: 363
	private int i;
}
