namespace Geo2DTests;


public static class SegmentTests
{
    [Fact]
    public static void Constructor()
    {
        Segment s0 = default;

        Assert.Equal(new Point(0f, 0f), s0.a);
        Assert.Equal(new Point(0f, 0f), s0.b);

        Segment s1 = new Segment(1f, 2f, 3f, 4f);

        Assert.Equal(new Point(1f, 2f), s1.a);
        Assert.Equal(new Point(3f, 4f), s1.b);

        Segment s2 = new Segment(new Point(1f, 2f), new Point(3f, 4f));

        Assert.Equal(new Point(1f, 2f), s2.a);
        Assert.Equal(new Point(3f, 4f), s2.b);
    }

    [Fact]
    public static void ConstructorDirection()
    {
        const float DIAGONAL_MAGNITUDE =  0.70710678118f;

        Segment s = new Segment(new Point(1f, 2f), new Direction(1f, 1f), 3f);

        Assert.Equal(new Point(1f, 2f), s.a);
        Assert.Equal(new Point(1f + (DIAGONAL_MAGNITUDE * 3f), 2f + (DIAGONAL_MAGNITUDE * 3f)), s.b);
    }

    [Fact]
    public static void Equal()
    {
        Segment s1 = new Segment(1f, 2f, 3f, 4f);
        Segment s2 = new Segment(0f, 0f, -2f, -5f);

        Assert.True(s1 == new Segment(1f, 2f, 3f, 4f));
        Assert.True(s2 == new Segment(0f, 0f, -2f, -5f));
        Assert.False(s1 == s2);

        Assert.True(s1.Equals(new Segment(1f, 2f, 3f, 4f)));
        Assert.True(s2.Equals(new Segment(0f, 0f, -2f, -5f)));
        Assert.False(s1.Equals(s2));

        Assert.False(s1.Equals(new object()));
    }

    [Fact]
    public static void NotEqual()
    {
        Segment s1 = new Segment(1f, 2f, 3f, 4f);
        Segment s2 = new Segment(0f, 0f, -2f, -5f);

        Assert.False(s1 != new Segment(1f, 2f, 3f, 4f));
        Assert.False(s2 != new Segment(0f, 0f, -2f, -5f));
        Assert.True(s1 != s2);
    }

    [Fact]
    public static void Translate()
    {
        Segment s1 = new Segment(1f, 2f, 3f, 4f);

        s1.Translate(-2f, 5f);
        Assert.Equal(new Segment(-1f, 7f, 1f, 9f), s1);

        Segment s2 = new Segment(0f, 0f, -2f, -5f);

        s2.Translate(new Point(2f, -5f));
        Assert.Equal(new Segment(2f, -5f, 0f, -10f), s2);

        s1 = new Segment(1f, 2f, 3f, 4f);
        s2 = new Segment(0f, 0f, -2f, -5f);

        Assert.Equal(new Segment(-1f, 7f, 1f, 9f), s1.GetTranslated(-2f, 5f));
        Assert.Equal(new Segment(2f, -5f, 0f, -10f), s2.GetTranslated(2f, -5f));
        Assert.Equal(new Segment(1f, 2f, 3f, 4f), s1);
        Assert.Equal(new Segment(0f, 0f, -2f, -5f), s2);

        Assert.Equal(new Segment(-1f, 7f, 1f, 9f), s1 + new Point(-2f, 5f));
        Assert.Equal(new Segment(2f, -5f, 0f, -10f), s2 + new Point(2f, -5f));
        Assert.Equal(new Segment(1f, 2f, 3f, 4f), s1);
        Assert.Equal(new Segment(0f, 0f, -2f, -5f), s2);
    }

    [Fact]
    public static void TranslateDirection()
    {
        const float DIAGONAL_MAGNITUDE =  0.70710678118f;

        Segment s = new Segment(1f, 2f, 3f, 4f);

        s.Translate(new Direction(1f, 1f), 3f);

        Assert.Equal(new Segment(1f + (DIAGONAL_MAGNITUDE * 3f), 2f + (DIAGONAL_MAGNITUDE * 3f), 3f + (DIAGONAL_MAGNITUDE * 3f), 4f + (DIAGONAL_MAGNITUDE * 3f)), s);

        Assert.Equal(new Segment(1f + (DIAGONAL_MAGNITUDE * 1f), 2f + (DIAGONAL_MAGNITUDE * 5f), 3f + (DIAGONAL_MAGNITUDE * 1f), 4f + (DIAGONAL_MAGNITUDE * 5f)), s.GetTranslated(new Direction(-1f, 1f), 2f));
        Assert.Equal(new Segment(1f + (DIAGONAL_MAGNITUDE * 3f), 2f + (DIAGONAL_MAGNITUDE * 3f), 3f + (DIAGONAL_MAGNITUDE * 3f), 4f + (DIAGONAL_MAGNITUDE * 3f)), s);
    }

    [Fact]
    public static void RotateRadians()
    {
        Segment s = new Segment(1f, 2f, -2f, -1f);

        s.RotateRadians(MathF.PI * 0.5f);
        Assert.Equal(new Segment(-2f, 1f, 1f, -2f), s);

        s.RotateRadians(MathF.PI);
        Assert.Equal(new Segment(2f, -1f, -1f, 2f), s);

        Assert.Equal(new Segment(1f, 2f, -2f, -1f), s.GetRotatedRadians(MathF.PI * -1.5f));
        Assert.Equal(new Segment(2f, -1f, -1f, 2f), s);
    }

    [Fact]
    public static void RotateRadiansPivot()
    {
        Segment s1 = new Segment(1f, 2f, -2f, -1f);

        s1.RotateRadians(new Point(1f, 2f), MathF.PI * 0.5f);
        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s1);

        s1.RotateRadians(new Point(4f, -1f), MathF.PI);
        Assert.Equal(new Segment(7f, -4f, 4f, -1f), s1);

        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s1.GetRotatedRadians(new Point(4f, -1f), -MathF.PI));
        Assert.Equal(new Segment(7f, -4f, 4f, -1f), s1);

        s1 = new Segment(1f, 2f, 4f, -1f);
        Assert.Equal(new Segment(1f, 2f, -2f, -1f), s1.GetRotatedRadians(new Point(1f, 2f), MathF.PI * -0.5f));
        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s1);

        Segment s2 = new Segment(1f, 2f, -2f, -1f);

        s2.RotateRadiansFromA(MathF.PI * 0.5f);
        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s2);

        s2.RotateRadiansFromB(MathF.PI);
        Assert.Equal(new Segment(7f, -4f, 4f, -1f), s2);

        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s2.GetRotatedRadiansFromB(-MathF.PI));
        Assert.Equal(new Segment(7f, -4f, 4f, -1f), s2);

        s2 = new Segment(1f, 2f, 4f, -1f);
        Assert.Equal(new Segment(1f, 2f, -2f, -1f), s2.GetRotatedRadiansFromA(MathF.PI * -0.5f));
        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s2);
    }

    [Fact]
    public static void RotateDegrees()
    {
        Segment s = new Segment(1f, 2f, -2f, -1f);

        s.RotateDegrees(90f);
        Assert.Equal(new Segment(-2f, 1f, 1f, -2f), s);

        s.RotateDegrees(180f);
        Assert.Equal(new Segment(2f, -1f, -1f, 2f), s);

        Assert.Equal(new Segment(1f, 2f, -2f, -1f), s.GetRotatedDegrees(-270f));
        Assert.Equal(new Segment(2f, -1f, -1f, 2f), s);
    }

    [Fact]
    public static void RotateDegreesPivot()
    {
        Segment s1 = new Segment(1f, 2f, -2f, -1f);

        s1.RotateDegrees(new Point(1f, 2f), 90f);
        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s1);

        s1.RotateDegrees(new Point(4f, -1f), 180f);
        Assert.Equal(new Segment(7f, -4f, 4f, -1f), s1);

        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s1.GetRotatedDegrees(new Point(4f, -1f), -180f));
        Assert.Equal(new Segment(7f, -4f, 4f, -1f), s1);

        s1 = new Segment(1f, 2f, 4f, -1f);
        Assert.Equal(new Segment(1f, 2f, -2f, -1f), s1.GetRotatedDegrees(new Point(1f, 2f), -90f));
        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s1);

        Segment s2 = new Segment(1f, 2f, -2f, -1f);

        s2.RotateDegreesFromA(90f);
        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s2);

        s2.RotateDegreesFromB(180f);
        Assert.Equal(new Segment(7f, -4f, 4f, -1f), s2);

        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s2.GetRotatedDegreesFromB(-180f));
        Assert.Equal(new Segment(7f, -4f, 4f, -1f), s2);

        s2 = new Segment(1f, 2f, 4f, -1f);
        Assert.Equal(new Segment(1f, 2f, -2f, -1f), s2.GetRotatedDegreesFromA(-90f));
        Assert.Equal(new Segment(1f, 2f, 4f, -1f), s2);
    }

    [Fact]
    public static void Scale()
    {
        Segment s1 = new Segment(1f, 2f, -3f, -4f);

        s1.Scale(0.5f);
        Assert.Equal(new Segment(0.5f, 1f, -1.5f, -2f), s1);

        s1.Scale(0.5f, 0.1f);
        Assert.Equal(new Segment(0.25f, 0.1f, -0.75f, -0.2f), s1);

        Segment s2 = new Segment(1f, 2f, -3f, -4f);

        Assert.Equal(new Segment(0.5f, 1f, -1.5f, -2f), s2.GetScaled(0.5f));
        Assert.Equal(new Segment(1f, 2f, -3f, -4f), s2);

        Assert.Equal(new Segment(0.5f, 0.2f, -1.5f, -0.4f), s2.GetScaled(0.5f, 0.1f));
        Assert.Equal(new Segment(1f, 2f, -3f, -4f), s2);
    }

    [Fact]
    public static void ScalePivot()
    {
        Segment s1 = new Segment(1f, 2f, -3f, -4f);

        s1.Scale(new Point(1f, 2f), 0.5f);
        Assert.Equal(new Segment(1f, 2f, -1f, -1f), s1);

        s1.Scale(new Point(1f, 2f), 0.5f, 0.1f);
        Assert.Equal(new Segment(1f, 2f, 0f, 1.7f), s1);

        Segment s2 = new Segment(1f, 2f, -3f, -4f);

        s2.Scale(new Point(-3f, -4f), 0.5f);
        Assert.Equal(new Segment(-1f, -1f, -3f, -4f), s2);

        s2.Scale(new Point(-3f, -4f), 0.5f, 0.1f);
        Assert.Equal(new Segment(-2f, -3.7f, -3f, -4f), s2);

        Segment s3 = new Segment(1f, 2f, -3f, -4f);

        s3.ScaleFromA(0.5f);
        Assert.Equal(new Segment(1f, 2f, -1f, -1f), s3);

        s3.ScaleFromA(0.5f, 0.1f);
        Assert.Equal(new Segment(1f, 2f, 0f, 1.7f), s3);

        Segment s4 = new Segment(1f, 2f, -3f, -4f);

        s4.ScaleFromB(0.5f);
        Assert.Equal(new Segment(-1f, -1f, -3f, -4f), s4);

        s4.ScaleFromB(0.5f, 0.1f);
        Assert.Equal(new Segment(-2f, -3.7f, -3f, -4f), s4);

        Segment s5 = new Segment(1f, 2f, -3f, -4f);

        Assert.Equal(new Segment(1f, 2f, -1f, -1f), s5.GetScaled(new Point(1f, 2f), 0.5f));
        Assert.Equal(new Segment(1f, 2f, 0.6f, 0.8f), s5.GetScaled(new Point(1f, 2f), 0.1f, 0.2f));
        Assert.Equal(new Segment(1f, 2f, -1f, -1f), s5.GetScaledFromA(0.5f));
        Assert.Equal(new Segment(1f, 2f, 0.6f, 0.8f), s5.GetScaledFromA(0.1f, 0.2f));
        Assert.Equal(new Segment(1f, 2f, -3f, -4f), s5);

        Assert.Equal(new Segment(-1f, -1f, -3f, -4f), s5.GetScaled(new Point(-3f, -4f), 0.5f));
        Assert.Equal(new Segment(-2.6f, -2.8f, -3f, -4f), s5.GetScaled(new Point(-3f, -4f), 0.1f, 0.2f));
        Assert.Equal(new Segment(-1f, -1f, -3f, -4f), s5.GetScaledFromB(0.5f));
        Assert.Equal(new Segment(-2.6f, -2.8f, -3f, -4f), s5.GetScaledFromB(0.1f, 0.2f));
        Assert.Equal(new Segment(1f, 2f, -3f, -4f), s5);
    }

    [Fact]
    public static void Bounds()
    {
        Segment s1 = new Segment(1f, 2f, -3f, -4f);

        s1.GetBounds(out var xMin, out var yMin, out var xMax, out var yMax);
        Assert.Equal(-3f, xMin);
        Assert.Equal(-4f, yMin);
        Assert.Equal(1f, xMax);
        Assert.Equal(2f, yMax);

        Bounds b = s1.GetBounds();
        Assert.Equal(-3f, b.xMin);
        Assert.Equal(-4f, b.yMin);
        Assert.Equal(1f, b.xMax);
        Assert.Equal(2f, b.yMax);

        Segment s2 = new Segment(-3f, -4f, 1f, 2f);

        s2.GetBounds(out xMin, out yMin, out xMax, out yMax);
        Assert.Equal(-3f, xMin);
        Assert.Equal(-4f, yMin);
        Assert.Equal(1f, xMax);
        Assert.Equal(2f, yMax);

        b = s2.GetBounds();
        Assert.Equal(-3f, b.xMin);
        Assert.Equal(-4f, b.yMin);
        Assert.Equal(1f, b.xMax);
        Assert.Equal(2f, b.yMax);
    }
}