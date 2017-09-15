using System;
using System.Threading.Tasks;
using MonTask;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    internal class SelectTests {
        private const string Text = "hello";

        private static Task<string> StringTask => Task.Run(async () => {
            await Task.Delay(100);
            return Text;
        });

        [Test]
        public async Task Catch_Exception() {
            var select = StringTask.Select<string, int>(s => throw new Exception("Exception"));
            try {
                await select;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

        [Test]
        public void Null_String_Task() {
            Task<string> task = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => task.Select(s => s.Length));
        }

        [Test]
        public void Null_Task() {
            Task task = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => task.Select(() => Text));
        }

        [Test]
        public void String_Task_Null_Selector() {
            Func<string, int> selector = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => StringTask.Select(selector));
        }

        [Test]
        public void Task_Null_Selector() {
            Func<int> selector = null;
            var task = Task.Delay(100);
            Assert.ThrowsAsync<ArgumentNullException>(() => task.Select(selector));
        }

        [Test]
        public async Task Task_Run_Catch_Exception() {
            var select = StringTask.Select<string, int>(s => throw new Exception("Exception"));
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

        [Test]
        public async Task Transform_String_Task_To_Length() {
            var length = await StringTask.Select(s => s.Length);
            Assert.AreEqual(Text.Length, length);
        }

        [Test]
        public async Task Transform_Void_Task_To_String_Task() {
            var text = await Task.Delay(100).Select(() => Text);
            Assert.AreEqual(Text, text);
        }
    }
}