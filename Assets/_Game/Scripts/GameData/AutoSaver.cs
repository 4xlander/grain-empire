namespace Game
{
    public class AutoSaver : IUpdatable
    {
        private readonly GameData _data;
        private readonly GameConfigSO _config;

        private float _timer;

        public AutoSaver(GameData gameData, GameConfigSO gameConfig)
        {
            _data = gameData;
            _config = gameConfig;
        }

        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer >= _config.AutoSaveTime)
            {
                SaveManager.SaveGame(_data);
                _timer = 0;
            }
        }
    }
}
