// "Wave SDK 
// © 2020 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the Wave SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Wave.Native;
using Wave.Essence.Raycast;

namespace Wave.Essence.Samples.Raycast
{
	[DisallowMultipleComponent]
	public class CubeEventHandler : MonoBehaviour,
		IPointerEnterHandler,
		IHoverHandler,
		IPointerExitHandler,
		IPointerDownHandler,
		IBeginDragHandler,
		IDragHandler,
		IEndDragHandler,
		IDropHandler,
		IPointerUpHandler,
		IPointerClickHandler,
		ISubmitHandler
	{
		const string LOG_TAG = "Wave.Essence.Samples.Raycast.CubeEventHandler";
		private void DEBUG(string msg) { Log.d(LOG_TAG, msg, true); }

		private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
		private Vector3 m_Position = Vector3.zero;
		private float fDistanceInMeter = 0;
		private int raycastID = 0;
		private bool IsDragging => raycastID != 0;
		void OnEnable()
		{
			fDistanceInMeter = transform.position.z;
			StartCoroutine("TrackPointer");
		}

		void OnDisable()
		{
			StopCoroutine("TrackPointer");
		}

		private void TeleportRandomly()
		{
			Vector3 direction = Random.onUnitSphere;
			direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
			direction.z = transform.localPosition.z;
			transform.localPosition = direction;
		}

		#region override event handling function
		public void OnPointerEnter(PointerEventData eventData)
		{
			DEBUG("OnPointerEnter, camera: " + eventData.enterEventCamera);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			// Do nothing
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			DEBUG("OnPointerDown()");
			ChangeColor();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			DEBUG("OnPointerUp()");
			ResetRaycastID(eventData.enterEventCamera);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			Camera _cam = eventData.enterEventCamera;
			if (_cam != null)
			{
				raycastID = _cam.GetInstanceID();
			}
			m_Position = transform.position;

			DEBUG("OnBeginDrag() position: " + m_Position);
		}

		public void OnDrag(PointerEventData eventData)
		{
			Camera _cam = eventData.enterEventCamera;
			if (_cam != null && _cam.GetInstanceID() == raycastID)
			{
				m_Position = _cam.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, fDistanceInMeter));
				DEBUG("OnDrag() position: " + m_Position);
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			DEBUG("OnEndDrag() position: " + m_Position);
		}

		public void OnDrop(PointerEventData eventData)
		{
			m_Position = eventData.enterEventCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, fDistanceInMeter));
			DEBUG("OnDrop() position: " + m_Position);
		}

		public void OnHover(RaycastEventData eventData)
		{
			//DEBUG("OnHover() from " + eventData.Actor);
			Rotate();
		}

		private void Rotate()
		{
			transform.Rotate(12 * (10 * Time.deltaTime), 0, 0);
			transform.Rotate(0, 12 * (10 * Time.deltaTime), 0);
		}

		private Color[] cubeColors = { Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.red, Color.white };
		private uint cubeColorIndex = 0;
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.enterEventCamera != null && !IsDragging)
			{
				m_Position = eventData.enterEventCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, fDistanceInMeter));
				DEBUG("OnPointerClick() position: " + m_Position);
			}

			//TeleportRandomly();
		}

		public void OnSubmit(BaseEventData eventData)
		{
			DEBUG("OnSubmit()");
			ChangeColor();
		}

		void ChangeColor()
		{
			GetComponent<Renderer>().material.color = cubeColors[(cubeColorIndex++ % cubeColors.Length)];
		}

		private void ResetRaycastID(Camera camera)
		{
			if (camera != null)
			{
				if (camera.GetInstanceID() == raycastID)
				{
					raycastID = 0;
				}
			}
		}
		#endregion

		IEnumerator TrackPointer()
		{
			while (true)
			{
				yield return waitForEndOfFrame;

				if (IsDragging)
				{
					transform.position = m_Position;
				}
			}
		}

		public void OnShowCube()
		{
			DEBUG("OnShowButton");
			transform.gameObject.SetActive(true);
		}

		public void OnHideCube()
		{
			DEBUG("OnHideButton");
			transform.gameObject.SetActive(false);
		}

		public void OnTrigger()
		{
			TeleportRandomly();
		}
	}
}
