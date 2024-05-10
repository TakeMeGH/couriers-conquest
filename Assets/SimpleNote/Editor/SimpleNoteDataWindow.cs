using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace DI.SimpleNote {
	public class SimpleNoteDataWindow : EditorWindow
	{
		List<int> hideNoteIndex = new List<int>();

		[MenuItem("Window/Simple Note/Show Notes")]
		static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(SimpleNoteDataWindow), false, "Simple Note");
			
		}
		
		void OnInspectorUpdate() {
			Repaint();
		}

		Vector3 scrollPos;
		void OnGUI()
		{
			EditorGUI.BeginChangeCheck();
			int index = 0;

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add a Note", new GUILayoutOption[] { GUILayout.Height(25) }))
			{
				SimpleNoteData.Instance.notes.Add(new Note("Title", "Note Here"));
			}
			EditorGUILayout.EndHorizontal();
			Rect lastRect = GUILayoutUtility.GetLastRect();
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			//scrollPos = GUILayout.BeginScrollView(scrollPos, GUIStyle.none, GUI.skin.verticalScrollbar);

			GUIStyleState textFieldNormal = new GUIStyleState();
			textFieldNormal.background = EditorStyles.textField.normal.background;
			textFieldNormal.textColor = EditorStyles.textField.normal.textColor;
			GUIStyleState textAreaNormal = EditorStyles.textArea.normal;

			lastRect.y -= EditorGUIUtility.singleLineHeight;
			EditorGUILayout.Space();

			foreach (Note note in SimpleNoteData.Instance.notes)
			{
				EditorStyles.textField.wordWrap = true;
				EditorStyles.textField.wordWrap = false;

				Color guiBgOriColor = GUI.backgroundColor;
				GUIStyle textFieldStyle = new GUIStyle(EditorStyles.textField);
				textFieldStyle.fontStyle = FontStyle.Bold;
				textFieldStyle.normal = EditorStyles.label.normal;
				textFieldStyle.focused = EditorStyles.label.normal;

				if (EditorPrefs.GetBool("ColorizeNote", true))
				{
					Vector2 bgTitleSize = textFieldStyle.CalcSize(new GUIContent(note.title));
					Rect bgTitleRect = GUILayoutUtility.GetLastRect();
					bgTitleRect.y += EditorGUIUtility.singleLineHeight/2;
					bgTitleRect.height = bgTitleSize.y;
					EditorGUI.DrawRect(bgTitleRect, SimpleNoteData.Instance.getBgColor1);

					textFieldStyle.normal.textColor = SimpleNoteData.Instance.getTextColor; 
					textFieldStyle.focused.textColor = SimpleNoteData.Instance.getTextColor;
				}
				
				EditorGUILayout.BeginHorizontal();
				Undo.RecordObject(SimpleNoteData.Instance, "Undo Title");
				note.title = EditorGUILayout.TextField(note.title, textFieldStyle);

				//if(!string.IsNullOrEmpty(lastControlFocus) && lastControlFocus != "TitleEdit" + index.ToString())
				//	GUI.FocusControl(null);
				/*
				if (GUI.GetNameOfFocusedControl() == "TitleEdit" + index.ToString()) {
					if (GUILayout.Button("Save", GUILayout.Width(45), GUILayout.Height(15)))
						GUI.FocusControl(null);
				}*/
				if(index != 0)
				{
					if (GUILayout.Button("\u2191", GUILayout.Width(15), GUILayout.Height(15)))
					{
						Undo.RecordObject(SimpleNoteData.Instance, "Swap Note");
						SimpleNoteData.Instance.notes[index] = SimpleNoteData.Instance.notes[index - 1];
						SimpleNoteData.Instance.notes[index - 1] = note;
						break;
					}
				}
				if (index != SimpleNoteData.Instance.notes.Count - 1)
				{
					if (GUILayout.Button("\u2193", GUILayout.Width(15), GUILayout.Height(15)))
					{
						Undo.RecordObject(SimpleNoteData.Instance, "Swap Note");
						SimpleNoteData.Instance.notes[index] = SimpleNoteData.Instance.notes[index + 1];
						SimpleNoteData.Instance.notes[index + 1] = note;
						break;
					}
				}
				else {
					GUILayout.Label("", GUILayout.Width(15));
				}
				if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15)))
				{
					if (hideNoteIndex.Contains(index))
						hideNoteIndex.Remove(index);
					else
						hideNoteIndex.Add(index);
				}

				if (GUILayout.Button("x", GUILayout.Width(15), GUILayout.Height(15)))
				{
					Undo.RecordObject(SimpleNoteData.Instance, "Delete Note");
					SimpleNoteData.Instance.notes.Remove(note);
					if (hideNoteIndex.Contains(index))
						hideNoteIndex.Remove(index);
					EditorUtility.SetDirty(SimpleNoteData.Instance);
					break;
				}

				EditorGUILayout.EndHorizontal();

				if (hideNoteIndex.Contains(index))
				{
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					index += 1;
					continue;
				}
				textFieldStyle.normal = textFieldNormal;
				textFieldStyle.fontStyle = FontStyle.Normal;

				GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
				textAreaStyle.richText = true;
				textAreaStyle.fontStyle = FontStyle.Normal;

				textAreaStyle.normal = EditorStyles.label.normal;
				textAreaStyle.focused = EditorStyles.label.normal;

				EditorGUILayout.BeginHorizontal();
				
				Undo.RecordObject(SimpleNoteData.Instance, "Undo Note");
				note.note = EditorGUILayout.TextArea(note.note, textAreaStyle);
				/*
				if (GUI.GetNameOfFocusedControl() == "NoteEdit" + index.ToString())
				{
					Debug.Log(index);
					if (GUILayout.Button("Save", GUILayout.Width(45)))
						GUI.FocusControl(null);
				}*/

				EditorGUILayout.EndHorizontal();

				textAreaStyle.richText = false;
				textAreaStyle.normal = textAreaNormal;

				if (EditorPrefs.GetBool("ColorizeNote", false))
					GUI.backgroundColor = guiBgOriColor;
				
				EditorGUILayout.Space();
				EditorGUILayout.Space();

				index += 1;
			}

			EditorGUILayout.EndScrollView();

			if (EditorWindow.focusedWindow != this)
				GUI.FocusControl(null);

			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(SimpleNoteData.Instance);
			}

		}
	}
}
