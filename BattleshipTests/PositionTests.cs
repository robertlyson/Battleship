using NUnit.Framework;
using Shouldly;

namespace BattleshipTests;

public class PositionTests
{
    [Test]
    public void Move_by_one_column_should_success()
    {
        var actual = new Position('A', 1).MoveByOneColumn();
        actual.ShouldBe(new Position('B', 1));
    }

    [Test]
    public void Move_by_one_row_should_success()
    {
        var actual = new Position('A', 1).MoveByOneRow();
        actual.ShouldBe(new Position('A', 2));
    }

    [Test]
    public void Move_too_many_rows_should_fail()
    {
        var sut = new Position('A', 1);
        var actual = sut;
        Should.Throw<ArgumentException>(() =>
        {
            for (int i = 0; i < 100; i++)
            {
                actual = actual.MoveByOneRow();
            }
        }).Message.ShouldBe("Invalid row, must be 1-10.");
    }

    [Test]
    public void Move_too_many_columns_should_fail()
    {
        var sut = new Position('A', 1);
        var actual = sut;
        Should.Throw<ArgumentException>(() =>
        {
            for (int i = 0; i < 100; i++)
            {
                actual = actual.MoveByOneColumn();
            }
        }).Message.ShouldBe("Invalid column, must be A-J.");
    }
}