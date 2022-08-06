// See https://aka.ms/new-console-template for more information

using BattleshipGame.Games;
using Spectre.Console;

var console = AnsiConsole.Console;
new App(console).Start(Game.Create());