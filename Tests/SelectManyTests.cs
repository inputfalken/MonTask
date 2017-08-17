using System.Threading.Tasks;
using MonTask;
using NUnit.Framework;

// TODO make tests simpler
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
            var flatMapRes = await StringTask.SelectMany(async s => {
                await Task.Delay(100);
                return s + "World";
            });
            Assert.AreEqual("HelloWorld", flatMapRes);
        }

        [Test]
        public async Task String_Task_FlatMap_String_Task_With_ResultSelector() {
            var flatMapRes = await StringTask
                .SelectMany(async s => {
                    await VoidTask;
                    return s + "World";
                }, (s, s1) => s + s1);
            Assert.AreEqual("HelloHelloWorld", flatMapRes);
        }

        [Test]
        public async Task String_Task_FlatMap_Void_Task() {
            var flatmaped = StringTask.SelectMany(s => VoidTask);
            Assert.AreEqual(0, _flatMapCounter);
            await flatmaped;
            Assert.AreEqual(1, _flatMapCounter);
        }

        [Test]
        public async Task String_Task_FlatMap_Void_Task_With_ResultSelector() {
            var flatmaped = StringTask.SelectMany(s => VoidTask, s => s + 1);
            Assert.AreEqual(0, _flatMapCounter);
            Assert.AreEqual("Hello1", await flatmaped);
            Assert.AreEqual(1, _flatMapCounter);
        }

        [Test]
        public async Task Void_Task_FlatMap_String_Task() {
            var flatmap = VoidTask.SelectMany(() => StringTask);
            Assert.AreEqual(0, _flatMapCounter);
            Assert.AreEqual("Hello", await flatmap);
            Assert.AreEqual(1, _flatMapCounter);
        }

        [Test]
        public async Task Void_Task_FlatMap_String_Task_Using_ResultSelector() {
            var flatmap = VoidTask.SelectMany(() => StringTask, s => s + 1);
            Assert.AreEqual(0, _flatMapCounter);
            Assert.AreEqual("Hello1", await flatmap);
            Assert.AreEqual(1, _flatMapCounter);
        }

        [Test]
        public async Task Void_Task_FlatMap_Void_Task() {
            var flatMap = VoidTask.SelectMany(() => VoidTask);
            Assert.AreEqual(0, _flatMapCounter);
            await flatMap;
            Assert.AreEqual(2, _flatMapCounter);
        }

        [Test]
        public async Task Void_Task_FlatMap_Void_Task_Using_ResultSelector() {
            var flatMap = VoidTask.SelectMany(() => VoidTask, () => "Hello");
            Assert.AreEqual(0, _flatMapCounter);
            Assert.AreEqual("Hello", await flatMap);
            Assert.AreEqual(2, _flatMapCounter);
        }
    }
}