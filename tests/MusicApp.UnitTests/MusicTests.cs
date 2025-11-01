using NUnit.Framework;
using MusicApp;

namespace MusicApp.UnitTests;

public class MusicTests
{
    [Test]
    public void PlayCommand_Returns_PlayingMessage()
    {
        var player = new MusicPlayer();
        var cmd = new PlayCommand(player);
        Assert.That(cmd.Execute(), Is.EqualTo("Playing the song."));
    }

    [Test]
    public void PauseCommand_Returns_PausingMessage()
    {
        var player = new MusicPlayer();
        var cmd = new PauseCommand(player);
        Assert.That(cmd.Execute(), Is.EqualTo("Pausing the song."));
    }

    [Test]
    public void SkipCommand_Returns_SkippingMessage()
    {
        var player = new MusicPlayer();
        var cmd = new SkipCommand(player);
        Assert.That(cmd.Execute(), Is.EqualTo("Skipping to the next song."));
    }

    [Test]
    public void Remote_PressButton_Executes_SetCommand()
    {
        var player = new MusicPlayer();
        var remote = new MusicRemote();

        remote.SetCommand(new PlayCommand(player));
        Assert.That(remote.PressButton(), Is.EqualTo("Playing the song."));

        remote.SetCommand(new PauseCommand(player));
        Assert.That(remote.PressButton(), Is.EqualTo("Pausing the song."));

        remote.SetCommand(new SkipCommand(player));
        Assert.That(remote.PressButton(), Is.EqualTo("Skipping to the next song."));
    }

    [Test]
    public void Remote_PressButton_WithoutCommand_Throws()
    {
        var remote = new MusicRemote();
        Assert.Throws<InvalidOperationException>(() => remote.PressButton());
    }
}