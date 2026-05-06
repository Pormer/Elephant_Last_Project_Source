using System;
using System.Collections.Generic;
using Code.UI.Visual.Style;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Editor
{
    public class ColorModifier : EditorWindow
    {
        [SerializeField] private GameObject root;
        [SerializeField] private List<ColorModifierData> colorModifiers = new List<ColorModifierData>();
        private Dictionary<Color, ColorModifierData> _colorModifierDict = new Dictionary<Color, ColorModifierData>();
        private HashSet<Color> _colors = new HashSet<Color>();
        private readonly Dictionary<Graphic, Color> _originalGraphicColors = new Dictionary<Graphic, Color>();
        private readonly Dictionary<GraphicStyle, List<Color>> _originalStyleColors = new Dictionary<GraphicStyle, List<Color>>();

        [MenuItem("Tools/UI/Color Modifier")]
        public static void Open()
        {
            GetWindow<ColorModifier>("Color Modifier");
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            var newRoot = (GameObject)EditorGUILayout.ObjectField("Root", root, typeof(GameObject), true);
            if (EditorGUI.EndChangeCheck())
            {
                root = newRoot;
                ClearState();
            }

            using (new EditorGUI.DisabledScope(root == null))
            {
                if (GUILayout.Button("Load all colors"))
                    LoadAllColors();

                if (GUILayout.Button("Modify all colors"))
                    ModifyAllColors();
            }

            using (new EditorGUI.DisabledScope(!HasRollbackData()))
            {
                if (GUILayout.Button("Rollback all colors"))
                    RollbackAllColors();
            }

            EditorGUILayout.Space();

            foreach (var cm in colorModifiers)
            {
                EditorGUILayout.BeginHorizontal();
                var oldLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 30f;
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.ColorField("From", cm.previousColor);
                }
                cm.modifiedColor = EditorGUILayout.ColorField("To", cm.modifiedColor);
                EditorGUIUtility.labelWidth = oldLabelWidth;
                EditorGUILayout.EndHorizontal();
            }
        }

        #region Loading

        public void LoadAllColors()
        {
            if (root == null)
                return;

            var graphics = root.GetComponentsInChildren<Graphic>(true);
            var graphicStyles = root.GetComponentsInChildren<GraphicStyle>(true);

            ClearState();
            CacheOriginalColors(graphics, graphicStyles);

            foreach (var g in graphics)
                GetColorFromGraphics(g);
            foreach (var g in graphicStyles)
                GetColorFromGraphicStyle(g);
        }

        private void GetColorFromGraphics(Graphic target)
        {
            var color = target.color;
            color.a = 1;
            if (_colors.Add(color))
            {
                var newModifier = GetNewColorModifier(color);
                newModifier.colorGraphics.Add(target);
            }
            else
            {
                _colorModifierDict[color].colorGraphics.Add(target);
            }
        }

        private void GetColorFromGraphicStyle(GraphicStyle target)
        {
            foreach (var st in target.GetStyles())
            {
                var color = st.Color;
                color.a = 1;
                if (_colors.Add(color))
                {
                    var newModifier = GetNewColorModifier(color);
                    newModifier.colorStyles.Add(target);
                }
                else
                {
                    _colorModifierDict[color].colorStyles.Add(target);
                }
            }
        }

        private ColorModifierData GetNewColorModifier(Color color)
        {
            var res = new ColorModifierData
            {
                previousColor = color,
                modifiedColor = color,
                colorGraphics = new List<Graphic>(),
                colorStyles = new List<GraphicStyle>()
            };
            colorModifiers.Add(res);
            _colorModifierDict.Add(color, res);

            return res;
        }

        #endregion

        #region Modifying

        public void ModifyAllColors()
        {
            foreach (var cm in colorModifiers)
                ModifyColor(cm);
        }

        public void RollbackAllColors()
        {
            foreach (var entry in _originalGraphicColors)
            {
                if (entry.Key == null)
                    continue;

                Undo.RecordObject(entry.Key, "Rollback UI Color");
                entry.Key.color = entry.Value;
                EditorUtility.SetDirty(entry.Key);
            }

            foreach (var entry in _originalStyleColors)
            {
                if (entry.Key == null)
                    continue;

                Undo.RecordObject(entry.Key, "Rollback UI Style Color");
                var styles = entry.Key.GetStyles();
                var count = Mathf.Min(styles.Count, entry.Value.Count);
                for (var i = 0; i < count; i++)
                    styles[i].Color = entry.Value[i];
                EditorUtility.SetDirty(entry.Key);
            }

            LoadAllColors();
        }

        private void ModifyColor(ColorModifierData cm)
        {
            foreach (var g in cm.colorGraphics)
            {
                if (g == null)
                    continue;

                Undo.RecordObject(g, "Modify UI Color");
                var a = g.color.a;
                var tarColor = cm.modifiedColor;
                tarColor.a = a;
                g.color = tarColor;
                EditorUtility.SetDirty(g);
            }

            foreach (var g in cm.colorStyles)
            {
                if (g == null)
                    continue;

                Undo.RecordObject(g, "Modify UI Style Color");
                var sts = g.GetStyles().FindAll(s => s.Color == cm.previousColor);
                foreach (var st in sts)
                {
                    var a = st.Color.a;
                    var tarColor = cm.modifiedColor;
                    tarColor.a = a;
                    st.Color = tarColor;
                }
                EditorUtility.SetDirty(g);
            }

            cm.previousColor = cm.modifiedColor;
        }

        private void ClearState()
        {
            colorModifiers.Clear();
            _colorModifierDict.Clear();
            _colors.Clear();
            _originalGraphicColors.Clear();
            _originalStyleColors.Clear();
        }

        private void CacheOriginalColors(Graphic[] graphics, GraphicStyle[] graphicStyles)
        {
            foreach (var g in graphics)
            {
                if (g != null && !_originalGraphicColors.ContainsKey(g))
                    _originalGraphicColors.Add(g, g.color);
            }

            foreach (var gs in graphicStyles)
            {
                if (gs == null || _originalStyleColors.ContainsKey(gs))
                    continue;

                var colors = new List<Color>();
                foreach (var style in gs.GetStyles())
                    colors.Add(style.Color);
                _originalStyleColors.Add(gs, colors);
            }
        }

        private bool HasRollbackData()
        {
            return _originalGraphicColors.Count > 0 || _originalStyleColors.Count > 0;
        }

        #endregion
    }

    [Serializable]
    public class ColorModifierData
    {
        public Color previousColor;
        public Color modifiedColor;
        [HideInInspector] public List<Graphic> colorGraphics;
        [HideInInspector] public List<GraphicStyle> colorStyles;
    }
}
