using MusicApp;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace MusicApp.BddTests.Steps;

[Binding]
public class MusicRemoteSteps
{
    private MusicPlayer? _player;
    private MusicRemote? _remote;
    private string? _result;

    [Given("un reproductor de música")]
    public void GivenUnReproductorDeMusica()
    {
        _player = new MusicPlayer();
    }

    [Given("un control remoto de música")]
    public void GivenUnControlRemotoDeMusica()
    {
        _remote = new MusicRemote();
    }

    [Given(@"el comando (.*) configurado en el control remoto")]
    public void GivenElComandoConfiguradoEnElControlRemoto(string command)
    {
        Assert.NotNull(_player);
        Assert.NotNull(_remote);
        IMusicCommand cmd = command switch
        {
            "Play" => new PlayCommand(_player!),
            "Pause" => new PauseCommand(_player!),
            "Skip" => new SkipCommand(_player!),
            _ => throw new ArgumentOutOfRangeException(nameof(command))
        };
        _remote!.SetCommand(cmd);
    }

    [When("presiono el botón del control remoto")]
    public void WhenPresionoElBotonDelControlRemoto()
    {
        _result = _remote!.PressButton();
    }

    [Then(@"recibo el mensaje (.*)")]
    public void ThenReciboElMensaje(string expected)
    {
        Assert.That(_result, Is.EqualTo(expected));
    }
}