using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CameraMotion : MonoBehaviour 
{
	[HideInInspector]public List<points> pointsList = new List<points>();
	string pathToFilenameForSerialization = "Assets/CameraMotion/Files/SerializeFile";
	points p;
	public GameObject target;
	[HideInInspector] public List<points> LeftAndRightEndPoints = new List<points>();
	[HideInInspector] public int curveCount, selectButton = 0;
	[HideInInspector] public bool create, change;

	void Start () 
	{
		if (create == true && change == false) {
			pointsList = Load ();
			LeftAndRightEndPoints.Add (new points ("", Vector3.zero, ""));
			LeftAndRightEndPoints.Add (new points ("", Vector3.zero, ""));
		}
		else {
			if(create == false)
				Debug.LogError("You don't create curve!");
			else if(change == true)
				Debug.LogError("You don't save curve!");
		}
	}

	void Update () 
	{
		if (create == true && change == false) {
			ArrayList LeftPoints = new ArrayList ();
			ArrayList RightPoints = new ArrayList ();
			for (int element = 0; element < pointsList.Count; element++) {
				p = (points)pointsList [element];
				if (p.point.x < target.transform.position.x && p.tag == "EndPointForCam") {
					LeftPoints.Add (p);
				} else if (p.point.x > target.transform.position.x && p.tag == "EndPointForCam") {
					RightPoints.Add (p);
				}
			}

			float maximum = -999999;
			int numberGameObjL = 0;
			for (int element = 0; element < LeftPoints.Count; element++) {
				p = (points)LeftPoints [element];
				if (p.point.x > maximum) {
					maximum = p.point.x;
					numberGameObjL = element;
				}
			}
			p = (points)LeftPoints [numberGameObjL];
			LeftAndRightEndPoints [0] = p;

			float minimum = 999999;
			int numberGameObjR = 0;
			for (int element = 0; element < RightPoints.Count; element++) {
				p = (points)RightPoints [element];
				if (p.point.x > minimum) {
					minimum = p.point.x;
					numberGameObjR = element;
				}
			}
			p = (points)RightPoints [numberGameObjR];
			LeftAndRightEndPoints [1] = p;

			float disBetweenPointsX = Mathf.Abs (LeftAndRightEndPoints [1].point.x - LeftAndRightEndPoints [0].point.x);
			float disBetweenPlayerAndLeftPointX = Mathf.Abs (target.transform.position.x - LeftAndRightEndPoints [0].point.x);
			float t = disBetweenPlayerAndLeftPointX / disBetweenPointsX;

			int indexLeft = pointsList.IndexOf (LeftAndRightEndPoints [0]);

			points p0 = (points)pointsList [indexLeft];
			points p1 = (points)pointsList [indexLeft + 1];
			points p2 = (points)pointsList [indexLeft + 2];
			points p3 = (points)pointsList [indexLeft + 3];

			gameObject.transform.position = CalculateCubicBezierPoint (t, ConvertToVector3 (p0.point), ConvertToVector3 (p1.point), ConvertToVector3 (p2.point), ConvertToVector3 (p3.point));
		}
	}


	public Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) 
	{ 
		float u = 1 - t; 
		float tt = t * t;
		float uu = u * u; 
		float uuu = uu * u; 
		float ttt = tt * t; 
		Vector3 p = uuu * p0; 
		p += 3 * uu * t * p1; 
		p += 3 * u * tt * p2; 
		p += ttt * p3;
		return p;
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		for (int j = 0; j < curveCount; j++) {
			for (int i = 1; i < 50; i++) {
				float t = (i - 1f) / 49f;
				float t1 = i / 49f;
				int indexLeft = j * 3;
				points p0 = (points)pointsList[indexLeft];
				points p1 = (points)pointsList[indexLeft+1];
				points p2 = (points)pointsList[indexLeft+2];
				points p3 = (points)pointsList[indexLeft+3];
				Gizmos.DrawLine (CalculateCubicBezierPoint (t, ConvertToVector3(p0.point), ConvertToVector3(p1.point), ConvertToVector3(p2.point), ConvertToVector3(p3.point)), CalculateCubicBezierPoint (t1, ConvertToVector3(p0.point), ConvertToVector3(p1.point), ConvertToVector3(p2.point), ConvertToVector3(p3.point)));
			}
		}
		if (curveCount > 0) {
			Gizmos.color = Color.red;
			for (int i = 1; i < 50; i++) {
				float t = (i - 1f) / 49f;
				float t1 = i / 49f;
				points p0 = (points)pointsList [selectButton];
				points p1 = (points)pointsList [selectButton + 1];
				points p2 = (points)pointsList [selectButton + 2];
				points p3 = (points)pointsList [selectButton + 3];
				Gizmos.DrawLine (CalculateCubicBezierPoint (t, ConvertToVector3 (p0.point), ConvertToVector3 (p1.point), ConvertToVector3 (p2.point), ConvertToVector3 (p3.point)), CalculateCubicBezierPoint (t1, ConvertToVector3 (p0.point), ConvertToVector3 (p1.point), ConvertToVector3 (p2.point), ConvertToVector3 (p3.point)));
			}
		}
	}

	public Vector3 ConvertToVector3(ConvertVector3 convertVector)
	{
		return new Vector3 (convertVector.x, convertVector.y, convertVector.z);
	}

	public static void SaveListToBinnary<points>(String FileName, List<points> SerializableObjects)
	{
		using (FileStream fs = File.Create(FileName))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			try 
			{
				formatter.Serialize(fs, SerializableObjects);
				Debug.Log("Serialization success");
			}
			catch (SerializationException e)  {
				Debug.LogError ("Failed serialization");
			}
		}
	}
	public static List<points> LoadListFromBinnary<points>(String FileName)
	{
		using (FileStream fs = File.Open(FileName, FileMode.Open))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			try 
			{
				List<points> list = (List<points>)formatter.Deserialize(fs);
				Debug.Log("Deserialization success");
				return list;
			}
			catch(SerializationException e) {
				Debug.LogError ("Failed deserialization");
				return null;
			}
		}
	}
	public void Save()
	{
		SaveListToBinnary<points> (pathToFilenameForSerialization, pointsList);
	}
	public List<points> Load()
	{
		return LoadListFromBinnary<points> (pathToFilenameForSerialization);
	}
}

[Serializable]
public struct points
{
	public string tag;
	public ConvertVector3 point;
	public string name;
	public points(string tag_, Vector3 point_, string name_)
	{
		tag = tag_;
		point = new ConvertVector3 (point_);
		name = name_;
	}
}

[Serializable]
public class ConvertVector3
{
	public float x;
	public float y;
	public float z;
	public ConvertVector3(Vector3 vector)
	{
		x = vector.x;
		y = vector.y;
		z = vector.z;
	}
}
