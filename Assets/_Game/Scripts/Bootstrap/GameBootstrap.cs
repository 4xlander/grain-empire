using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private GameConfigSO _gameConfig;
        [SerializeField] private UpdateManager _updateManager;
        [SerializeField] private Field[] _fields;
        [SerializeField] private Building[] _buildings;
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
            foreach (var building in _buildings)
                _updateManager.Register(building);

            var fieldsManager = new FieldsManager(balance, _gameState.Fields, _fields, inventory);
            foreach (var field in _fields)
                _updateManager.Register(field);

            _gameplayUI.Construct(balance);
        }

        private void OnApplicationQuit()
        {
            SaveManager.SaveGame(_gameState);
        }
    }
}
