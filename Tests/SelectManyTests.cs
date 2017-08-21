using System.Threading.Tasks;
using MonTask;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    internal class SelectManyTests {
        private const string Text = "Hello";

        private static Task<string> StringTask => Task.Run(async () => {
            await Task.Delay(100);
            return Text;
        });

        private int _flatMapCounter;

        [TearDown]
        public void Reset() {
            _flatMapCounter = 0;
        }

        private Task VoidTask => Task.Run(async () => {
            await Task.Delay(100);
            _flatMapCounter++;
        });

        [Test]
        public async Task String_Task_FlatMap_String_Task() {
            var flatMapped = await StringTask.SelectMany(async s => {
                await Task.Delay(100);
                return s + "World";
            });
            Assert.AreEqual("HelloWorld", flatMapped);
        }

        [Test]
        public async Task String_Task_FlatMap_String_Task_With_ResultSelector() {
            var flatMapped = await StringTask
                .SelectMany(async s => {
                    await VoidTask;
                    return s + "World";
                }, (s, s1) => s + s1);
            Assert.AreEqual("HelloHelloWorld", flatMapped);
        }

        [Test]
        public async Task String_Task_FlatMap_Void_Task() {
            var flatMapped = StringTask.SelectMany(s => VoidTask);
            Assert.AreEqual(0, _flatMapCounter);
            await flatMapped;
            Assert.AreEqual(1, _flatMapCounter);
        }

        [Test]
        public async Task String_Task_FlatMap_Void_Task_With_ResultSelector() {
            var flatMapped = StringTask.SelectMany(s => VoidTask, s => s + 1);
            Assert.AreEqual(0, _flatMapCounter);
            Assert.AreEqual("Hello1", await flatMapped);
            Assert.AreEqual(1, _flatMapCounter);
        }

        [Test]
        public async Task Void_Task_FlatMap_String_Task() {
            var flatMapped = VoidTask.SelectMany(() => StringTask);
            Assert.AreEqual(0, _flatMapCounter);
            Assert.AreEqual("Hello", await flatMapped);
            Assert.AreEqual(1, _flatMapCounter);
        }

        [Test]
        public async Task Void_Task_FlatMap_String_Task_Using_ResultSelector() {
            var flatMapped = VoidTask.SelectMany(() => StringTask, s => s + 1);
            Assert.AreEqual(0, _flatMapCounter);
            Assert.AreEqual("Hello1", await flatMapped);
            Assert.AreEqual(1, _flatMapCounter);
        }

        [Test]
        public async Task Void_Task_FlatMap_Void_Task() {
            var flatMapped = VoidTask.SelectMany(() => VoidTask);
            Assert.AreEqual(0, _flatMapCounter);
            await flatMapped;
            Assert.AreEqual(2, _flatMapCounter);
        }

        [Test]
        public async Task Void_Task_FlatMap_Void_Task_Using_ResultSelector() {
            var flatMapped = VoidTask.SelectMany(() => VoidTask, () => "Hello");
            Assert.AreEqual(0, _flatMapCounter);
            Assert.AreEqual("Hello", await flatMapped);
            Assert.AreEqual(2, _flatMapCounter);
        }
    }
}