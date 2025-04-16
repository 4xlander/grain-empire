using System;
using System.Collections.Generic;

namespace Game
{
    public class FieldsManager
    {
        private readonly Balance _balance;
        private readonly List<FieldData> _fieldsData = new();
        private readonly Dictionary<int, Field> _fields = new();
        private readonly Inventory _inventory;

        public FieldsManager(Balance balance, List<FieldData> fieldsData, Field[] fields, Inventory inventory)
        {
            _balance = balance;
            _fieldsData = fieldsData;
            _inventory = inventory;

            foreach (var field in fields)
            {
                _fields.Add(field.GetPlotId(), field);
                field.OnClick += Field_OnClick;
            }

            foreach (var fieldData in _fieldsData)
                if (_fields.TryGetValue(fieldData.PlotId, out var field))
                {
                    field.Construct(fieldData, _inventory);
                    field.Build();
                }
        }

        private void Field_OnClick(object sender, EventArgs e)
        {
            if (sender is not Field field
                || _fieldsData.Exists(x => x.PlotId == field.GetPlotId())) return;

            var config = field.GetFieldConfig();

            if (!_balance.TrySpendMoney(config.Price)) return;

            field.Build();
            var plotId = field.GetPlotId();
            var newField = new FieldData
            {
                CropId = config.CropId,
                State = Field.State.Idle,
                Progress = 0,
                PlotId = plotId,
            };

            _fieldsData.Add(newField);
            field.Construct(newField, _inventory);
        }
    }
}
