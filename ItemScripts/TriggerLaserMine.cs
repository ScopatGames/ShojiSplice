using UnityEngine;
using System.Collections;

public class TriggerLaserMine : MonoBehaviour {

	public float armDelay = 1f;
	public float maxLaserSweepAngle = 45f;
	public float maxLaserRange = 5f;
	public float laserSweepSpeed = 1f;
	public LayerMask mask;
	//Audio clip one is attachment sound; audio clip two is the armed sound...
	public AudioClip[] audioClips;
	[HideInInspector] public bool triggerExplode = false;


	private Vector3 direction = Vector3.zero;
	private bool armed = false;
	private float theta=0f;
	private float theta0 = 0f;
	private float delayTimer=0f;
	private float laserSweepCounter = 0f;
	private LineRenderer lineRenderer;
	private AudioSource audioSource;
	private EnemyStats enemyStats;





	void Start () {
		enemyStats = GetComponentInChildren<EnemyStats> ();
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.SetPosition (0, transform.position);
		lineRenderer.SetPosition (1, transform.position);
		audioSource = GetComponent<AudioSource> ();
		if (armDelay == 0f) {
			lineRenderer.enabled = true;
			armed = true;
		}
		else{
			audioSource.clip = audioClips[0];
			audioSource.volume = 0.3f;
			audioSource.Play ();
		}

	}
	

	void Update () {
		if(!armed){
			delayTimer += Time.deltaTime;
			if(delayTimer > armDelay){
				armed = true;
				lineRenderer.enabled=true;
				audioSource.clip = audioClips[1];
				audioSource.volume = 1f;
				audioSource.Play ();
			}
		}
		else if(enemyStats==null){
			GetComponent<grenadeExplode>().Explode();
		}
		else{
			theta0 = transform.eulerAngles.y;
			theta = maxLaserSweepAngle*Mathf.Sin(laserSweepCounter);
			direction = new Vector3(Mathf.Sin ((theta0+theta)*Mathf.Deg2Rad), 0f, Mathf.Cos ((theta0 + theta)*Mathf.Deg2Rad));
			laserSweepCounter += laserSweepSpeed*Time.deltaTime;
			RaycastHit hit;
			if(Physics.Raycast(transform.position,direction, out hit, maxLaserRange, mask)){
				if(hit.collider.tag == Tags.player || hit.collider.tag == Tags.enemy || hit.collider.tag == Tags.npc || hit.collider.tag == Tags.neutral){
					triggerExplode = true;
				}
				//Draw ray to hit point...
				lineRenderer.SetPosition(1, hit.point);
				//Debug.DrawRay(transform.position, direction*hit.distance, Color.red);
			}
			else{
				//Draw ray to max range...
				lineRenderer.SetPosition(1, transform.position+direction*maxLaserRange);
				//Debug.DrawRay(transform.position, direction*maxLaserRange, Color.red);
			}
			if(triggerExplode){
				GetComponent<grenadeExplode>().Explode();
			}
		}
	}
}
