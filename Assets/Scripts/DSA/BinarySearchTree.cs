using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/*
 * COMPILE THIS IN C++
 * 
main(k) 
{ 
    float i, j, r, x, y = -16; 
    while (puts(""),y++ < 15) 
        for (x = 0; x++ < 84; putchar(" .:-;!/>)|&IH%*#"[k & 15])) 
            for (i = k = r = 0;`j = r * r - i * i - 2 + x / 25, i = 2 * r * i + y / 10, j * j + i * i < 11 && k++ < 111; r = j); 
}
*/

namespace DSA
{
    public class BinarySearchTree : BinaryTree<int>
    {
        #region Properties

        #endregion

        public void Add(int iValue)
        {
            m_root = Insert(m_root, iValue);
        }

        protected virtual Node Insert(Node node, int iValue)
        {
            // create node?
            if (node == null)
            {
                return new Node { m_value = iValue };
            }

            if (iValue <= node.m_value)
            {
                node.m_left = Insert(node.m_left, iValue);
            }
            else
            {
                node.m_right = Insert(node.m_right, iValue);
            }

            return node;
        }

        public void Remove(int iValue)
        {
            m_root = Remove(m_root, iValue);
        }

        protected virtual Node Remove(Node node, int iValue)
        {
            if (node == null)
            {
                return null;
            }

            if (iValue < node.m_value)
            {
                // target node somewhere in the left subtree
                node.m_left = Remove(node.m_left, iValue);
            }
            else if (iValue > node.m_value)
            {
                // target node somewhere in the right subtree
                node.m_right = Remove(node.m_right, iValue);
            }
            else
            {
                // Gotcha!

                if (node.m_left == null &&
                    node.m_right == null)
                {
                    // leaf node
                    return null;
                }
                else if (node.m_left != null &&
                        node.m_right != null)
                {
                    // let successor take our place
                    Node successor = GetSuccessor(node);
                    node.m_value = successor.m_value;
                    node.m_right = Remove(node.m_right, successor.m_value);
                }
                else
                {
                    // single kid? no probs
                    return node.m_left != null ? node.m_left : node.m_right;
                }
            }

            return node;
        }

        protected Node GetSuccessor(Node node)
        {
            // Step 1 right
            Node n = node.m_right;

            // Step as many as possible to the left
            while (n.m_left != null)
            {
                n = n.m_left;
            }

            return n;
        }

        public int Find(int iValue)
        {
            return Find(m_root, iValue).m_value;
        }

        protected Node Find(Node node, int iValue)
        {
            if(node == null)
                return null;
            
            if(iValue < node.m_value)
                return Find(node.m_left, iValue);
            
            if(iValue > node.m_value)
                return Find(node.m_right, iValue);
            
            return node;
        }
    }
}