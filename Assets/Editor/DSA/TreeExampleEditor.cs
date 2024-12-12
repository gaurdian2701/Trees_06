using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DSA
{
    [CustomEditor(typeof(TreeExample))]
    public class TreeExampleEditor : Editor
    {
        private Gradient m_valueColors;

        private void OnEnable()
        {
            m_valueColors = new Gradient();
            m_valueColors.colorKeys = new GradientColorKey[]{
                new GradientColorKey(Color.red, 0.0f),
                new GradientColorKey(new Color(1.0f, 0.5f, 0.0f), 0.2f),
                new GradientColorKey(Color.yellow, 0.4f),
                new GradientColorKey(Color.green, 0.6f),
                new GradientColorKey(Color.blue, 0.8f),
                new GradientColorKey(new Color(1.0f, 0.0f, 1.0f), 1.0f)
            };
        }

        private void OnSceneGUI()
        {
            Tools.current = Tool.None;
            TreeExample te = target as TreeExample;
            DSAEditorUtils.DrawTree(te.m_tree, NodeColorCallback, (int i) => i.ToString(), NodeDrawingCallback);
        }

        void NodeDrawingCallback(Tree<int>.Node node, Rect rect)
        {
            TreeExample te = target as TreeExample;

            // in stack?
            if (te.CurrentNode == node || te.m_nodeStack.Contains(node))
            {
                DSAEditorUtils.DrawElement(node.m_value, rect, 0.0f, node == te.CurrentNode ? new Color(1.0f, 0.5f, 0.0f) : Color.yellow, node.m_value.ToString(), null, false);
            }

            // traverse index
            int iIndex = te.m_traversedNodes.IndexOf(node);
            if (iIndex >= 0)
            {
                DSAEditorUtils.DrawTextAt((iIndex + 1).ToString(), new Vector3(rect.center.x, rect.yMax + 0.2f, 0.0f), 16.0f, Color.yellow, TextAnchor.MiddleCenter);
            }
        }

        Color NodeColorCallback(int elem, int iIndex)
        {
            TreeExample te = target as TreeExample;

            if (te.IsTraversing)
            {
                return Color.gray;
            }

            float f = elem / 64.0f; 
            return m_valueColors.Evaluate(f);
        }
    }
}