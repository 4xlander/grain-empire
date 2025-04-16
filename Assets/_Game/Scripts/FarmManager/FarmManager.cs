using System;

namespace Game
{
    public class FarmManager
    {
        private readonly GameData _gameData;
        private readonly FieldsManager _fieldsManager;
        private readonly BuildingsManager _buildingsManager;

        public event Action<int> OnFarmLevelChanged;

        public FarmManager(GameData gameData, FieldsManager fieldsManager, BuildingsManager buildingsManager)
        {
            _gameData = gameData;
            fieldsManager.OnConstruct += FieldsManager_OnConstruct;
            buildingsManager.OnConstruct += FieldsManager_OnConstruct;
        }

        private void FieldsManager_OnConstruct(object sender, EventArgs e)
        {
            _gameData.FarmLevel++;
            OnFarmLevelChanged?.Invoke(_gameData.FarmLevel);
        }

        public int GetFarmLevel() =>
            _gameData.FarmLevel;
    }
}
