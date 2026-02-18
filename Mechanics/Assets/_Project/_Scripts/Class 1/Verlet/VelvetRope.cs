using System.Collections.Generic;
using UnityEngine;

namespace VerletIntegration
{
    public class VelvetRope : MonoBehaviour
    {
        [SerializeField] private int _pointCount = 15;
        [SerializeField] private float _segmentLength = 0.3f;
        [SerializeField] private int _solverIterations = 12;

        [SerializeField] private Vector2 _gravity = new Vector2(0, -9.8f);

        [Header("Stability")]
        [SerializeField] private float _damping = 0.985f;
        [SerializeField] private float _fixedDt = 1f / 60f;
        [SerializeField] private float _collisionSoftness = 0.6f;

        [Header("Collision")]
        [SerializeField] private float _friction = 0.35f;
        [SerializeField] private MonoBehaviour[] _obstacles;

        private List<VerletPoint> _points = new List<VerletPoint>();
        private List<DistanceConstraint> _constraints = new List<DistanceConstraint>();
        private List<ICollider> _verletColliders = new List<ICollider>();

        private LineRenderer _lineRenderer;
        private float _accumulator;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            foreach (var c in _obstacles)
            {
                if (c is ICollider vc)
                {
                    _verletColliders.Add(vc);
                }
            }

            CreateRope();
        }

        private void Update()
        {
            _accumulator += Time.deltaTime;

            while (_accumulator >= _fixedDt)
            {
                Simulate(_fixedDt);
                _accumulator -= _fixedDt;
            }

            DrawRope();
        }

        private void CreateRope()
        {
            Vector2 startPosition = transform.position;

            _points.Clear();
            _constraints.Clear();

            for (int i = 0; i < _pointCount; i++)
            {
                Vector2 position = startPosition + Vector2.down * _segmentLength * i;

                bool locked = i == 0;
                var point = new VerletPoint(position, locked);

                _points.Add(point);

                if (i > 0)
                {
                    _constraints.Add(new DistanceConstraint(_points[i - 1], point, _segmentLength));
                }
            }
        }

        private void Simulate(float dt)
        {
            foreach (var p in _points)
            {
                p.Integrate(_gravity, dt, _damping);
            }

            for (int i = 0; i < _solverIterations; i++)
            {
                foreach (var c in _constraints)
                {
                    c.Solve();
                }

                foreach (var p in _points)
                {
                    SolveCollisions(p);
                }
            }
        }

        private void SolveCollisions(VerletPoint p)
        {
            if (p.locked) return;

            foreach (var collider in _verletColliders)
            {
                collider.ResolveCollision(p, _collisionSoftness, _friction);
            }
        }

        private void DrawRope()
        {
            _lineRenderer.positionCount = _points.Count;

            for (int i = 0; i < _points.Count; i++)
            {
                _lineRenderer.SetPosition(i, _points[i].position);
            }
        }
    }
}