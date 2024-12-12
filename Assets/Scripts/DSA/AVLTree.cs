using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSA
{
    public class AVLTree : BinarySearchTree
    {
        protected override Node Insert(Node node, int iValue)
        {
            // trying to add a duplicate value?
            if (node != null && node.m_value == iValue)
            {
                return node;
            }

            // do standard BST insert
            node = base.Insert(node, iValue);

            // do the AVL balancing
            return BalanceNode(node);
        }

        protected override Node Remove(Node node, int iValue)
        {
            // do standard BST removal
            node = base.Remove(node, iValue);

            // do the AVL balancing
            return BalanceNode(node);
        }

        public static Node BalanceNode(Node node)
        {
            // get the balance
            int iBalance = GetBalance(node);

            // Case 1: Left Left
            if (iBalance > 1 && GetBalance(node.m_left) >= 0)
            {
                return RotateRight(node);
            }

            // Case 2: Right Right
            if (iBalance < -1 && GetBalance(node.m_right) <= 0)
            {
                return RotateLeft(node);
            }

            // Case 3: Left Right
            if (iBalance > 1 && GetBalance(node.m_left) < 0)
            {
                node.m_left = RotateLeft(node.m_left);
                return RotateRight(node);
            }

            // Case 4: Right Left
            if (iBalance < -1 && GetBalance(node.m_right) > 0)
            {
                node.m_right = RotateRight(node.m_right);
                return RotateLeft(node);
            }

            return node;
        }

        public static Node RotateRight(Node node)
        {
            Node oldLeft = node.m_left;
            Node newLeft = oldLeft.m_right;

            // right rotation
            oldLeft.m_right = node;
            node.m_left = newLeft;

            // return new root
            return oldLeft;
        }

        public static Node RotateLeft(Node node)
        {
            Node oldRight = node.m_right;
            Node newRight = oldRight.m_left;

            // left rotation
            oldRight.m_left = node;
            node.m_right = newRight;

            // return new root
            return oldRight;
        }
    }
}