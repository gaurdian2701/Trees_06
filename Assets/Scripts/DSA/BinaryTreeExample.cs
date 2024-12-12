using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DSA
{
    [ExecuteInEditMode]
    public class BinaryTreeExample : MonoBehaviour
    {
        public BinaryTree<int>              m_tree;
        public List<BinaryTree<int>.Node>   m_traversedNodes = new List<BinaryTree<int>.Node>();
        public Stack<BinaryTree<int>.Node>  m_nodeStack = new Stack<BinaryTree<int>.Node>();

        #region Properties

        public bool IsTraversing { get; private set; }

        public BinaryTree<int>.Node CurrentNode { get; private set; }

        #endregion

        private void OnEnable()
        {
            // CreateRandomTree();            
            CreateBinarySearchTree();
            // CreateAVLTree();
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(TraverseTree());
            }
        }

        protected void CreateRandomTree()
        {
            m_tree = new BinaryTree<int>();
            m_tree.m_root = CreateRandomNode(0, 4);
        }

        protected void CreateBinarySearchTree()
        {
            m_tree = new BinarySearchTree();
            int[] values = new int[] { 21, 15, -3, 37, 8, 22, 44, 55, 10, 11, 64, 17, -32, 48 };
            foreach (int iValue in values)
            {
                (m_tree as BinarySearchTree).Add(iValue);
            }
        }

        protected void CreateAVLTree()
        {
            m_tree = new AVLTree();
            for (int i = 0; i < 100; ++i)
            {
                (m_tree as BinarySearchTree).Add(Random.Range(0, 16));
            }
        }

        protected BinaryTree<int>.Node CreateRandomNode(int iDepth, int iMaxDepth)
        {
            // create node
            BinaryTree<int>.Node node = new BinaryTree<int>.Node { m_value = Random.Range(0, 99) };

            // create children?
            if (iDepth < iMaxDepth)
            {
                if (Random.value < 0.66f || iDepth < 2) node.m_left = CreateRandomNode(iDepth + 1, iMaxDepth);
                if (Random.value < 0.66f || iDepth < 2) node.m_right = CreateRandomNode(iDepth + 1, iMaxDepth);
            }

            return node;
        }

        IEnumerator TraverseTree()
        {
            IsTraversing = true;
            m_nodeStack = new Stack<BinaryTree<int>.Node>();
            CurrentNode = null;
            yield return new WaitForSeconds(2.0f);
            yield return TraverseTree_InOrder(m_tree.m_root);
            //yield return TraverseTree_PreOrder(m_tree.m_root);
            //yield return TraverseTree_PostOrder(m_tree.m_root);

            IsTraversing = false;
        }

        IEnumerator TraverseTree_InOrder(BinaryTree<int>.Node node)
        {
            if (node != null)
            {
                CurrentNode = node;
                m_nodeStack.Push(node);
                yield return new WaitForSeconds(0.7f);

                yield return TraverseTree_InOrder(node.m_left);

                CurrentNode = node;
                m_traversedNodes.Add(node);
                m_nodeStack.Pop();
                yield return new WaitForSeconds(0.7f);

                yield return TraverseTree_InOrder(node.m_right);

                CurrentNode = node;
                yield return new WaitForSeconds(0.7f);
            }
        }

        IEnumerator TraverseTree_PreOrder(BinaryTree<int>.Node node)
        {
            if (node != null)
            {
                CurrentNode = node;
                m_nodeStack.Push(node);
                m_traversedNodes.Add(node);
                m_nodeStack.Pop();
                yield return new WaitForSeconds(0.7f);

                yield return TraverseTree_PreOrder(node.m_left);

                CurrentNode = node;
                yield return new WaitForSeconds(0.7f);

                yield return TraverseTree_PreOrder(node.m_right);

                CurrentNode = node;
                yield return new WaitForSeconds(0.7f);
            }
        }

        IEnumerator TraverseTree_PostOrder(BinaryTree<int>.Node node)
        {
            if (node != null)
            {
                CurrentNode = node;
                m_nodeStack.Push(node);
                yield return new WaitForSeconds(0.7f);

                yield return TraverseTree_PostOrder(node.m_right);

                CurrentNode = node;
                m_traversedNodes.Add(node);
                m_nodeStack.Pop();
                Debug.Log(node.m_value.ToString());
                yield return new WaitForSeconds(0.7f);

                yield return TraverseTree_PostOrder(node.m_left);

                CurrentNode = node;
                yield return new WaitForSeconds(0.7f);
            }
        }
    }
}