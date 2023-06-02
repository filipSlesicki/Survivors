using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quadtree
{
    public class QuadTreePool
    {
        private Stack<QuadTree> quadTrees;
        private int maxObjectsPerNode;

        public QuadTreePool(int initialSize, int maxObjectsPerNode)
        {
            this.maxObjectsPerNode = maxObjectsPerNode;
            quadTrees = new Stack<QuadTree>(initialSize);
            for (int i = 0; i < initialSize; i++)
            {
                quadTrees.Push(new QuadTree(maxObjectsPerNode, -1, new SimpleRect()));
            }
        }

        public QuadTree Get(int level, SimpleRect bounds, QuadTree parent = null)
        {
            if (quadTrees.Count == 0)
            {
                return new QuadTree(maxObjectsPerNode, level, bounds, parent);
            }

            var qt = quadTrees.Pop();


            qt.Set(level, bounds, parent);
            return qt;

        }

        public void Release(QuadTree quadTree)
        {
            quadTree.Clear();
            quadTrees.Push(quadTree);
        }
    }
}