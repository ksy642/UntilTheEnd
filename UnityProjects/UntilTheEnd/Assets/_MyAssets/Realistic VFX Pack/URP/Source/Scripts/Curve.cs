using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;
using RFX.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.VFX;
using System.Collections;
using UnityEngine.VFX.Utility;


namespace RFX
{

	[ExecuteInEditMode]
	public class Curve : MonoBehaviour
	{
		public static readonly int MinCount = 4;
		public static readonly int MaxCount = 16;


		[Header("Assignments")]
		public Transform pointsParent;
		public Transform vfxParent;

		[Header("Path")]
		public bool closed = false;
		[Range(0, 1)]
		public float curviness = 0.5f;
		[Min(0.01f)]
		public float radius = 5;
		[Min(0.0001f)]
		public float speed = 0.05f;
		[Min(0.0001f)]
		public float followPathForce = 0.5f;
		[Min(0.01f)]
		public float drag = 15f;

		public bool showIndex = false;
		public float handlesSize = 1.0f;
		public int labelSize = 20;
		public Color curveColor = new(0, 0, 1);
		public float curveThickness = 3.0f;
		public Color handlesColor = new(1, 1, 1);
		public Color labelColor1 = new(1, 0, 0);
		public Color labelColor2 = new(0, 0, 1);


		public List<Transform> ActivePoints
		{
			get => pointsParent.GetAllChildren()
					.Where(t => t.gameObject.activeSelf)
					.ToList();
		}

		private void Add()
		{
			Transform nextPoint = pointsParent.GetChild(ActivePoints.Count);
			nextPoint.gameObject.SetActive(true);
		}
		private void Remove()
		{
			Transform nextPoint = pointsParent.GetChild(ActivePoints.Count - 1);
			nextPoint.gameObject.SetActive(false);
		}
		private void RemoveRange(int index, int count)
		{
			for (int i = index; i < index + count; i++)
			{
				Transform nextPoint = pointsParent.GetChild(i);
				nextPoint.gameObject.SetActive(false);
			}
		}


		public void RefreshPoints()
		{
			bool changed = false;

			// Remove if null.
			int removeIndex = -1;
			for (int i = 0; i < ActivePoints.Count; i++)
			{
				if (ActivePoints[i] == null)
				{
					changed = true;
					removeIndex = i;
					break;
				}
			}
			if (removeIndex != -1)
				RemoveRange(removeIndex, ActivePoints.Count - removeIndex);

			while (ActivePoints.Count < MinCount)
			{
				Add();
				changed = true;
			}
			while (ActivePoints.Count > MaxCount)
			{
				Remove();
				changed = true;
			}

			if (changed)
				UpdateProperties();
		}

		public bool AddPoint()
		{
			if (ActivePoints.Count >= MaxCount) return false;

			if (pointsParent == null) return false;
			if (pointsParent.childCount <= ActivePoints.Count) return false;

			Add();

			UpdateProperties();

			return true;
		}
		public bool RemovePoint()
		{
			if (ActivePoints.Count <= MinCount) return false;

			Remove();

			UpdateProperties();

			return true;
		}


		internal void UpdateProperties()
		{
			foreach (var child in vfxParent.GetAllChildren())
			{
				if (child.TryGetComponent(out VisualEffect vfx))
				{
					vfx.SetInt("Count", ActivePoints.Count);
					vfx.SetBool("Closed", closed);
					vfx.SetFloat("Curviness", curviness);
					vfx.SetFloat("Radius", radius);
					vfx.SetFloat("Speed", speed);
					vfx.SetFloat("Follow Path Force", followPathForce);
					vfx.SetFloat("Drag", drag);
				}
			}
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(Curve))]
	public class CurveEditor : Editor
	{
		SerializedProperty _pointsParent;
		SerializedProperty _vfxParent;
		SerializedProperty _closed;
		SerializedProperty _curviness;
		SerializedProperty _radius;
		SerializedProperty _speed;
		SerializedProperty _followPathForce;
		SerializedProperty _drag;
		SerializedProperty _showIndex;
		SerializedProperty _curveThickness, _curveColor;
		SerializedProperty _handlesSize, _handlesColor;
		SerializedProperty _labelSize, _labelColor1, _labelColor2;

		bool f_debug = false;


		private string feedbackText = "";
		private const int feedbackTime = 2; //seconds
		private bool feedbackDisplaying = false;
		private MessageType feedbackType;


		private void OnEnable()
		{
			FindProperties();

			Bind();

			Curve t = target as Curve;

			Undo.undoRedoPerformed += t.UpdateProperties;

			t.RefreshPoints();
			t.UpdateProperties();
		}

		private void OnDisable()
		{
			Curve t = target as Curve;

			Undo.undoRedoPerformed -= t.UpdateProperties;
		}


		public override void OnInspectorGUI()
		{
			var t = target as Curve;

			t.RefreshPoints();

			if (t.pointsParent != null)
			{
				int childCount = t.pointsParent.childCount;
				if (childCount > Curve.MaxCount)
				{
					EditorGUILayout.HelpBox($"Points Parent contains too many points. Please remove {childCount - Curve.MaxCount} of them.", MessageType.Error);
				}
				if (childCount < Curve.MaxCount)
				{
					EditorGUILayout.HelpBox($"Points Parent does not contains enough points. Please add {Curve.MaxCount - childCount} children.", MessageType.Error);
				}
			}

			EditorGUILayout.PropertyField(_pointsParent);
			EditorGUILayout.PropertyField(_vfxParent);

			EditorGUILayout.Space();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_closed);
			EditorGUILayout.PropertyField(_curviness);
			EditorGUILayout.PropertyField(_radius);
			EditorGUILayout.PropertyField(_speed);
			EditorGUILayout.PropertyField(_followPathForce);
			EditorGUILayout.PropertyField(_drag);
			if (EditorGUI.EndChangeCheck())
			{
				EditorApplication.delayCall += t.UpdateProperties;
			}

			if (t.vfxParent == null)
			{
				EditorGUILayout.HelpBox("Assign VFX.", MessageType.Error);
			}

			if (t.pointsParent != null)
			{
				GUI.enabled = false;
				EditorGUILayout.IntSlider(new GUIContent("POINTS", $"Amount of points currently being used. MAX is {Curve.MaxCount}."), t.ActivePoints.Count, Curve.MinCount, Curve.MaxCount);
				GUI.enabled = true;
			}
			else
			{
				EditorGUILayout.HelpBox("Assign points parent.", MessageType.Error);
			}

			Color cache = GUI.backgroundColor;
			GUI.backgroundColor = new(.56f, .93f, .56f);
			if (GUILayout.Button("NEW POINT"))
			{
				if (!t.AddPoint())
				{
					Feedback(t, $"Could not add a new point.{(t.ActivePoints.Count >= Curve.MaxCount ? " Limit reached." : "")}", MessageType.Warning);
				}
			}
			GUI.backgroundColor = new(.93f, .56f, .56f);
			if (GUILayout.Button("REMOVE LAST POINT"))
			{
				if (!t.RemovePoint())
				{
					Feedback(t, $"Could not remove any points.{(t.ActivePoints.Count <= Curve.MinCount ? " Limit reached." : "")}", MessageType.Warning);
				}
			}
			GUI.backgroundColor = cache;

			if (feedbackDisplaying)
			{
				EditorGUILayout.HelpBox(feedbackText, feedbackType);
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			f_debug = EditorGUILayout.BeginFoldoutHeaderGroup(f_debug, "Debug");
			if (f_debug)
			{
				EditorGUILayout.PropertyField(_showIndex);
				EditorGUILayout.PropertyField(_handlesSize);
				EditorGUILayout.PropertyField(_curveThickness);
				EditorGUILayout.PropertyField(_curveColor);
				EditorGUILayout.PropertyField(_handlesColor);
				EditorGUILayout.PropertyField(_labelSize);
				EditorGUILayout.PropertyField(_labelColor1);
				EditorGUILayout.PropertyField(_labelColor2);
			}

			serializedObject.ApplyModifiedProperties();
		}


		public void OnSceneGUI()
		{
			FindProperties();

			const float HandleSnapMagnitude = 0.15f;

			var t = target as Curve;

			if (t.pointsParent == null) return;

			var textStyle = new GUIStyle();
			textStyle.fontStyle = FontStyle.Bold;
			textStyle.fontSize = t.labelSize;
			textStyle.alignment = TextAnchor.UpperCenter;
			textStyle.normal.textColor = t.labelColor1;

			int length = t.ActivePoints.Count;

			if (length == 0) return;

			Transform GetAtIndex(int index)
			{
				return t.ActivePoints[index];
			}

			float largestDistance = 0;
			float smallestDistance = Mathf.Infinity;
			float sum = 0;
			int count = 0;
			for (int i = 0; i < length; i++)
			{
				Transform anchor = GetAtIndex(i);

				if (!t.closed)
				{
					if (i == length - 1) continue;
				}

				float d = Vector3.Distance(anchor.position, GetAtIndex((i + 1) % length).position);
				if (d >= largestDistance)
					largestDistance = d;
				if (d <= smallestDistance)
					smallestDistance = d;

				sum += d;
				count++;
			}
			float average = sum / (float)count;

			float maxDeviation = Mathf.Max(Mathf.Abs(largestDistance - average), Mathf.Abs(smallestDistance - average));

			float minDeviation = average * .125f;

			for (int i = 0; i < (t.closed ? length : length - 1); i++)
			{
				Vector3 anchor = GetAtIndex(i).position;
				Vector3 next = GetAtIndex((i + 1) % length).position;

				float d = Vector3.Distance(anchor, next);

				float deviationFromAverage = Mathf.Abs(d - average);
				float _t = deviationFromAverage > minDeviation ? Mathf.InverseLerp(minDeviation, maxDeviation, deviationFromAverage - minDeviation) : 0;
				_t = Mathf.Pow(_t, .5f);

				Handles.color = Color.Lerp(Color.green, Color.red, _t);
				Handles.DrawLine(anchor, next);

				int c = length;

				int i0 = (i - 1 + c) % c;
				int i1 = i;
				int i2 = (i + 1) % c;
				int i3 = (i + 2) % c;

				Vector3 p0 = GetAtIndex(i0).position;
				Vector3 p1 = GetAtIndex(i1).position;
				Vector3 p2 = GetAtIndex(i2).position;
				Vector3 p3 = GetAtIndex(i3).position;

				Vector3 p1p0 = (p0 - p1).normalized;
				Vector3 p1p2 = (p2 - p1).normalized;
				Vector3 p2p3 = (p2 - p3).normalized;

				Vector3 t0 = p1 + (_curviness.floatValue * (p2 - p1).magnitude * (p1p2 - p1p0).normalized);
				Vector3 t1 = p2 + (_curviness.floatValue * (p2 - p3).magnitude * (p2p3 - p1p2).normalized);

				Handles.color = _curveColor.colorValue;
				Handles.DrawBezier(anchor, next, t0, t1, Color.white, null, _curveThickness.floatValue);
			}

			for (int i = 0; i < length; i++)
			{
				Transform anchor = GetAtIndex(i);

				int hash = GetHashCode() + i;

				EditorGUI.BeginChangeCheck();

				Handles.color = t.handlesColor;

				Undo.RecordObject(anchor, "Anchor Move");
#pragma warning disable 0618
				anchor.position = Handles.FreeMoveHandle(hash, anchor.position, Quaternion.identity, t.handlesSize, Vector3.one * HandleSnapMagnitude, Handles.SphereHandleCap);
#pragma warning restore 0618

				EditorGUI.EndChangeCheck();

				if (_showIndex.boolValue)
				{
					textStyle.normal.textColor = Color.Lerp(t.labelColor1, t.labelColor2, (float)i / (float)length);
					Handles.Label(anchor.position + Vector3.up * .25f, (i + 1).ToString(), textStyle);
				}
			}

			textStyle.normal.textColor = _showIndex.boolValue ? _curveColor.colorValue : _labelColor1.colorValue;
			Handles.Label(GetAtIndex(0).position + Vector3.up * 1, "START", textStyle);
		}


		private void FindProperties()
		{
			_pointsParent = serializedObject.FindProperty("pointsParent");
			_vfxParent = serializedObject.FindProperty("vfxParent");
			_closed = serializedObject.FindProperty("closed");
			_curviness = serializedObject.FindProperty("curviness");
			_radius = serializedObject.FindProperty("radius");
			_speed = serializedObject.FindProperty("speed");
			_followPathForce = serializedObject.FindProperty("followPathForce");
			_drag = serializedObject.FindProperty("drag");
			_showIndex = serializedObject.FindProperty("showIndex");
			_curveThickness = serializedObject.FindProperty("curveThickness");
			_curveColor = serializedObject.FindProperty("curveColor");
			_handlesSize = serializedObject.FindProperty("handlesSize");
			_handlesColor = serializedObject.FindProperty("handlesColor");
			_labelSize = serializedObject.FindProperty("labelSize");
			_labelColor1 = serializedObject.FindProperty("labelColor1");
			_labelColor2 = serializedObject.FindProperty("labelColor2");
		}


		private void Bind()
		{
			Curve t = target as Curve;

			foreach (var child in t.vfxParent.GetAllChildren())
			{
				if (child.TryGetComponent(out VFXPropertyBinder vfxBinder))
				{
					if (child.TryGetComponent(out VisualEffect vfx))
					{
						vfxBinder.RemovePropertyBinders<VFXTransformBinderExternal>();

						for (int i = 0; i < t.pointsParent.childCount; i++)
						{
							var tr = t.pointsParent.GetChild(i);

							var binder = vfxBinder.AddPropertyBinder<VFXTransformBinderExternal>();
							binder.Target = tr;
							binder.Property = $"Transform{i}";
						}
					}
				}
			}
		}


		private void Feedback(Curve t, string message, MessageType type)
		{
			void Run()
			{
				t.StopAllCoroutines();
				feedbackText = message;
				feedbackType = type;
				feedbackDisplaying = true;
				Repaint(); // Refresh the inspector to show the text
				t.StartCoroutine(RunFeedback());
			}

			EditorApplication.delayCall += Run;
		}

		private IEnumerator RunFeedback()
		{
			string txt = feedbackText;

			int t = 0;
			while (t < feedbackTime)
			{
				feedbackText = txt + $"\t[{feedbackTime - t - 1}]";
				Repaint(); // Refresh
				yield return new WaitForSeconds(1);
				t++;
			}

			feedbackText = "";
			feedbackDisplaying = false;
			Repaint(); // Refresh the inspector to hide the text
		}
	}
#endif

}