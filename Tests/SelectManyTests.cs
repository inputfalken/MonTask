using System.Threading.Tasks;
using MonTask;
using NUnit.Framework;
using NUnit.Framework.Internal;

// TODO make tests simpler
namespace Tests {
    [TestFixture]
    internal class SelectManyTests {
        private const string Text = "Hello";

        private static Task<string> StringTask => Task.Run(async () => {
            await Task.Delay(100);
            return Text;
        });

        private int FlatMapCounter;

        [TearDown]
        public void Reset() {
            FlatMapCounter = 0;
        }

        private Task VoidTask => Task.Run(async () => {
            await Task.Delay(100);
            FlatMapCounter++;
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
            Assert.AreEqual(0, FlatMapCounter);
            await flatmaped;
            Assert.AreEqual(1, FlatMapCounter);
        }

        [Test]
        public async Task Void_Task_FlatMap_String_Task() { }

        [Test]
        public async Task Void_Task_FlatMap_Void_Task() { }
    }
}