using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ThroughAThousandEyes.CombatModule
{
    public class Wave : IEnumerable<Unit>
    {
        private List<Unit> _enemies = new List<Unit>();

        public Wave()
        {
            
        }

        public Wave(IEnumerable<Unit> enemies)
        {
            _enemies = enemies.ToList();
        }

        public Wave(params Unit[] enemies) : this((IEnumerable<Unit>) enemies) { }
        
        public IEnumerator<Unit> GetEnumerator()
        {
            return _enemies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _enemies).GetEnumerator();
        }

        public void AddEnemy(Unit enemy) => _enemies.Add(enemy);

        public Unit this[int i] => _enemies[i];
    }
}