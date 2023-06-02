using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quadtree
{
    public class QTTester : MonoBehaviour
    {
        [SerializeField] Vector2 center;
        [SerializeField] float width;
        [SerializeField] float height;
        [SerializeField] int count = 30;
        [SerializeField] int max = 1;
        [SerializeField] QTTestObject spawnPrefab;

        public static float halfMapWidth;
        public static float halfMapHeight;

        private QuadTree mainQuadTree;
        private List<QuadtreeObject> allObjs = new List<QuadtreeObject>();
        private List<QuadtreeObject> moved = new List<QuadtreeObject>();
        private List<QuadtreeObject> inRange = new List<QuadtreeObject>();

        private void Start()
        {
            halfMapWidth = width / 2;
            halfMapHeight = height / 2;
            mainQuadTree = new QuadTree(max, 0, new SimpleRect(center, width, height));
            for (int i = 0; i < count; i++)
            {
                Vector3 randomPos = (Vector3)center + new Vector3(Random.Range(-halfMapWidth, halfMapWidth), Random.Range(-halfMapHeight, halfMapHeight), 0);
                QTTestObject spawned = Instantiate(spawnPrefab, randomPos, Quaternion.identity);
                spawned.name += i;
                allObjs.Add(spawned.qtInfo);
                mainQuadTree.Insert(spawned.qtInfo);
            }
        }



        void UpdateQuadTree()
        {
            moved.Clear();
            foreach (var obj in allObjs)
            {
                if (obj.Moved())
                {
                    obj.RemoveFromTree();
                    moved.Add(obj);
                }
            }
            int movedCount = moved.Count;
            if (movedCount > 0)
            {
                mainQuadTree.Collapse();

                for (int i = 0; i < movedCount; i++)
                {
                    mainQuadTree.Insert(moved[i]);
                }
            }
        }

        private void Update()
        {
            UpdateQuadTree();
            FindAllClosest();
        }

        void FindAllClosest()
        {
            SimpleCircle quarycircle = new SimpleCircle();
            quarycircle.radius = 10;
            foreach (var obj in allObjs)
            {
                quarycircle.center = obj.bounds.center;
                mainQuadTree.GetAllInCircle(quarycircle, inRange);
                float bestDist = float.MaxValue;
                QuadtreeObject closest = null;
                foreach (var close in inRange)
                {
                    if (obj == close)
                        continue;

                    float distance = Vector2.Distance(close.bounds.center, obj.bounds.center);
                    if (distance < bestDist)
                    {
                        bestDist = distance;
                        closest = close;
                    }
                }
                if (closest != null)
                {
                    Vector2 toClosest = closest.bounds.center - obj.bounds.center;
                    Debug.DrawRay(obj.bounds.center, toClosest, Color.green);
                }

                inRange.Clear();
            }
        }

        void RebuildQT()
        {
            mainQuadTree.CollapseAll();
            int objCount = allObjs.Count;
            for (int i = 0; i < objCount; i++)
            {
                mainQuadTree.Insert(allObjs[i]);
            }
        }


        private void OnDrawGizmos()
        {
            if (mainQuadTree == null)
            {
                return;
            }

            mainQuadTree.Draw();
        }
    }
}