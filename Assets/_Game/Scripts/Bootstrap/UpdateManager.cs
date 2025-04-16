using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IUpdatable
    {
        void OnUpdate(float deltaTime);
    }

    public class UpdateManager : MonoBehaviour
    {
        private readonly List<IUpdatable> _updatables = new();

        private void Update()
        {
            foreach (var updatable in _updatables)
                updatable.OnUpdate(Time.deltaTime);
        }

        public void Register(IUpdatable updatable) =>
            _updatables.Add(updatable);

        public void Unregister(IUpdatable updatable) =>
            _updatables.Remove(updatable);
    }
}
