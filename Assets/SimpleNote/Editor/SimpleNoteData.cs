using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DI.SimpleNote {
	public class SimpleNoteData : ScriptableObject {

		public static SimpleNoteData Instance { get { return _instance ? _instance : CreateInstance(); } }
		public static string filePath = "Assets/SimpleNote/Resources/SimpleNoteData.asset";
		static SimpleNoteData _instance = null;
		static SimpleNoteData CreateInstance() {
#if UNITY_EDITOR
			if (!AssetDatabase.IsValidFolder("Assets/SimpleNote/Resources"))
				AssetDatabase.CreateFolder("Assets/SimpleNote", "Resources");
			_instance = AssetDatabase.LoadAssetAtPath(filePath, typeof(SimpleNoteData)) as SimpleNoteData;
			if (_instance == null)
			{
				_instance = ScriptableObject.CreateInstance<SimpleNoteData>();
				AssetDatabase.CreateAsset(Instance, filePath);
				AssetDatabase.SaveAssets();
			}
#else
			_instance = Resources.Load("SimpleNoteData.asset", typeof(SimpleNoteData)) as SimpleNoteData;
#endif
			return _instance;
		}

		public List<Note> notes = new List<Note>();
		public enum SimpleNoteSceneViewPosition {
			TopLeft = 0, BottomLeft = 1
		}

#if UNITY_EDITOR
		[PreferenceItem("Simple Note")]
		public static void SimpleNotePreference() {
			//Scene VIew Preference
			EditorGUILayout.LabelField("GameObject Note", EditorStyles.boldLabel);
			EditorGUI.indentLevel += 1;
			Vector2 offset = EditorGUILayout.Vector2Field("Note Offset", new Vector2(EditorPrefs.GetFloat("NoteOffsetX", 15), EditorPrefs.GetFloat("NoteOffsetY", 15)));
			if (offset != new Vector2(EditorPrefs.GetFloat("NoteOffsetX", 15), EditorPrefs.GetFloat("NoteOffsetY", 15)))
			{
				EditorPrefs.SetFloat("NoteOffsetX", offset.x);
				EditorPrefs.SetFloat("NoteOffsetY", offset.y);
			}
			SimpleNoteSceneViewPosition notePosition = (SimpleNoteSceneViewPosition)EditorGUILayout.EnumPopup("Note Position", ((SimpleNoteSceneViewPosition)EditorPrefs.GetInt("simpleNoteSceneViewPosition", 1)));
			if (((int)notePosition) != EditorPrefs.GetInt("simpleNoteSceneViewPosition", 1))
				EditorPrefs.SetInt("simpleNoteSceneViewPosition", ((int)notePosition));
			float noteWidth = EditorGUILayout.FloatField("Note Width", EditorPrefs.GetFloat("NoteWidthSceneView", 300));
			if (noteWidth != EditorPrefs.GetFloat("NoteWidthSceneView", 300))
				EditorPrefs.SetFloat("NoteWidthSceneView", noteWidth);

			//Toggle Show Note Manager
			/*bool showNoteManager = EditorGUILayout.Toggle("Show Note Manager", EditorPrefs.GetBool("ShowNoteManager", false));
			if (showNoteManager != EditorPrefs.GetBool("ShowNoteManager", false)) {
				EditorPrefs.SetBool("ShowNoteManager", showNoteManager);
				HideFlags currentHideflags = SimpleNoteManager.Instance.gameObject.hideFlags;
				if (showNoteManager)
					SimpleNoteManager.Instance.gameObject.hideFlags = HideFlags.None;
				else
					SimpleNoteManager.Instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
				EditorUtility.SetDirty(SimpleNoteManager.Instance);
			}*/

			EditorGUI.indentLevel -= 1;

			EditorGUILayout.Space();
			//Color Preference
			EditorGUILayout.LabelField("Color", EditorStyles.boldLabel);
			EditorGUI.indentLevel += 1;

			Color bgColor1, bgColor2, textColor;
			bgColor1 = EditorGUILayout.ColorField("Background Color 1", new Color(EditorPrefs.GetFloat("bgNoteColor1R", 63 / 255f), EditorPrefs.GetFloat("bgNoteColor1G", 63 / 255f), EditorPrefs.GetFloat("bgNoteColor1B", 63 / 255f), EditorPrefs.GetFloat("bgNoteColor1A", .95f)));
			bgColor2 = EditorGUILayout.ColorField("Background Color 2", new Color(EditorPrefs.GetFloat("bgNoteColor2R", 49 / 255f), EditorPrefs.GetFloat("bgNoteColor2G", 49 / 255f), EditorPrefs.GetFloat("bgNoteColor2B", 49 / 255f), EditorPrefs.GetFloat("bgNoteColor2A", .95f)));
			textColor = EditorGUILayout.ColorField("Text Color", new Color(EditorPrefs.GetFloat("textNoteColorR", 174 / 255f), EditorPrefs.GetFloat("textNoteColorG", 174 / 255f), EditorPrefs.GetFloat("textNoteColorB", 174 / 255f), EditorPrefs.GetFloat("textNoteColorA", 1)));

			if (bgColor1.r != EditorPrefs.GetFloat("bgNoteColor1R", 63 / 255f))
				EditorPrefs.SetFloat("bgNoteColor1R", bgColor1.r);
			if (bgColor1.g != EditorPrefs.GetFloat("bgNoteColor1G", 63 / 255f))
				EditorPrefs.SetFloat("bgNoteColor1G", bgColor1.g);
			if (bgColor1.b != EditorPrefs.GetFloat("bgNoteColor1B", 63 / 255f))
				EditorPrefs.SetFloat("bgNoteColor1B", bgColor1.b);
			if (bgColor1.a != EditorPrefs.GetFloat("bgNoteColor1A", .95f))
				EditorPrefs.SetFloat("bgNoteColor1A", bgColor1.a);


			if (bgColor2.r != EditorPrefs.GetFloat("bgNoteColor2R", 49 / 255f))
				EditorPrefs.SetFloat("bgNoteColor2R", bgColor2.r);
			if (bgColor2.g != EditorPrefs.GetFloat("bgNoteColor2G", 49 / 255f))
				EditorPrefs.SetFloat("bgNoteColor2G", bgColor2.g);
			if (bgColor2.b != EditorPrefs.GetFloat("bgNoteColor2B", 49 / 255f))
				EditorPrefs.SetFloat("bgNoteColor2B", bgColor2.b);
			if (bgColor2.a != EditorPrefs.GetFloat("bgNoteColor2A", .5f))
				EditorPrefs.SetFloat("bgNoteColor2A", bgColor2.a);


			if (textColor.r != EditorPrefs.GetFloat("textNoteColo2R", 174 / 255f))
				EditorPrefs.SetFloat("textNoteColorR", textColor.r);
			if (textColor.g != EditorPrefs.GetFloat("textNoteColorG", 174 / 255f))
				EditorPrefs.SetFloat("textNoteColorG", textColor.g);
			if (textColor.b != EditorPrefs.GetFloat("textNoteColorB", 174 / 255f))
				EditorPrefs.SetFloat("textNoteColorB", textColor.b);
			if (textColor.a != EditorPrefs.GetFloat("textNoteColorA", 1))
				EditorPrefs.SetFloat("textNoteColorA", textColor.a);

			if(GUILayout.Button("Reset Color")){
				EditorPrefs.SetFloat("bgNoteColor1R", 63 / 255f); EditorPrefs.SetFloat("bgNoteColor1G", 63 / 255f); EditorPrefs.SetFloat("bgNoteColor1B", 63 / 255f); EditorPrefs.SetFloat("bgNoteColor1A", .95f);
				EditorPrefs.SetFloat("bgNoteColor2R", 49 / 255f); EditorPrefs.SetFloat("bgNoteColor2G", 49 / 255f); EditorPrefs.SetFloat("bgNoteColor2B", 49 / 255f); EditorPrefs.SetFloat("bgNoteColor2A", .95f);
				EditorPrefs.SetFloat("textNoteColorR", 174 / 255f); EditorPrefs.SetFloat("textNoteColorG", 174 / 255f); EditorPrefs.SetFloat("textNoteColorB", 174 / 255f); EditorPrefs.SetFloat("textNoteColorA", 1);
			}


			EditorGUI.indentLevel -= 1;

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Global Note", EditorStyles.boldLabel);
			EditorGUI.indentLevel += 1;
			bool colorize = EditorGUILayout.Toggle("Colorize", EditorPrefs.GetBool("ColorizeNote", true));
			if (EditorPrefs.GetBool("ColorizeNote", true) != colorize)
				EditorPrefs.SetBool("ColorizeNote", colorize);


			EditorGUI.indentLevel -= 1;


		}

		public Color getTextColor { get { return new Color(EditorPrefs.GetFloat("textNoteColorR", 174 / 255f), EditorPrefs.GetFloat("textNoteColorG", 174 / 255f), EditorPrefs.GetFloat("textNoteColorB", 174 / 255f), EditorPrefs.GetFloat("textNoteColorA", 1)); } }
		public Color getBgColor1 { get { return new Color(EditorPrefs.GetFloat("bgNoteColor1R", 63 / 255f), EditorPrefs.GetFloat("bgNoteColor1G", 63 / 255f), EditorPrefs.GetFloat("bgNoteColor1B", 63 / 255f), EditorPrefs.GetFloat("bgNoteColor1A", .95f)); } }
		public Color getBgColor2 { get { return new Color(EditorPrefs.GetFloat("bgNoteColor2R", 49 / 255f), EditorPrefs.GetFloat("bgNoteColor2G", 49 / 255f), EditorPrefs.GetFloat("bgNoteColor2B", 49 / 255f), EditorPrefs.GetFloat("bgNoteColor2A", .95f)); } }
#endif

	}

}
