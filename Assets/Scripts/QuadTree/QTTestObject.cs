using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quadtree
{
    public class QTTestObject : MonoBehaviour
    {

        [SerializeField] bool move;

        public QuadtreeObject qtInfo;

        private Transform trans;
        private Vector2 pos;
        private int framesSincleLast;
        private Vector2 dir;

        void Awake()
        {
            trans = transform;
            pos = trans.position;
            qtInfo = new QuadtreeObject(new SimpleRect(pos, 1, 1), this);
        }

        //void OnEnable()
        //{

        //}
        void OnDisable()
        {
            if(qtInfo.tree != null)
            qtInfo.RemoveFromTree();
        }

        public void Update()
        {
            framesSincleLast++;
            if (move)
            {
                Move();
            }
            pos = trans.position;
            qtInfo.UpdatePos(pos);
        }

        void Move()
        {
            if (framesSincleLast % 10 == 0)
            {
                dir = Random.insideUnitCircle * 10 * Time.deltaTime;
            }
            pos += dir;
            if (pos.x > QTTester.halfMapWidth)
            {
                pos.x = QTTester.halfMapWidth;
            }
            else if (pos.x < -QTTester.halfMapWidth)
            {
                pos.x = -QTTester.halfMapWidth;
            }
            if (pos.y > QTTester.halfMapHeight)
            {
                pos.y = QTTester.halfMapHeight;
            }
            else if (pos.y < -QTTester.halfMapHeight)
            {
                pos.y = -QTTester.halfMapHeight;
            }
            trans.position = pos;
        }


    }
}