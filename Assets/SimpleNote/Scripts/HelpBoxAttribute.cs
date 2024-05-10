using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DI.SimpleNote {
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class HelpBoxAttribute : PropertyAttribute {

		public string note;
		public MessageType messageType = MessageType.None;

		public HelpBoxAttribute(string note) {
			this.note = note;
		}
		public HelpBoxAttribute(string note, MessageType messageType) {
			this.note = note;
			this.messageType = messageType;
		}
	}
	
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(HelpBoxAttribute))]
	public class HelpBoxDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Rect helpBoxRect = new Rect(position.x, position.y, position.width, EditorStyles.helpBox.CalcHeight(new GUIContent(((HelpBoxAttribute)attribute).note), position.width));
			Rect propertyRect = position;
			propertyRect.y = position.y + helpBoxRect.height + EditorGUIUtility.singleLineHeight * .2f;
			propertyRect.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.HelpBox(helpBoxRect, ((HelpBoxAttribute)attribute).note, (UnityEditor.MessageType)((int)((HelpBoxAttribute)attribute).messageType));
			EditorGUI.PropertyField(propertyRect, property);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (EditorGUIUtility.singleLineHeight + EditorStyles.helpBox.CalcHeight(new GUIContent(((HelpBoxAttribute)attribute).note), EditorGUIUtility.currentViewWidth)) + EditorGUIUtility.standardVerticalSpacing * 2;
		}
	}
#endif
}
