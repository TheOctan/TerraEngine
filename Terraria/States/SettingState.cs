using GameEngine.Core;
using GameEngine.Event;
using GameEngine.GUI;
using GameEngine.Resource;
using GameEngine.States.StateMachine;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine.States
{
    class SettingState : StateBase
    {
        private RectangleShape background;

        private StackMenu menu;
        private ScrollBar musicBar;
        private ScrollBar soundBar;
        private Lock fullScreenLocker;
        private TextBox nickNameTextBox1;
        private TextBox nickNameTextBox2;
        private Button resetButton;
        private Button backButton;

        public SettingState(Game game) : base(game)
        {}

        public override void Init()
        {
            background = new RectangleShape();
            background.Size = new Vector2f(Game.Window.Size.X, Game.Window.Size.Y);
            background.Texture = ResourceHolder.Textures.Get("Background/orig");

            musicBar = new ScrollBar("Music");
            musicBar.Texture = ResourceHolder.Textures.Get("Widget/scrollBar");
			musicBar.ValueChangedEvent += OnMusicBarValueChanged;
            musicBar.Value = game.config.Music;

            soundBar = new ScrollBar("Sound");
            soundBar.Texture = ResourceHolder.Textures.Get("Widget/scrollBar");
			soundBar.ValueChangedEvent += OnSoundBarValueChanged;
            soundBar.Value = game.config.Value;

            fullScreenLocker = new Lock("Full screen mode");
            fullScreenLocker.Texture = ResourceHolder.Textures.Get("Widget/lock");
			fullScreenLocker.PressedEvent += OnFullscreenPressed;
            fullScreenLocker.Value = game.config.FullScreen;

            nickNameTextBox1 = new TextBox(game.config.NickName1);
            nickNameTextBox1.Texture = ResourceHolder.Textures.Get("Widget/textBox");
			nickNameTextBox1.TextChangedEvent += OnNicknameTextChanged1;

            nickNameTextBox2 = new TextBox(game.config.NickName2);
            nickNameTextBox2.Texture = ResourceHolder.Textures.Get("Widget/textBox");
            nickNameTextBox2.TextChangedEvent += OnNicknameTextChanged2;
            nickNameTextBox2.SetActive(false);

            resetButton = new Button("Reset");
            resetButton.Texture = ResourceHolder.Textures.Get("Widget/button");
			resetButton.PressedEvent += OnResetButtonPressed;

            backButton = new Button("Back");
            backButton.Texture = ResourceHolder.Textures.Get("Widget/button");
			backButton.PressedEvent += OnBackButtonPressed;            

            menu = new StackMenu();
            menu.Title = "Settings";
            menu.Texture = ResourceHolder.Textures.Get("Widget/demo_background");
            menu.FillColor = new Color(0, 0, 0, 150);

            menu.AddWidget(musicBar);
            menu.AddWidget(soundBar);
            menu.AddWidget(fullScreenLocker);
            menu.AddWidget(nickNameTextBox1);
            menu.AddWidget(nickNameTextBox2);
            menu.AddWidget(resetButton);
            menu.AddWidget(backButton);

            menu.Subscribe();

            menu.Origin = menu.Size / 2f;
            menu.Position = new Vector2f(Game.Window.Size.X / 2f, Game.Window.Size.Y / 2f);
        }

        private void OnResetButtonPressed(object sender, WidgetEventArgs e)
        {
            game.config.ResetConfiguration();

            musicBar.Value = game.config.Music;
            soundBar.Value = game.config.Value;
            nickNameTextBox1.Text = game.config.NickName1;
            nickNameTextBox2.Text = game.config.NickName2;
        }

        public override void Render(float alpha)
        {
            Game.Window.Draw(background);
            Game.Window.Draw(menu);
        }

        public override void Update(Time time)
        {
            menu.Update();
        }

        private void OnMusicBarValueChanged(object sender, WidgetEventArgs e)
        {
            game.config.Music = e.Value;
        }

        private void OnSoundBarValueChanged(object sender, WidgetEventArgs e)
        {
            game.config.Value = e.Value;
        }

        private void OnFullscreenPressed(object sender, WidgetEventArgs e)
        {
            game.config.FullScreen = fullScreenLocker.Value;

            if (game.config.FullScreen)
            {
                Game.Window.Close();
                Game.Window = new RenderWindow(new VideoMode(1366, 768), Game.GameName, Styles.Fullscreen);
                Game.Window.SetFramerateLimit(60);

                game.InitEvents();
                menu.Subscribe();
            }
            else
            {
                Game.Window.Close();
                Game.Window = new RenderWindow(new VideoMode(1366, 768), Game.GameName, Styles.Close);
                Game.Window.SetFramerateLimit(60);

                game.InitEvents();
                menu.Subscribe();
            }
        }

        private void OnNicknameTextChanged1(object sender, WidgetEventArgs e)
        {
            game.config.NickName1 = e.Message;
        }

        private void OnNicknameTextChanged2(object sender, WidgetEventArgs e)
        {
            game.config.NickName2 = e.Message;
        }

        private void OnBackButtonPressed(object sender, Event.WidgetEventArgs e)
        {
            menu.Unsubscribe();

            game.config.SaveConfiguration();
            Game.stateMachine.PopState();
        }
    }
}
