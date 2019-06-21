using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
Source:
	https://github.com/Unity-Technologies/2d-extras

License:

	MIT License

	Copyright (c) 2016 Unity Technologies

	Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

 */

namespace UnityEditor
{
	[CreateAssetMenu(fileName = "Prefab brush", menuName = "Brushes/Prefab brush")]
	[CustomGridBrush(false, true, false, "Prefab Brush")]
	public class PrefabBrush : GridBrush
	{
		public GameObject[] m_Prefabs;
		public int m_Index = 0;
		public int m_Z;
		private GameObject prev_brushTarget;
		private Vector3Int prev_position;

		public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			Transform itemInCell = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, m_Z));
			if (itemInCell != null) return;
			if (position == prev_position)
			{
				return;
			}
			prev_position = position;
			if (brushTarget)
			{
				prev_brushTarget = brushTarget;
			}
			brushTarget = prev_brushTarget;

			// Do not allow editing palettes
			if (brushTarget.layer == 31)
				return;

			int index = Mathf.Clamp(m_Index, 0, m_Prefabs.Length - 1);
			GameObject prefab = m_Prefabs[index];
			GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
			if (instance != null)
			{
				Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
				Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");
				instance.transform.SetParent(brushTarget.transform);
				instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, m_Z) + new Vector3(.5f, .5f, .5f)));
			}
		}

		public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			if (brushTarget)
			{
				prev_brushTarget = brushTarget;
			}
			brushTarget = prev_brushTarget;
			// Do not allow editing palettes
			if (brushTarget.layer == 31)
				return;

			Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, m_Z));
			if (erased != null)
				Undo.DestroyObjectImmediate(erased.gameObject);
		}

		private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
		{
			int childCount = parent.childCount;
			Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
			Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
			Bounds bounds = new Bounds((max + min) * .5f, max - min);

			for (int i = 0; i < childCount; i++)
			{
				Transform child = parent.GetChild(i);
				if (bounds.Contains(child.position))
					return child;
			}
			return null;
		}

		private static float GetPerlinValue(Vector3Int position, float scale, float offset)
		{
			return Mathf.PerlinNoise((position.x + offset) * scale, (position.y + offset) * scale);
		}
	}

	[CustomEditor(typeof(PrefabBrush))]
	public class PrefabBrushEditor : GridBrushEditor
	{
		private PrefabBrush prefabBrush { get { return target as PrefabBrush; } }

		private SerializedProperty m_Prefabs;
		private SerializedObject m_SerializedObject;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_SerializedObject = new SerializedObject(target);
			m_Prefabs = m_SerializedObject.FindProperty("m_Prefabs");
		}

		public override void OnPaintInspectorGUI()
		{
			m_SerializedObject.UpdateIfRequiredOrScript();
			prefabBrush.m_Index = EditorGUILayout.IntSlider("Index to spawn", prefabBrush.m_Index, 0, prefabBrush.m_Prefabs.Length - 1);
			prefabBrush.m_Z = EditorGUILayout.IntField("Position Z", prefabBrush.m_Z);

			EditorGUILayout.PropertyField(m_Prefabs, true);
			m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
		}
	}
}
