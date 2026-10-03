

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace cis_580_game;

public class MusicManager
{
    private Resources _resources;

    public MusicManager(Resources resources)
    {
        _resources = resources;

        _resources.GameStateChangedEvent += Handle_GameStateChangedEvent;

        MediaPlayer.IsRepeating = true;
    }

    public void LoadContent(ContentManager content)
    {
        
    }

    public void Handle_GameStateChangedEvent(object sender, GameStateChangedEventArgs e)
    {
        if (e.NewState == e.OldState) return;
        MediaPlayer.Stop();
        switch(e.NewState)
        {
            case GameState.TitleScreen:
                MediaPlayer.Play(_resources.BackgroundMusic[3], new(0,0,30));
                break;
            case GameState.Playing:
                MediaPlayer.Play(_resources.BackgroundMusic[2]);
                break;
            default:
                break;
        }
    }
}
