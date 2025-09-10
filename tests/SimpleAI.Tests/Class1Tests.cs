using NUnit.Framework;

namespace SimpleAI.Tests;

public class Class1Tests
{
    [Test]
    public void Add_ReturnsSum()
    {
        var c = new SimpleAI.Class1();
        Assert.That(c.Add(2, 3), Is.EqualTo(5));
    }
}