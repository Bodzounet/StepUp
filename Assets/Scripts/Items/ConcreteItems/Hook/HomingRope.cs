using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Items
{
    public class HomingRope : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        private Vector3 _targetPosition;

        private Transform _hook;

        void Awake()
        {
            _lineRenderer = this.GetComponent<LineRenderer>();
            _hook = transform.GetChild(0);
        }

        void Start()
        {
            StartCoroutine(Co_GrowRope());
        }

        private List<Vector3> _pos = new List<Vector3>();
        int count = 2;
        private IEnumerator Co_GrowRope()
        {
            Vector3 ini = GameObject.Find("Player1").transform.position;
            Vector3 end = GameObject.Find("Player3").transform.position;

            Vector3 movement = (end - ini).normalized * 0.33f;

            _lineRenderer.SetPosition(0, ini);
            _lineRenderer.SetPosition(1, ini + movement);

            _pos.Add(ini);
            _pos.Add(ini + movement);

            while (true)
            {
                _lineRenderer.SetVertexCount(count + 1);
                _lineRenderer.SetPosition(count, ini + movement * count);
                _hook.localPosition = ini + movement * count;
                
                count++;
                _pos.Add(ini + movement * count);

                yield return new WaitForEndOfFrame();
            }
        }

        public void Test()
        {
            _pos.Reverse();
            _lineRenderer.SetPositions(_pos.ToArray());

            StartCoroutine(Co_ReduceRope());
        }

        private IEnumerator Co_ReduceRope()
        {
            var t = GameObject.Find("Player1").transform;

            while (count > 2)
            {
                t.position = _pos[count - 1];
                _lineRenderer.SetVertexCount(count--);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}