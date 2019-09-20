using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic; 
using System;

[CustomEditor (typeof(CameraMotion))]

public class CameraMotionEditor : Editor 
{
	const string curveNameConst = "Curve ";
	float buttonWidth = 25, buttonHeight = 20;
	public override void OnInspectorGUI()
	{
		CameraMotion MotionScr = target as CameraMotion;
		if (MotionScr.target == null)
			EditorGUILayout.HelpBox ("The target is not assigned!", MessageType.Warning);
		if (MotionScr.change)
			EditorGUILayout.HelpBox ("Curve not saved!", MessageType.Warning);
		DrawDefaultInspector ();
		if (MotionScr.pointsList.Count == 0) {
			if (GUILayout.Button ("Create curve")) {
				MotionScr.change = true;
				MotionScr.create = true;
				MotionScr.curveCount++;
				MotionScr.selectButton = 0;
				MotionScr.pointsList.Clear ();
				MotionScr.pointsList.Add (new points ("EndPointForCam", MotionScr.transform.position, curveNameConst));
				MotionScr.pointsList.Add (new points ("MiddlePointForCam", MotionScr.transform.position, ""));
				MotionScr.pointsList.Add (new points ("MiddlePointForCam", MotionScr.transform.position, ""));
				MotionScr.pointsList.Add (new points ("EndPointForCam", MotionScr.transform.position, curveNameConst));
			}
		} else {
			if (GUILayout.Button ("Add curve!")) {
				MotionScr.change = true;
				MotionScr.curveCount++;
				MotionScr.pointsList.Add (new points ("MiddlePointForCam", MotionScr.ConvertToVector3 (MotionScr.pointsList [MotionScr.pointsList.Count - 1].point), ""));
				MotionScr.pointsList.Add (new points ("MiddlePointForCam", MotionScr.ConvertToVector3 (MotionScr.pointsList [MotionScr.pointsList.Count - 1].point), ""));
				MotionScr.pointsList.Add (new points ("EndPointForCam", MotionScr.ConvertToVector3 (MotionScr.pointsList [MotionScr.pointsList.Count - 1].point), curveNameConst));
			}
		}
		if (MotionScr.change) {
			if (GUILayout.Button ("Save")) {
				MotionScr.Save ();
				MotionScr.change = false;
			}
		}
		EditorGUILayout.BeginVertical ();
		if (MotionScr.create) {
			for (int element = 0, element2 = 1; element < MotionScr.pointsList.Count - 1; element += 3, element2++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label (MotionScr.pointsList [element].name + " " + (element2));
				if (MotionScr.selectButton == element) {
					buttonWidth = 18;
					buttonHeight = 20;
				} else {
					buttonWidth = 25;
					buttonHeight = 20;
				}
				if (GUILayout.Button ("S", GUILayout.Width (buttonWidth), GUILayout.Height (buttonHeight))) {
					MotionScr.selectButton = element;
				}
				if (GUILayout.Button ("X", GUILayout.Width (25), GUILayout.Height (20))) {
					if (element == MotionScr.pointsList.Count - 4 && element != 0) {
						MotionScr.pointsList.RemoveAt (element + 1);
						MotionScr.pointsList.RemoveAt (element + 1);
						MotionScr.pointsList.RemoveAt (element + 1);
						if(MotionScr.selectButton == element)
							MotionScr.selectButton -= 3;
					} else if (element != MotionScr.pointsList.Count - 4 && element != 0) {
						MotionScr.pointsList.RemoveAt (element);
						MotionScr.pointsList.RemoveAt (element);
						MotionScr.pointsList.RemoveAt (element);
					} else {
						MotionScr.pointsList.Clear ();
						MotionScr.create = false;
					}
					MotionScr.curveCount--;
					MotionScr.change = true;
				}
				EditorGUILayout.EndHorizontal ();
				GUILayout.Space (10);
			}
		}
		EditorGUILayout.EndVertical ();
		if (MotionScr.create) {
			if (GUILayout.Button ("Reset")) {
				MotionScr.pointsList.Clear ();
				MotionScr.create = false;
				MotionScr.curveCount = 0;
				MotionScr.change = true;
			}
		}
	}

	public void OnSceneGUI()
	{
		CameraMotion MotionScr = target as CameraMotion;
		if (MotionScr && MotionScr.create == true) {
			Quaternion rot = Quaternion.identity;
			for (int element = 0; element < MotionScr.pointsList.Count; element++) {
				points p;
				p = (points)MotionScr.pointsList [element];
				float size = HandleUtility.GetHandleSize (MotionScr.ConvertToVector3 (p.point)) * 0.2f;
				MotionScr.pointsList [element] = new points (p.tag, Handles.FreeMoveHandle (MotionScr.ConvertToVector3 (p.point), rot, size, Vector3.zero, Handles.SphereCap), p.name);
			}
			for (int element = 0; element < MotionScr.pointsList.Count - 1; element += 3) {
				points p, p1;
				p = (points)MotionScr.pointsList [element];
				p1 = (points)MotionScr.pointsList [element + 1];
				Handles.DrawLine (MotionScr.ConvertToVector3 (p.point), MotionScr.ConvertToVector3 (p1.point));
			}
			for (int element = 3; element < MotionScr.pointsList.Count; element += 3) {
				points p, p1;
				p = (points)MotionScr.pointsList [element];
				p1 = (points)MotionScr.pointsList [element - 1];
				Handles.DrawLine (MotionScr.ConvertToVector3 (p.point), MotionScr.ConvertToVector3 (p1.point));
			}
		}
		if (GUI.changed) {
			EditorUtility.SetDirty (target);
			MotionScr.change = true;
		}
	}
}
