
using System.Collections.Generic;


namespace Kojuelo.Geo2D
{
    public readonly struct PolyIndex
    {
        public readonly int vertexIndex;

        public readonly float linkInterpolant;


        public PolyIndex(int vertexIndex, float linkInterpolant)
        {
            this.vertexIndex = vertexIndex;
            this.linkInterpolant = linkInterpolant;
        }

        public PolyIndex(int vertexIndex)
            : this(vertexIndex, 0f)
        {
        }

        public PolyIndex(float floatIndex)
            : this((int)floatIndex, floatIndex % 1)
        {
        }


        public static bool operator ==(in PolyIndex a, in PolyIndex b)
        {
            if (a.vertexIndex != b.vertexIndex)
            {
                return false;
            }

            if (a.linkInterpolant != b.linkInterpolant)
            {
                return false;
            }

            return true;
        }

        public static bool operator !=(in PolyIndex a, in PolyIndex b)
        {
            return !(a == b);
        }

        public static bool operator <(in PolyIndex a, in PolyIndex b)
        {
            if (a.vertexIndex == b.vertexIndex)
            {
                return a.linkInterpolant < b.linkInterpolant;
            }

            return a.vertexIndex < b.vertexIndex;
        }

        public static bool operator >(in PolyIndex a, in PolyIndex b)
        {
            if (a.vertexIndex == b.vertexIndex)
            {
                return a.linkInterpolant > b.linkInterpolant;
            }

            return a.vertexIndex > b.vertexIndex;
        }

        public static bool operator <=(in PolyIndex a, in PolyIndex b)
        {
            if (a.vertexIndex == b.vertexIndex)
            {
                return a.linkInterpolant <= b.linkInterpolant;
            }

            return a.vertexIndex < b.vertexIndex;
        }

        public static bool operator >=(in PolyIndex a, in PolyIndex b)
        {
            if (a.vertexIndex == b.vertexIndex)
            {
                return a.linkInterpolant >= b.linkInterpolant;
            }

            return a.vertexIndex > b.vertexIndex;
        }


        public readonly bool isIndexFloating => (linkInterpolant > 0f) && (linkInterpolant < 1f);


        public static IEnumerable<PolyIndex> IterateBetweenIndexes(PolyIndex from, PolyIndex to, bool fromExclusive = false, bool toExclusive = false)
        {
            if (from == to)
            {
                if (fromExclusive && toExclusive)
                {
                    yield break;
                }

                yield return from;
            }

            if (from < to)
            {
                if (!fromExclusive)
                {
                    yield return from;
                }

                for (var iF = from.Next(); iF < to; iF = iF.Next())
                {
                    yield return iF;
                }

                if (!toExclusive)
                {
                    yield return to;
                }
            }
            else
            {
                if (!fromExclusive)
                {
                    yield return from;
                }

                for (var iF = from.Previous(); iF > to; iF = iF.Previous())
                {
                    yield return iF;
                }

                if (!toExclusive)
                {
                    yield return to;
                }
            }
        }

        public static bool AreIndicesAdjacent(in PolyIndex a, in PolyIndex b)
        {
            if (a.vertexIndex == b.vertexIndex)
            {
                return true;
            }

            if ((a.vertexIndex == (b.vertexIndex - 1)) && (b.linkInterpolant == 0f))
            {
                return true;
            }

            if ((a.vertexIndex == (b.vertexIndex + 1)) && (a.linkInterpolant == 0f))
            {
                return true;
            }

            return false;
        }

        public readonly PolyIndex Next()
        {
            return new PolyIndex(vertexIndex + 1);
        }

        public readonly PolyIndex Previous()
        {
            return new PolyIndex((linkInterpolant > 0f) ? vertexIndex : (vertexIndex - 1));
        }

        public readonly override string ToString()
        {
            return (linkInterpolant == 0f) ? vertexIndex.ToString() : (vertexIndex + linkInterpolant).ToString("0.00");
        }

        public readonly override bool Equals(object obj)
        {
            if (obj is PolyIndex objIndex)
            {
                return this == objIndex;
            }

            if (obj is int objInt)
            {
                return this == new PolyIndex(objInt);
            }

            if (obj is float objFloat)
            {
                return this == new PolyIndex((float)objFloat);
            }

            return false;
        }

        public readonly override int GetHashCode()
        {
            return vertexIndex.GetHashCode() ^ linkInterpolant.GetHashCode();
        }
    }
}