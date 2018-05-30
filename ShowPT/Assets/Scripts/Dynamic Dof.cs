using UnityEngine;
using System.Collections;
using System;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(Camera))]

public class StackDoFAutoFocus : MonoBehaviour
{

	private GameObject doFFocusTarget;
	private Vector3 lastDoFPoint;

	private PostProcessingProfile m_Profile;

	public DoFAFocusQuality focusQuality = StackDoFAutoFocus.DoFAFocusQuality.NORMAL;
	public LayerMask hitLayer = 1;
	public float maxDistance = 100.0f;
	public bool interpolateFocus = false;
	public float interpolationTime = 0.7f;

	public enum DoFAFocusQuality
	{
		NORMAL,
		HIGH
	}

	void Start()
	{
		doFFocusTarget = new GameObject("DoFFocusTarget");
		var behaviour = GetComponent<PostProcessingBehaviour>();
		m_Profile = behaviour.profile;
	}

	void Update()
	{

		// switch between Modes Test Focus every Frame
		if (focusQuality == StackDoFAutoFocus.DoFAFocusQuality.HIGH)
		{
			Focus();
		}

	}

	void FixedUpdate()
	{
		// switch between modes Test Focus like the Physicsupdate
		if (focusQuality == StackDoFAutoFocus.DoFAFocusQuality.NORMAL)
		{
			Focus();
		}
	}

	IEnumerator InterpolateFocus(Vector3 targetPosition)
	{

		Vector3 start = this.doFFocusTarget.transform.position;
		Vector3 end = targetPosition;
		float dTime = 0;

		// Debug.DrawLine(start, end, Color.green);
		var depthOfField = m_Profile.depthOfField.settings;
		while (dTime < 1)
		{
			yield return new WaitForEndOfFrame();
			dTime += Time.deltaTime / this.interpolationTime;
			this.doFFocusTarget.transform.position = Vector3.Lerp(start, end, dTime);
			depthOfField.focusDistance = Vector3.Distance(doFFocusTarget.transform.position, transform.position);
			m_Profile.depthOfField.settings = depthOfField;
		}
		this.doFFocusTarget.transform.position = end;
	}

	void Focus()
	{
		// our ray
		Ray ray = transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, this.maxDistance, this.hitLayer))
		{
			Debug.DrawLine(ray.origin, hit.point);

			// do we have a new point?					
			if (this.lastDoFPoint == hit.point)
			{
				return;
				// No, do nothing
			}
			else if (this.interpolateFocus)
			{ // Do we interpolate from last point to the new Focus Point ?
				// stop the Coroutine
				StopCoroutine("InterpolateFocus");
				// start new Coroutine
				StartCoroutine(InterpolateFocus(hit.point));
			}
			else
			{
				this.doFFocusTarget.transform.position = hit.point;
				var depthOfField = m_Profile.depthOfField.settings;
				depthOfField.focusDistance = Vector3.Distance(doFFocusTarget.transform.position, transform.position);
				// print(depthOfField.focusDistance);
				m_Profile.depthOfField.settings = depthOfField;
			}
			// asign the last hit
			this.lastDoFPoint = hit.point;
		}
	}
}