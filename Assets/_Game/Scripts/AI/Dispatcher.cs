using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class Dispatcher
    {
        private readonly Character[] _workers;
        private readonly Building _silo;
        private readonly Inventory _inventory;

        private readonly Queue<Field> _waitingFields = new();

        public Dispatcher(Field[] fields, Character[] characters, Building silo, Inventory inventory)
        {
            _inventory = inventory;
            _workers = characters;
            foreach (var worker in _workers)
                worker.OnStateChanged += Worker_OnStateChanged;
            _silo = silo;

            foreach (var field in fields)
            {
                field.OnStateChanged += Field_OnStateChanged;
            }
        }

        private void Worker_OnStateChanged(Character worker)
        {
            if (worker.IsFree())
                AssignWorker();
        }

        private void Field_OnStateChanged(Field field)
        {
            if (field.GetState() != Field.State.HarvestReady) return;
            _waitingFields.Enqueue(field);
            AssignWorker();
        }

        private void AssignWorker()
        {
            if (!_waitingFields.Any()) return;

            var worker = _workers.FirstOrDefault(c => c.IsFree());
            if (worker == null) return;

            var field = _waitingFields.Dequeue();

            worker.MoveTo(field.transform.position, OnComplete);

            void OnComplete(Character worker)
            {
                var resource = field.Grab();

                worker.ApplyResource(resource);
                worker.MoveTo(_silo.transform.position, OnSiloReached);
            }
        }

        private void OnSiloReached(Character worker)
        {
            var resource = worker.GrabResource();
            _inventory.AddItem(resource.Id, resource.Value);
        }
    }
}
