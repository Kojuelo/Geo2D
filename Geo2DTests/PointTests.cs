namespace Geo2DTests;


public static class PointTests
{
    [Fact]
    public static void Constructor()
    {
        Point p = new Point(1.5f, -2.37f);

        Assert.Equal(1.5f, p.x);
        Assert.Equal(-2.37f, p.y);
    }

    [Fact]
    public static void Equal()
    {
        Point p1 = new Point(1.5f, -2.37f);
        Point p2 = new Point(-0.1f, 2.56f);

        Assert.True(p1 == new Point(1.5f, -2.37f));
        Assert.True(p2 == new Point(-0.1f, 2.56f));
        Assert.False(p1 == p2);

        Assert.True(p1.Equals(new Point(1.5f, -2.37f)));
        Assert.True(p2.Equals(new Point(-0.1f, 2.56f)));
        Assert.False(p1.Equals(p2));

        Assert.False(p1.Equals(new object()));
    }

    [Fact]
    public static void NotEqual()
    {
        Point p1 = new Point(1.5f, -2.37f);
        Point p2 = new Point(-0.1f, 2.56f);

        Assert.False(p1 != new Point(1.5f, -2.37f));
        Assert.False(p2 != new Point(-0.1f, 2.56f));
        Assert.True(p1 != p2);
    }

    [Fact]
    public static void Translate()
    {
        Point p1 = new Point(1f, 2f);
        Point p2 = new Point(-3f, 4f);

        Point p = p1;
        p.Translate(-3f, 4f);
        Assert.Equal(new Point(-2f, 6f), p);

        p = p1;
        p.Translate(p2);
        Assert.Equal(new Point(-2f, 6f), p);

        p = p1;
        Assert.Equal(new Point(-2f, 6f), p.GetTranslated(-3f, 4f));
        Assert.Equal(new Point(-2f, 6f), p.GetTranslated(p2));
        Assert.Equal(p1, p);

        Assert.Equal(new Point(-2f, 6f), p1 + p2);
        Assert.Equal(new Point(4f, -2f), p1 - p2);
    }

    [Fact]
    public static void TranslateDirection()
    {
        const float DIAGONAL_MAGNITUDE =  0.70710678118f;

        Point p = new Point(1f, 2f);

        p.Translate(new Direction(1f, 1f), 3f);

        Assert.Equal(new Point(1f + (DIAGONAL_MAGNITUDE * 3f), 2f + (DIAGONAL_MAGNITUDE * 3f)), p);
        Assert.Equal(new Point(1f + (DIAGONAL_MAGNITUDE * 1f), 2f + (DIAGONAL_MAGNITUDE * 5f)), p.GetTranslated(new Direction(-1f, 1f), 2f));
        Assert.Equal(new Point(1f + (DIAGONAL_MAGNITUDE * 3f), 2f + (DIAGONAL_MAGNITUDE * 3f)), p);
    }

    [Fact]
    public static void RotateRadians()
    {
        Point p = new Point(1f, 2f);

        p.RotateRadians(MathF.PI * 0.5f);
        Assert.Equal(new Point(-2f, 1f), p);

        p.RotateRadians(MathF.PI);
        Assert.Equal(new Point(2f, -1f), p);

        Assert.Equal(new Point(1f, 2f), p.GetRotatedRadians(MathF.PI * -1.5f));
        Assert.Equal(new Point(2f, -1f), p);
    }

    [Fact]
    public static void RotateRadiansPivot()
    {
        Point p = new Point(1f, 2f);

        p.RotateRadians(new Point(-1f, -2f), MathF.PI * 0.5f);
        Assert.Equal(new Point(-5f, 0f), p);

        Assert.Equal(new Point(3f, -4f), p.GetRotatedRadians(new Point(-1f, -2f), -MathF.PI));
        Assert.Equal(new Point(-5f, 0f), p);
    }

    [Fact]
    public static void RotateDegrees()
    {
        Point p = new Point(1f, 2f);

        p.RotateDegrees(90f);
        Assert.Equal(new Point(-2f, 1f), p);

        p.RotateDegrees(180f);
        Assert.Equal(new Point(2f, -1f), p);

        Assert.Equal(new Point(1f, 2f), p.GetRotatedDegrees(-270f));
        Assert.Equal(new Point(2f, -1f), p);
    }

    [Fact]
    public static void RotateDegreesPivot()
    {
        Point p = new Point(1f, 2f);

        p.RotateDegrees(new Point(-1f, -2f), 90);
        Assert.Equal(new Point(-5f, 0f), p);

        Assert.Equal(new Point(3f, -4f), p.GetRotatedDegrees(new Point(-1f, -2f), -180f));
        Assert.Equal(new Point(-5f, 0f), p);
    }

    [Fact]
    public static void Scale()
    {
        Point p1 = new Point(1f, 2f);

        p1.Scale(0.5f);
        Assert.Equal(new Point(0.5f, 1f), p1);

        p1.Scale(0.5f, 0.1f);
        Assert.Equal(new Point(0.25f, 0.1f), p1);

        Point p2 = new Point(-3f, -4f);

        Assert.Equal(new Point(-1.5f, -2f), p2.GetScaled(0.5f));
        Assert.Equal(new Point(-3f, -4f), p2);

        Assert.Equal(new Point(-1.5f, -0.4f), p2.GetScaled(0.5f, 0.1f));
        Assert.Equal(new Point(-3f, -4f), p2);
    }

    [Fact]
    public static void ScalePivot()
    {
        Point p1 = new Point(1f, 2f);

        p1.Scale(new Point(-3f, -4f), 0.5f);
        Assert.Equal(new Point(-1f, -1f), p1);

        p1.Scale(new Point(-3f, -4f), 0.5f, 0.1f);
        Assert.Equal(new Point(-2f, -3.7f), p1);

        Point p2 = new Point(-3f, -4f);

        Assert.Equal(new Point(-1f, -1f), p2.GetScaled(new Point(1f, 2f), 0.5f));
        Assert.Equal(new Point(0.6f, 0.8f), p2.GetScaled(new Point(1f, 2f), 0.1f, 0.2f));
        Assert.Equal(new Point(-3f, -4f), p2);
    }
}
