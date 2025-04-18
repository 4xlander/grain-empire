using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private GameConfigSO _gameConfig;
        [SerializeField] private UpdateManager _updateManager;
        [SerializeField] private FarmUpgrades _farmUpgrades;
        [Space]
        [SerializeField] private Field[] _fields;
        [SerializeField] private Building[] _buildings;
        [Space]
        [SerializeField] private Character[] _workers;
        [Header("UI")]
        [SerializeField] private GameplayUI _gameplayUI;
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private Button _inventoryUIButton;

        private GameData _gameState;

        private void Awake()
        {
            _gameState = SaveManager.LoadGame() ?? new GameData(_gameConfig);
            var autoSaver = new AutoSaver(_gameState, _gameConfig);
            _updateManager.Register(autoSaver);

            var balance = new Balance(_gameState);
            var inventory = new Inventory(_gameState.Inventory);
            var inventoryPresenter = new InventoryPresenter(balance, inventory, _inventoryUI, _gameConfig.InventoryItemsConfigs);
            _inventoryUIButton.onClick.AddListener(() => inventoryPresenter.ShowInventory());

            var buildingsManager = new BuildingsManager(balance, _gameState.Buildings, _buildings);

            var fieldsManager = new FieldsManager(balance, _gameState.Fields, _fields, inventory);

            var farmManager = new FarmManager(_gameState, fieldsManager, buildingsManager);
            _farmUpgrades.Inject(farmManager);

            _gameplayUI.Inject(balance);

            var dispatcher = new Dispatcher(_fields, _workers, _buildings[0], inventory);

            foreach (var building in _buildings)
                _updateManager.Register(building);

            foreach (var field in _fields)
                _updateManager.Register(field);
        }

        private void OnApplicationQuit()
        {
            SaveManager.SaveGame(_gameState);
        }
    }
}
