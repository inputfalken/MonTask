using System;
using System.Threading.Tasks;
using MonTask;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    internal class WhereTests {
        private const string Text = "hello";

        private static Task<string> StringTask => Task.Run(async () => {
            await Task.Delay(100);
            return Text;
        });

        [Test]
        public async Task True_Predicate_Has_Value() {
            var option = await StringTask.Where(s => s.Length == 5);
            Assert.AreEqual(true, option.HasValue);
        }

        [Test]
        public async Task False_Predicate_Has_No_Value() {
            var option = await StringTask.Where(s => s.Length == 20);
            Assert.AreEqual(false, option.HasValue);
        }

        [Test]
        public void Null_Predicate_Throws() {
            var stringTask = StringTask;
            stringTask = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => stringTask.Where(s => true));
        }

        [Test]
        public void Null_Task_Throws() {
            Func<string, bool> predicate = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => StringTask.Where(predicate));
        }
    }
}