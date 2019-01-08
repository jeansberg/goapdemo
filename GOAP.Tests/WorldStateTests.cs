using Core;
using Core.AI.Goals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Goap.Tests
{
    [TestClass]
    public class WorldStateTests
    {
        private class FakeCondition1 : ICondition
        {
        }

        private class FakeCondition2 : ICondition
        {
        }

        [TestMethod]
        public void GetUpdatedState_ReturnsCorrectState_ForNewCondition()
        {
            var fake1 = new FakeCondition1();
            var fake2 = new FakeCondition2();

            var originalState = new WorldState(new Dictionary<ICondition, bool> { { fake1, true } });
            var changes = new WorldState(new Dictionary<ICondition, bool> { { fake2, false } });

            var updatedState = originalState.GetUpdatedState(changes);

            CollectionAssert.AreEqual(updatedState.Conditions, new Dictionary<ICondition, bool> {{ fake1, true }, { fake2, false } });
        }

        [TestMethod]
        public void GetUpdatedState_ReturnsCorrectState_ForUpdatedCondition()
        {
            var fake1 = new FakeCondition1();

            var originalState = new WorldState(new Dictionary<ICondition, bool> { { fake1, true } });
            var changes = new WorldState(new Dictionary<ICondition, bool> { { fake1, false } });

            var updatedState = originalState.GetUpdatedState(changes);

            CollectionAssert.AreEqual(updatedState.Conditions, new Dictionary<ICondition, bool> { { fake1, false } });
        }

        [TestMethod]
        public void GetUpdatedState_ReturnsCorrectState_ForNewAndUpdatedCondition()
        {
            var fake1 = new FakeCondition1();
            var fake2 = new FakeCondition2();

            var originalState = new WorldState(new Dictionary<ICondition, bool> { { fake1, true } });
            var changes = new WorldState(new Dictionary<ICondition, bool> { { fake1, false }, { fake2, true } });

            var updatedState = originalState.GetUpdatedState(changes);

            CollectionAssert.AreEqual(updatedState.Conditions, new Dictionary<ICondition, bool> { { fake1, false }, { fake2, true } });
        }
    }
}
