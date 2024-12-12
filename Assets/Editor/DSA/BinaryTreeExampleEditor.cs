using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Web;

namespace DSA
{
    [CustomEditor(typeof(BinaryTreeExample))]
    public class BinaryTreeExampleEditor : Editor
    {
        private Gradient    m_valueColors;
        private int         m_iValueToAdd;
        private int         m_iValueToFind;
        
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

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            BinaryTreeExample te = target as BinaryTreeExample;
            BinarySearchTree bst = te.m_tree as BinarySearchTree;
            if (bst != null)
            {
                GUILayout.Space(10);
                m_iValueToAdd = EditorGUILayout.IntField("Value", m_iValueToAdd);
                if (GUILayout.Button("Add", EditorStyles.miniButton))
                {
                    bst.Add(m_iValueToAdd);
                    SceneView.RepaintAll();
                }
                
                GUILayout.Space(10);
                m_iValueToFind = EditorGUILayout.IntField("Enter Value to be Found", m_iValueToAdd);
                if (GUILayout.Button("Find", EditorStyles.miniButton))
                { 
                    Debug.Log(bst.Find(m_iValueToFind));
                    SceneView.RepaintAll();
                }
            }
        }

        private void OnSceneGUI()
        {
            // draw binary tree
            Tools.current = Tool.None;
            BinaryTreeExample te = target as BinaryTreeExample;
            //DSAEditorUtils.DrawBinaryTree(te.m_tree, TraverseCallback, (int i) => i.ToString(), NodeDrawingCallback);
            DSAEditorUtils.DrawBinaryTree(te.m_tree, TraverseCallback, (int i) => i.ToString(), IsNodeBalanced_Callback);

            // traversed list
            if (te.m_traversedNodes != null && te.m_traversedNodes.Count > 0)
            {
                Handles.matrix = Matrix4x4.Translate(new Vector3(te.m_traversedNodes.Count * -0.5f, 3.0f, 0.0f));
                DSAEditorUtils.DrawArray(te.m_traversedNodes.ConvertAll(n => n.m_value).ToArray(),
                                         NodeColorCallback,
                                         (int i) => i.ToString(), 
                                         false);
                Handles.matrix = Matrix4x4.identity;
            }
        }

        void NodeDrawingCallback(BinaryTree<int>.Node node, Rect rect)
        {
            BinaryTreeExample te = target as BinaryTreeExample;

            // in stack?
            if (te.CurrentNode == node || 
                te.m_nodeStack.Contains(node))
            {
                DSAEditorUtils.DrawElement(node.m_value, rect, 0.0f, node == te.CurrentNode ? new Color(1.0f, 0.5f, 0.0f) : Color.yellow, node.m_value.ToString(), null, false);
            }

            // traverse index
            int iIndex = te.m_traversedNodes.IndexOf(node);
            if (iIndex >= 0)
            {
                DSAEditorUtils.DrawTextAt((iIndex + 1).ToString(), new Vector3(rect.center.x, rect.yMax + 0.2f, 0.0f), 16.0f, Color.yellow, TextAnchor.MiddleCenter);
            }

            // delete node?
            #if true
            Handles.color = new Color(1.0f, 1.0f, 1.0f, 0.001f);
            if (te.m_tree is BinarySearchTree bst && Handles.Button(new Vector3(rect.center.x, rect.center.y, 0.0f), Quaternion.identity, 1.0f, 1.0f, Handles.CubeHandleCap))
            {
                bst.Remove(node.m_value);
            }
            #endif
        }

        Color TraverseCallback(int elem, int iIndex)
        {
            BinaryTreeExample te = target as BinaryTreeExample;
            if (te.IsTraversing)
            {
                return Color.gray;
            }

            return NodeColorCallback(elem, iIndex);
        }

        Color NodeColorCallback(int elem, int iIndex)
        {
            float f = elem / 64.0f;
            return m_valueColors.Evaluate(f);
        }

        void IsNodeBalanced_Callback(BinaryTree<int>.Node node, Rect rect)
        {
            #if true
            bool bBalanced = BinaryTree<int>.IsNodeBalanced(node);
            //string title = bBalanced ? "Balanced" : "Not Balanced";
            string title = BinaryTree<int>.GetBalance(node).ToString();
            DSAEditorUtils.DrawTextAt(title,
                                      new Vector3(rect.center.x, rect.yMax + 0.2f, 0.0f),
                                      16,
                                      bBalanced ? new Color(0.2f, 1.0f, 0.2f) : new Color(1.0f, 0.2f, 0.2f),
                                      TextAnchor.LowerCenter);
            #endif

            // Left click = Left Rotate
            // CTRL + Left click = Right Rotate
            #if true
            Handles.color = new Color(1.0f, 1.0f, 1.0f, 0.001f);
            if (Handles.Button(new Vector3(rect.center.x, rect.center.y, 0.0f), Quaternion.identity, 1.0f, 1.0f, Handles.CubeHandleCap))
            {
                BinaryTreeExample bte = target as BinaryTreeExample;
                BinaryTree<int>.Node parent = FindParent(node, bte.m_tree.m_root);                
                bool bRight = Event.current.control;

                if (parent != null)
                {
                    if (parent.m_left == node)
                    {
                        parent.m_left = bRight ? AVLTree.RotateRight(node) : AVLTree.RotateLeft(node);
                    }
                    else
                    {
                        parent.m_right = bRight ? AVLTree.RotateRight(node) : AVLTree.RotateLeft(node);
                    }
                }
                else if (node == bte.m_tree.m_root)
                {
                    bte.m_tree.m_root = bRight ? AVLTree.RotateRight(node) : AVLTree.RotateLeft(node);
                }
            }
            #endif
        }

        BinaryTree<int>.Node FindParent(BinaryTree<int>.Node node, BinaryTree<int>.Node currentNode)
        {
            if (currentNode == null)
            {
                return null;
            }

            if (currentNode.m_left == node ||
                currentNode.m_right == node)
            {
                return currentNode;
            }

            BinaryTree<int>.Node parent = FindParent(node, currentNode.m_left);
            if (parent != null) return parent;

            parent = FindParent(node, currentNode.m_right);
            if (parent != null) return parent;

            return null;
        }
    }
}