using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DSA
{
    [ExecuteInEditMode]
    public class TreeExample : MonoBehaviour
    {
        public class WaitForKey : CustomYieldInstruction
        {
            static float m_fLastKeyPress = -1.0f;

            public override bool keepWaiting
            {
                get
                {
                    if (Time.unscaledTime - m_fLastKeyPress > 0.3f && (Input.GetKeyDown(KeyCode.Space)))
                    {
                        m_fLastKeyPress = Time.unscaledTime;
                        return false;
                    }

                    return true;
                }
            }
        }

        public Tree<int>                m_tree;
        public List<Tree<int>.Node>     m_traversedNodes = new List<Tree<int>.Node>();
        public Stack<Tree<int>.Node>    m_nodeStack = new Stack<Tree<int>.Node>();

        #region Properties

        public bool IsTraversing { get; private set; }

        public Tree<int>.Node CurrentNode { get; private set; }

        #endregion

        private void OnEnable()
        {
            CreateRandomTree();            
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
            m_tree = new Tree<int>();
            m_tree.m_root = CreateRandomNode(0, 4, 4);
        }

        protected Tree<int>.Node CreateRandomNode(int iDepth, int iMaxDepth, int iMaxChildren)
        {
            // create node
            Tree<int>.Node node = new Tree<int>.Node { m_value = Random.Range(0, 99) };

            // create children?
            if (iDepth < iMaxDepth)
            {
                int iNumChildren = Random.Range(0, iMaxChildren);
                if (iNumChildren > 0)
                {
                    node.m_children = new Tree<int>.Node[iNumChildren];
                    for (int i = 0; i < iNumChildren; i++)
                    {
                        node.m_children[i] = CreateRandomNode(iDepth + 1, iMaxDepth, iMaxChildren);
                    }
                }
            }

            return node;
        }

        IEnumerator TraverseTree()
        {
            IsTraversing = true;
            m_nodeStack = new Stack<Tree<int>.Node>();
            CurrentNode = null;
            yield return new WaitForSeconds(2.0f);
            //yield return TraverseTree_InOrder(m_tree.m_root);
            yield return TraverseTree_LevelOrder(m_tree.m_root);

            IsTraversing = false;
        }

        IEnumerator TraverseTree_InOrder(Tree<int>.Node node)
        {
            CurrentNode = node;
            m_nodeStack.Push(node);
            yield return new WaitForSeconds(1.0f);

            foreach (Tree<int>.Node child in node.Children)
            {
                yield return TraverseTree_InOrder(child);
            }

            yield return new WaitForSeconds(1.0f);
            m_traversedNodes.Add(node);
            m_nodeStack.Pop();
        }

        IEnumerator TraverseTree_LevelOrder(Tree<int>.Node node)
        {
            Queue<Tree<int>.Node> nodeQueue = new Queue<Tree<int>.Node>();
            nodeQueue.Enqueue(node);

            while (nodeQueue.Count > 0)
            {
                CurrentNode = nodeQueue.Dequeue();
                m_nodeStack.Push(CurrentNode);
                m_traversedNodes.Add(CurrentNode);
                yield return new WaitForSeconds(1.0f);

                foreach (Tree<int>.Node child in CurrentNode.Children)
                {
                    nodeQueue.Enqueue(child);
                }
            }

            CurrentNode = null;
        }
    }
}