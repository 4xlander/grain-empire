using System;
using System.Collections.Generic;

namespace Game
{
    public class BuildingsManager
    {
        private readonly Balance _balance;
        private readonly List<BuildingData> _buildingsData = new();
        private readonly Dictionary<int, Building> _buildings = new();

        public event EventHandler OnConstruct;

        public BuildingsManager(Balance balance, List<BuildingData> buildingsData, Building[] buildings)
        {
            _balance = balance;
            _buildingsData = buildingsData;

            foreach (var building in buildings)
            {
                _buildings.Add(building.GetPlotId(), building);
                building.OnClick += Building_OnClick;
            }

            foreach (var buildingData in _buildingsData)
                if (_buildings.TryGetValue(buildingData.PlotId, out var building))
                    building.Build();
        }

        private void Building_OnClick(Building building)
        {
            if (building == null
                || _buildingsData.Exists(x => x.PlotId == building.GetPlotId())) return;

            var config = building.GetBuilidngConfig();

            if (!_balance.TrySpendMoney(config.Price)) return;

            building.Build();
            var plotId = building.GetPlotId();
            var newBuilding = new BuildingData
            {
                Id = config.Id,
                Level = 1,
                PlotId = building.GetPlotId(),
            };

            _buildingsData.Add(newBuilding);

            OnConstruct?.Invoke(this, EventArgs.Empty);
        }
    }
}
