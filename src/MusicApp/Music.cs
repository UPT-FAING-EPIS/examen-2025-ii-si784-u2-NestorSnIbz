namespace MusicApp;

public interface IMusicCommand
{
    string Execute();
}

// Receiver - MusicPlayer
public class MusicPlayer
{
    public string Play()
    {
        return "Playing the song.";
    }
    public string Pause()
    {
        return "Pausing the song.";
    }
    public string Skip()
    {
        return "Skipping to the next song.";
    }
}

// Concrete Commands
public class PlayCommand : IMusicCommand
{
    private readonly MusicPlayer _player;
    public PlayCommand(MusicPlayer player)
    {
        _player = player;
    }
    public string Execute()
    {
        return _player.Play();
    }
}

public class PauseCommand : IMusicCommand
{
    private readonly MusicPlayer _player;
    public PauseCommand(MusicPlayer player)
    {
        _player = player;
    }
    public string Execute()
    {
        return _player.Pause();
    }
}

public class SkipCommand : IMusicCommand
{
    private readonly MusicPlayer _player;
    public SkipCommand(MusicPlayer player)
    {
        _player = player;
    }
    public string Execute()
    {
        return _player.Skip();
    }
}

// Invoker - MusicRemote
public class MusicRemote
{
    private IMusicCommand? _command;
    public void SetCommand(IMusicCommand command)
    {
        _command = command;
    }
    public string PressButton()
    {
        if (_command is null)
            throw new InvalidOperationException("No command set.");
        return _command.Execute();
    }
}