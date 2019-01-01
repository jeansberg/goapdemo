using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GOAP.Tests
{
    [TestClass]
    public class WorldStateTests
    {
        [TestMethod]
        public void GetUpdatedState_ReturnsCorrectState_ForNewCondition()
        {
            var originalState = new WorldState(new Dictionary<string, bool> { { "state1", true } });
            var changes = new WorldState(new Dictionary<string, bool> { { "state2", false } });

            var updatedState = originalState.GetUpdatedState(changes);

            CollectionAssert.AreEqual(updatedState.Conditions, new Dictionary<string, bool> { { "state1", true }, { "state2", false } });
        }

        [TestMethod]
        public void GetUpdatedState_ReturnsCorrectState_ForUpdatedCondition()
        {
            var originalState = new WorldState(new Dictionary<string, bool> { { "state1", true } });
            var changes = new WorldState(new Dictionary<string, bool> { { "state1", false } });

            var updatedState = originalState.GetUpdatedState(changes);

            CollectionAssert.AreEqual(updatedState.Conditions, new Dictionary<string, bool> { { "state1", false } });
        }

        [TestMethod]
        public void GetUpdatedState_ReturnsCorrectState_ForNewAndUpdatedCondition()
        {
            var originalState = new WorldState(new Dictionary<string, bool> { { "state1", true } });
            var changes = new WorldState(new Dictionary<string, bool> { { "state1", false }, { "state2", true } });

            var updatedState = originalState.GetUpdatedState(changes);

            CollectionAssert.AreEqual(updatedState.Conditions, new Dictionary<string, bool> { { "state1", false }, { "state2", true } });
        }
    }
}
