using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quadtree
{
    public class QuadTree
    {
        private int maxObjectsPerNode;
        private int depth;
        private SimpleRect bounds;

        private QuadtreeObject[] objects; //Array for performance. Could convert to list or custom collection for readability
        private int objectsCount = 0;

        private QuadTree parent;
        private QuadTree[] children;
        private bool split;

        static QuadTreePool treePool;

        public QuadTree(int maxObjectsPerNode, int level, SimpleRect bounds, QuadTree parent = null)
        {
            this.maxObjectsPerNode = maxObjectsPerNode;
            depth = level;
            this.bounds = bounds;
            if (level == 0 && treePool == null)
            {
                treePool = new QuadTreePool(32, maxObjectsPerNode);
            }
            objects = new QuadtreeObject[maxObjectsPerNode + 1];
            children = new QuadTree[4];
            this.parent = parent;
        }

        public void Set(int level, SimpleRect bounds, QuadTree parent)
        {
            depth = level;
            this.bounds = bounds;
            this.parent = parent;
        }

        public void Clear()
        {
            objectsCount = 0;

            children[0] = null;
            children[1] = null;
            children[2] = null;
            children[3] = null;
            split = false;
        }

        public bool Contains(SimpleRect rect)
        {
            return bounds.Contains(rect.center);
        }

        /// <summary>
        /// Insert objec into current tree or one of it's children
        /// </summary>
        /// <param name="obj">Object to insert</param>
        /// <returns>Succesfully inserted</returns>
        public bool Insert(QuadtreeObject obj)
        {
            SimpleRect objRect = obj.bounds;
            if (!bounds.Contains(objRect.center))
                return false;

            if (split)
            {
                // Insert into child node
                for (int i = 0; i < 4; i++)
                {
                    if (children[i].Insert(obj))
                    {
                        return true;
                    }
                }
                Debug.LogWarning("cant insert into children. Should never happen");
                return false;
            }

            //Add to this tree
            objects[objectsCount] = obj;
            objectsCount++;
            obj.tree = this;

            if (objectsCount > maxObjectsPerNode)
            {
                if (!split)
                {
                    Split();
                }
                //Spread objects from this to children
                for (int i = 0; i < objectsCount; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (children[j].Insert(objects[i]))
                        {
                            break;
                        }
                    }
                }
                objectsCount = 0;
            }
            return true;
        }

        /// <summary>
        /// Split current tree into 4 smaller trees
        /// </summary>
        private void Split()
        {
            float subWidth = bounds.halfWidth;
            float subHeight = bounds.halfHeight;
            float x = bounds.center.x;
            float y = bounds.center.y;

            children[0] = treePool.Get(depth + 1, new SimpleRect(new Vector2(x - subWidth / 2, y + subHeight / 2), subWidth, subHeight), this);
            children[1] = treePool.Get(depth + 1, new SimpleRect(new Vector2(x + subWidth / 2, y + subHeight / 2), subWidth, subHeight), this);
            children[2] = treePool.Get(depth + 1, new SimpleRect(new Vector2(x - subWidth / 2, y - subHeight / 2), subWidth, subHeight), this);
            children[3] = treePool.Get(depth + 1, new SimpleRect(new Vector2(x + subWidth / 2, y - subHeight / 2), subWidth, subHeight), this);
            split = true;
        }

        /// <summary>
        /// Remove object from this and only this tree
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(QuadtreeObject obj)
        {
            for (int i = 0; i < objectsCount; i++)
            {
                if (objects[i] == obj)
                {
                    //Remove
                    objectsCount--;
                    objects[i] = objects[objectsCount];
                    objects[objectsCount] = null;
                    return;
                }
            }
            Debug.LogWarning("Couldnt remove. Should never happen");
        }

        /// <summary>
        /// Collapse empty child nodes to fix structure
        /// </summary>
        /// <returns>Can collapse</returns>
        public bool Collapse()
        {
            if (!split)
            {
                return true;
            }

            int objectsInChildren = 0;
            bool allCollapsable = true;
            for (int i = 0; i < 4; i++)
            {
                if (!children[i].Collapse())
                {
                    allCollapsable = false;
                }
                objectsInChildren += children[i].objectsCount;
            }
            if (!allCollapsable)
                return false;

            if (objectsInChildren <= maxObjectsPerNode)
            {
                for (int i = 0; i < 4; i++)
                {
                    int objCount = children[i].objectsCount;
                    for (int j = 0; j < objCount; j++)
                    {
                        //Add
                        objects[objectsCount] = children[i].objects[j];
                        objectsCount++;
                        children[i].objects[j].tree = this;
                    }
                    treePool.Release(children[i]);
                    children[i] = null;
                }
                split = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Collapse all nodes. Used to rebuild tree from scratch
        /// </summary>
        public void CollapseAll()
        {
            for (int i = 0; i < children.Length; i++)
            {
                QuadTree child = children[i];

                if (child != null)
                {
                    child.CollapseAll();
                    treePool.Release(children[i]);
                    children[i] = null;
                }
            }
            split = false;
        }

        /// <summary>
        /// Get all objects in a rectangular area and put them in a provided list
        /// </summary>
        /// <param name="queryBounds">rectangle to check</param>
        /// <param name="result">list of all found objects</param>
        public void GetAllInRect(SimpleRect queryBounds, List<QuadtreeObject> result)
        {
            if (!bounds.OverlapsRect(queryBounds))
            {
                return;
            }

            for (int i = 0; i < objectsCount; i++)
            {
                if (queryBounds.OverlapsRect(objects[i].bounds))
                {
                    result.Add(objects[i]);
                }
            }

            if (split)
            {
                children[0].GetAllInRect(queryBounds, result);
                children[1].GetAllInRect(queryBounds, result);
                children[2].GetAllInRect(queryBounds, result);
                children[3].GetAllInRect(queryBounds, result);
            }
        }

        /// <summary>
        /// Get all objects in a circular area and put them in a provided list
        /// </summary>
        /// <param name="circle">search area</param>
        /// <param name="result">list of all objects found</param>
        public void GetAllInCircle(SimpleCircle circle, List<QuadtreeObject> result)
        {
            if (!bounds.OverlapsCircle(circle))
            {
                return;
            }

            for (int i = 0; i < objectsCount; i++)
            {
                if (objects[i].bounds.OverlapsCircle(circle))
                {
                    result.Add(objects[i]);
                }
            }

            if (split)
            {
                children[0].GetAllInCircle(circle, result);
                children[1].GetAllInCircle(circle, result);
                children[2].GetAllInCircle(circle, result);
                children[3].GetAllInCircle(circle, result);
            }
        }

        //private int GetChildIndex(SimpleRect objBounds)
        //{
        //    for (int i = 0; i < children.Length; i++)
        //    {
        //        if (children[i].bounds.Contains(objBounds.center))
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}

        /// <summary>
        /// Draw quadtree and all children. Called from OnDrawGizmos
        /// </summary>
        public void Draw()
        {
            Gizmos.DrawWireCube(bounds.center, new Vector2(bounds.halfWidth * 2, bounds.halfHeight * 2));
            if (split)
            {
                children[0].Draw();
                children[1].Draw();
                children[2].Draw();
                children[3].Draw();
            }
        }

        public override string ToString()
        {
            int childInParent = parent != null ? System.Array.IndexOf<QuadTree>(parent.children, this) : -2;
            return "Tree level " + depth + " index " + childInParent;
        }
    }
}