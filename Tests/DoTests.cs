using System;
using System.Threading.Tasks;
using MonTask;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    internal class DoTests {
        private const string Text = "Hello";

        private static Task<string> StringTask => Task.Run(async () => {
            await Task.Delay(100);
            return Text;
        });

        private static Task VoidTask => Task.Run(async () => { await Task.Delay(100); });

        [Test]
        public async Task String_Task_Assign_Variable_In_Outer_Scope() {
            var stringLength = 0;
            var task = StringTask.Do(s => stringLength = s.Length);
            Assert.AreEqual(0, stringLength);
            var str = await task;
            Assert.AreEqual(str.Length, stringLength);
        }

        [Test]
        public async Task Void_Task_Assign_Variable_In_Outer_Scope() {
            var stringLength = 0;
            var task = VoidTask.Do(() => stringLength = 5);
            Assert.AreEqual(0, stringLength);
            await task;
            Assert.AreEqual(5, stringLength);
        }

        [Test]
        public async Task String_Task_Catch_Exception() {
            var select = StringTask.Do(s => throw new Exception("Exception"));
            try {
                await select;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

        [Test]
        public async Task String_Task_Run_Catch_Exception() {
            var select = StringTask.Do(s => throw new Exception("Exception"));
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

        [Test]
        public async Task Void_Task_Catch_Exception() {
            var select = StringTask.Do(() => throw new Exception("Exception"));
            try {
                await select;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

        [Test]
        public async Task Void_Task_Run_Catch_Exception() {
            var select = StringTask.Do(() => throw new Exception("Exception"));
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

        [Test]
        public void Null_String_Task_Throws() {
            var stringTask = StringTask;
            stringTask = null;
            Assert.Throws<ArgumentNullException>(() => stringTask.Do(s => { }));
        }

        [Test]
        public void Null_Void_Task_Throws() {
            var stringTask = VoidTask;
            stringTask = null;
            Assert.Throws<ArgumentNullException>(() => stringTask.Do(() => { }));
        }

        [Test]
        public void String_Task_Null_Action_Throws() {
            Action<string> action = null;
            Assert.Throws<ArgumentNullException>(() => StringTask.Do(action));
        }

        [Test]
        public void Void_Task_Null_Action_Throws() {
            Action action = null;
            Assert.Throws<ArgumentNullException>(() => VoidTask.Do(action));
        }
    }
}