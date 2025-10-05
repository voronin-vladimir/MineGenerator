using System.Collections.Generic;

namespace Tree
{
    public class BinaryTree<T>
    {
        public TreeNode<T> Root { get; private set; }

        public BinaryTree(T rootValue)
        {
            Root = new TreeNode<T>(rootValue);
        }
        
        public void Insert(T value)
        {
            Root = InsertRec(Root, value);
        }

        private TreeNode<T> InsertRec(TreeNode<T> node, T value)
        {
            if (node == null)
            {
                node = new TreeNode<T>(value);
                return node;
            }

            var comparison = Comparer<T>.Default.Compare(value, node.Value);
            
            switch (comparison)
            {
                case < 0:
                    node.Left = InsertRec(node.Left, value);
                    break;
                case > 0:
                    node.Right = InsertRec(node.Right, value);
                    break;
            }
            return node;
        }
    }
}