using NUnit.Framework;
using Shouldly;

namespace BattleshipTests;

public class ShipTests
{
    [Test]
    public void Create_horizontal_destroyer_should_success()
    {
        var actual = new Destroyer("destroyer", new Position('A', 1), ShipAlignment.Horizontal);
        actual.Positions.ShouldBe(new[]
            { new Position('A', 1), new Position('B', 1), new Position('C', 1), new Position('D', 1) });
    }

    [Test]
    public void Create_vertical_destroyer_should_success()
    {
        var actual = new Destroyer("destroyer", new Position('A', 1), ShipAlignment.Vertical);
        actual.Positions.ShouldBe(new[]
            { new Position('A', 1), new Position('A', 2), new Position('A', 3), new Position('A', 4) });
    }

    [Test]
    public void Create_invalid_vertical_destroyer_should_fail()
    {
        Should.Throw<ArgumentException>(() => new Destroyer("destroyer", new Position('A', 8), ShipAlignment.Vertical))
            .Message.ShouldBe("Invalid row, must be 1-10.");
    }

    [Test]
    public void Create_invalid_horizontal_destroyer_should_fail()
    {
        Should.Throw<ArgumentException>(() => new Battleship("destroyer", new Position('H', 1), ShipAlignment.Horizontal))
            .Message.ShouldBe("Invalid column, must be A-J.");
    }
    
    [Test]
    public void Create_horizontal_battleship_should_success()
    {
        var actual = new Battleship("destroyer", new Position('A', 1), ShipAlignment.Horizontal);
        actual.Positions.ShouldBe(new[]
            { new Position('A', 1), new Position('B', 1), new Position('C', 1), new Position('D', 1), new Position('E', 1) });
    }

    [Test]
    public void Create_vertical_battleship_should_success()
    {
        var actual = new Battleship("destroyer", new Position('A', 1), ShipAlignment.Vertical);
        actual.Positions.ShouldBe(new[]
            { new Position('A', 1), new Position('A', 2), new Position('A', 3), new Position('A', 4), new Position('A', 5) });
    }

    [Test]
    public void Create_invalid_vertical_battleship_should_fail()
    {
        Should.Throw<ArgumentException>(() => new Battleship("destroyer", new Position('A', 8), ShipAlignment.Vertical))
            .Message.ShouldBe("Invalid row, must be 1-10.");
    }

    [Test]
    public void Create_invalid_horizontal_battleship_should_fail()
    {
        Should.Throw<ArgumentException>(() => new Battleship("destroyer", new Position('H', 1), ShipAlignment.Horizontal))
            .Message.ShouldBe("Invalid column, must be A-J.");
    }
}