using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quadtree
{
    public class QuadtreeObject
    {
        public SimpleRect bounds;
        public QuadTree tree;

        public MonoBehaviour owner;

        public QuadtreeObject(SimpleRect bounds, MonoBehaviour owner)
        {
            this.bounds = bounds;
            this.owner = owner;
        }

        public bool Moved()
        {
            if (tree == null)
            {
                return false;
            }

            return !tree.Contains(bounds);
        }

        public void RemoveFromTree()
        {
            tree.Remove(this);
        }

        public void UpdatePos(Vector2 pos)
        {
            bounds.UpdatePos(pos);
        }
    }
}