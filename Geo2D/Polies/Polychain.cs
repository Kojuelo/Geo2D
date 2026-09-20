using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace Kojuelo.Geo2D
{
    public class Polychain : IList<Polychain.Vertex>
    {
        private List<Vertex> _vertices;


        public Polychain()
        {
            _vertices = new List<Vertex>();
        }

        public Polychain(int capacity)
        {
            _vertices = new List<Vertex>(capacity);
        }

        public Polychain(in IEnumerable<Vertex> vertices)
        {
            _vertices = new List<Vertex>(vertices);
        }

        public Polychain(in IEnumerable<Point> points)
        {
            _vertices = new List<Vertex>(PointsToVertices(points));
        }

        public Polychain(params Vertex[] vertices)
        {
            _vertices = new List<Vertex>(vertices);
        }

        public Polychain(params Point[] points)
        {
            _vertices = new List<Vertex>(PointsToVertices(points));
        }


        public Vertex this[int index]
        {
            get => _vertices[index];
            set => _vertices[index] = value;
        }

        public Point this[PolyIndex polyIndex] => GetPoint(polyIndex);


        int ICollection<Vertex>.Count => _vertices.Count;

        bool ICollection<Vertex>.IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_vertices).GetEnumerator();
        }


        public int count => _vertices.Count;


        public void Add(Vertex item)
        {
            if (_vertices.Count > 0)
            {
                SetCurve(_vertices.Count - 1, default);
            }

            _vertices.Add(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Point point)
        {
            Add(new Vertex(point));
        }

        public void Insert(int index, Vertex item)
        {
            if (_vertices.Count > 0)
            {
                var prevIndex = GetPreviousIndex(index);

                SetCurve(prevIndex, default);
            }

            _vertices.Insert(index, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, Point point)
        {
            Insert(index, new Vertex(point));
        }

        public bool Remove(Vertex item)
        {
            var indexOfItem = IndexOf(item);

            if (indexOfItem < 0)
            {
                return false;
            }

            _vertices.RemoveAt(indexOfItem);

            return true;
        }

        public void RemoveAt(int index)
        {
            if (_vertices.Count > 1)
            {
                var prevIndex = GetPreviousIndex(index);

                SetCurve(prevIndex, default);
            }

            _vertices.RemoveAt(index);
        }

        public void Clear()
        {
            _vertices.Clear();
        }

        public int IndexOf(Vertex item)
        {
            return _vertices.IndexOf(item);
        }

        public bool Contains(Vertex item)
        {
            return _vertices.Contains(item);
        }

        public void CopyTo(Vertex[] array, int arrayIndex)
        {
            _vertices.CopyTo(array, arrayIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPoint(int vertexIndex, in Point point)
        {
            this[vertexIndex] = new Vertex(point, this[vertexIndex].curve);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCurve(int vertexIndex, in Curve curve)
        {
            this[vertexIndex] = new Vertex(this[vertexIndex].point, curve);
        }

        public Point GetPoint(int vertexIndex, float nextInterpolation = 0f)
        {
            switch (nextInterpolation)
            {
                case 0f:
                    return this[vertexIndex].point;

                case 1f:
                    return this[GetNextIndex(vertexIndex)].point;

                default:
                    var vertex = this[vertexIndex];
                    return vertex.curve.GetPointInCurve(vertex.point, this[GetNextIndex(vertexIndex)].point, nextInterpolation);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point GetPoint(in PolyIndex floatIndex)
        {
            return GetPoint(floatIndex.vertexIndex, floatIndex.linkInterpolant);
        }

        public int GetNextIndex(int index)
        {
            if (index == (count - 1))
            {
                return 0;
            }
            else
            {
                return index + 1;
            }
        }

        public int GetPreviousIndex(int index)
        {
            if (index == 0)
            {
                return count - 1;
            }
            else
            {
                return index - 1;
            }
        }

        public int GetLinkCount()
        {
            if (count <= 1)
            {
                return 0;
            }

            if (count == 2)
            {
                return 1;
            }

            return count;
        }

        public IEnumerable<VertexInfo> IterateVertices()
        {
            for (int i = 0; i < count; i++)
            {
                yield return new VertexInfo(this, i);
            }
        }

        public IEnumerable<LinkInfo> IterateLinks()
        {
            var linkCount = GetLinkCount();

            for (int i = 0; i < linkCount; i++)
            {
                yield return new LinkInfo(this, i);
            }
        }

        public IEnumerator<Vertex> GetEnumerator()
        {
            return _vertices.GetEnumerator();
        }

        private static IEnumerable<Vertex> PointsToVertices(IEnumerable<Point> points)
        {
            foreach (var p in points)
            {
                yield return new Vertex(p);
            }
        }


        public struct Curve
        {
            public Bezier type;

            private Point _aControlOffset;

            private Point _bControlOffset;


            public enum Bezier
            {
                Linear = 0,

                Quadratic = 1,

                Cubic = 2
            }


            public Point quadraticCenterControlOffset
            {
                readonly get => _aControlOffset;
                set => _aControlOffset = value;
            }

            public Point cubicStartControlOffset
            {
                readonly get => _aControlOffset;
                set => _aControlOffset = value;
            }

            public Point cubicEndControlOffset
            {
                readonly get => _bControlOffset;
                set => _bControlOffset = value;
            }


            public readonly Point GetPointInCurve(in Point a, in Point b, float t)
            {
                return type switch
                {
                    Bezier.Quadratic => GetPointInQuadraticCurve(a, b, t, _aControlOffset),
                    Bezier.Cubic => GetPointInCubicCurve(a, b, t, _aControlOffset, _bControlOffset),
                    _ => GetPointInLinearCurve(a, b, t),
                };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly Point GetQuadraticControlPosition(in Point a, in Point b)
            {
                return GetPointInLinearCurve(a, b, 0.5f) + _aControlOffset;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly void GetCubicControlPositions(in Point a, in Point b, out Point aControl, out Point bControl)
            {
                aControl = a + _aControlOffset;
                bControl = b + _bControlOffset;
            }

            public readonly override string ToString()
            {
                return type switch
                {
                    Bezier.Linear => $"{{{type}}}",
                    Bezier.Quadratic => $"{{{type}, {_aControlOffset}}}",
                    Bezier.Cubic => $"{{{type}, {_aControlOffset}, {_bControlOffset}}}",
                    _ => $"{{{type}, {_aControlOffset}, {_bControlOffset}}}"
                };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static Point GetPointInLinearCurve(in Point a, in Point b, float t)
            {
                return a + ((b - a) * t);
            }

            private static Point GetPointInQuadraticCurve(in Point a, in Point b, float t, in Point centerControlOffset)
            {
                var control = GetPointInLinearCurve(a, b, 0.5f) + centerControlOffset;

                var tReciprocal = 1f - t;

                return (tReciprocal * tReciprocal * a) + (2f * tReciprocal * t * control) + (t * t * b);
            }

            private static Point GetPointInCubicCurve(in Point a, in Point b, float t, in Point aControlOffset, in Point bControlOffset)
            {
                var aControl = a + aControlOffset;
                var bControl = b + bControlOffset;

                var tReciprocal = 1f - t;

                return (tReciprocal * tReciprocal * tReciprocal * a) + (3f * tReciprocal * tReciprocal * t * aControl) + (3f * tReciprocal * t * t * bControl) + (t * t * t * b);
            }
        }

        public struct Vertex
        {
            public Point point;

            public Curve curve;


            public Vertex(in Point point)
            {
                this.point = point;

                curve = default;
            }

            public Vertex(in Point point, in Curve curve)
            {
                this.point = point;
                this.curve = curve;
            }


            public float x
            {
                readonly get => point.x;
                set => point.x = value;
            }

            public float y
            {
                readonly get => point.y;
                set => point.y = value;
            }
        }

        public readonly struct VertexInfo
        {
            public readonly Vertex vertex;

            public readonly int index;


            public readonly Point point => vertex.point;

            public readonly Curve curve => vertex.curve;


            public VertexInfo(in Polychain polychain, int index)
            {
                if ((index < 0) || (index >= polychain.count))
                {
                    throw new IndexOutOfRangeException($"No polyline vertex of index \"{index}\".");
                }

                vertex = polychain[index];

                this.index = index;
            }
        }

        public readonly struct LinkInfo
        {
            public readonly VertexInfo a;

            public readonly VertexInfo b;


            public readonly Point aPoint => a.point;

            public readonly Point bPoint => b.point;

            public readonly Curve curve => a.curve;

            public readonly int aVertexIndex => a.index;

            public readonly int bVertexIndex => b.index;


            public LinkInfo(in Polychain polychain, int vertexIndex)
            {
                if ((vertexIndex < 0) || (vertexIndex >= polychain.count))
                {
                    throw new IndexOutOfRangeException($"No polychain vertex of index \"{vertexIndex}\".");
                }

                a = new VertexInfo(polychain, vertexIndex);

                b = new VertexInfo(polychain, polychain.GetNextIndex(vertexIndex));
            }


            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool IsVertexInLink(int vertexIndex)
            {
                return (vertexIndex == aVertexIndex) || (vertexIndex == bVertexIndex);
            }

            public bool IsAnyVertexInLink(params int[] vertexIndices)
            {
                foreach (var i in vertexIndices)
                {
                    if (IsVertexInLink(i))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
