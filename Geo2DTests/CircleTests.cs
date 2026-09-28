namespace Geo2DTests;


public static class CircleTests
{
    [Fact]
    public static void Constructor()
    {
        Circle c1 = new Circle(1f, 2f, 3f);

        Assert.Equal(1f, c1.x);
        Assert.Equal(2f, c1.y);
        Assert.Equal(3f, c1.radius);

        Circle c2 = new Circle(new Point(-3f, -4f), 5f);

        Assert.Equal(-3f, c2.x);
        Assert.Equal(-4f, c2.y);
        Assert.Equal(5f, c2.radius);
    }

    [Fact]
    public static void Equal()
    {
        Circle c1 = new Circle(1f, 2f, 3f);
        Circle c2 = new Circle(-3f, -4f, 5f);

        Assert.True(c1 == new Circle(1f, 2f, 3f));
        Assert.True(c2 == new Circle(-3f, -4f, 5f));
        Assert.False(c1 == c2);

        Assert.True(c1.Equals(new Circle(1f, 2f, 3f)));
        Assert.True(c2.Equals(new Circle(-3f, -4f, 5f)));
        Assert.False(c1.Equals(c2));

        Assert.False(c1.Equals(new object()));

        Assert.False(c1 == new Circle(1f, 2f, 4f));
        Assert.False(c1.Equals(new Circle(1f, 2f, 4f)));
    }

    [Fact]
    public static void NotEqual()
    {
        Circle c1 = new Circle(1f, 2f, 3f);
        Circle c2 = new Circle(-3f, -4f, 5f);

        Assert.False(c1 != new Circle(1f, 2f, 3f));
        Assert.False(c2 != new Circle(-3f, -4f, 5f));
        Assert.True(c1 != c2);

        Assert.True(c1 != new Circle(1f, 2f, 4f));
    }

    [Fact]
    public static void Translate()
    {
        Circle c1 = new Circle(1f, 2f, 5f);
        Point translate = new Point(-3f, 4f);

        Circle c = c1;
        c.Translate(-3f, 4f);
        Assert.Equal(new Circle(-2f, 6f, 5f), c);

        c = c1;
        c.Translate(translate);
        Assert.Equal(new Circle(-2f, 6f, 5f), c);

        c = c1;
        Assert.Equal(new Circle(-2f, 6f, 5f), c.GetTranslated(-3f, 4f));
        Assert.Equal(new Circle(-2f, 6f, 5f), c.GetTranslated(translate));
        Assert.Equal(c1, c);

        Assert.Equal(new Circle(-2f, 6f, 5f), c1 + translate);
        Assert.Equal(new Circle(4f, -2f, 5f), c1 - translate);
    }

    [Fact]
    public static void TranslateDirection()
    {
        const float DIAGONAL_MAGNITUDE =  0.70710678118f;

        Circle c = new Circle(1f, 2f, 5f);

        c.Translate(new Direction(1f, 1f), 3f);

        Assert.Equal(new Circle(1f + (DIAGONAL_MAGNITUDE * 3f), 2f + (DIAGONAL_MAGNITUDE * 3f), 5f), c);
        Assert.Equal(new Circle(1f + (DIAGONAL_MAGNITUDE * 1f), 2f + (DIAGONAL_MAGNITUDE * 5f), 5f), c.GetTranslated(new Direction(-1f, 1f), 2f));
        Assert.Equal(new Circle(1f + (DIAGONAL_MAGNITUDE * 3f), 2f + (DIAGONAL_MAGNITUDE * 3f), 5f), c);
    }

    [Fact]
    public static void RotateRadians()
    {
        Circle c = new Circle(1f, 2f, 5f);

        c.RotateRadians(MathF.PI * 0.5f);
        Assert.Equal(new Circle(-2f, 1f, 5f), c);

        c.RotateRadians(MathF.PI);
        Assert.Equal(new Circle(2f, -1f, 5f), c);

        Assert.Equal(new Circle(1f, 2f, 5f), c.GetRotatedRadians(MathF.PI * -1.5f));
        Assert.Equal(new Circle(2f, -1f, 5f), c);
    }

    [Fact]
    public static void RotateRadiansPivot()
    {
        Circle c = new Circle(1f, 2f, 5f);

        c.RotateRadians(new Point(-1f, -2f), MathF.PI * 0.5f);
        Assert.Equal(new Circle(-5f, 0f, 5f), c);

        Assert.Equal(new Circle(3f, -4f, 5f), c.GetRotatedRadians(new Point(-1f, -2f), -MathF.PI));
        Assert.Equal(new Circle(-5f, 0f, 5f), c);
    }

    [Fact]
    public static void RotateDegrees()
    {
        Circle c = new Circle(1f, 2f, 5f);

        c.RotateDegrees(90f);
        Assert.Equal(new Circle(-2f, 1f, 5f), c);

        c.RotateDegrees(180f);
        Assert.Equal(new Circle(2f, -1f, 5f), c);

        Assert.Equal(new Circle(1f, 2f, 5f), c.GetRotatedDegrees(-270f));
        Assert.Equal(new Circle(2f, -1f, 5f), c);
    }

    [Fact]
    public static void RotateDegreesPivot()
    {
        Circle c = new Circle(1f, 2f, 5f);

        c.RotateDegrees(new Point(-1f, -2f), 90);
        Assert.Equal(new Circle(-5f, 0f, 5f), c);

        Assert.Equal(new Circle(3f, -4f, 5f), c.GetRotatedDegrees(new Point(-1f, -2f), -180f));
        Assert.Equal(new Circle(-5f, 0f, 5f), c);
    }

    [Fact]
    public static void Scale()
    {
        Circle c1 = new Circle(1f, 2f, 5f);

        c1.Scale(0.5f);
        Assert.Equal(new Circle(0.5f, 1f, 2.5f), c1);

        c1.Scale(0.5f, 0.1f, 0.2f);
        Assert.Equal(new Circle(0.25f, 0.1f, 0.5f), c1);

        Circle c2 = new Circle(-3f, -4f, 6f);

        Assert.Equal(new Circle(-1.5f, -2f, 3f), c2.GetScaled(0.5f));
        Assert.Equal(new Circle(-3f, -4f, 6f), c2);

        Assert.Equal(new Circle(-1.5f, -0.4f, 1.2f), c2.GetScaled(0.5f, 0.1f, 0.2f));
        Assert.Equal(new Circle(-3f, -4f, 6f), c2);
    }

    [Fact]
    public static void ScalePivot()
    {
        Circle c1 = new Circle(1f, 2f, 5f);

        c1.Scale(new Point(-3f, -4f), 0.5f);
        Assert.Equal(new Circle(-1f, -1f, 2.5f), c1);

        c1.Scale(new Point(-3f, -4f), 0.5f, 0.1f, 0.2F);
        Assert.Equal(new Circle(-2f, -3.7f, 0.5f), c1);

        Circle c2 = new Circle(-3f, -4f, 6f);

        Assert.Equal(new Circle(-1f, -1f, 3f), c2.GetScaled(new Point(1f, 2f), 0.5f));
        Assert.Equal(new Circle(0.6f, 0.8f, 1.8f), c2.GetScaled(new Point(1f, 2f), 0.1f, 0.2f, 0.3f));
        Assert.Equal(new Circle(-3f, -4f, 6f), c2);
    }
}
