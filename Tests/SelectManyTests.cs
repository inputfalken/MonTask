using System.Threading.Tasks;
using MonTask;
using NUnit.Framework;

// TODO make tests simpler
namespace Tests {
    [TestFixture]
    internal class SelectManyTests {
        private const string Text = "Hello";

        private static Task<string> GenericTask => Task.Run(async () => {
            await Task.Delay(100);
            return Text;
        });

        private static Task VoidTask => Task.Delay(100);


        [Test]
        public async Task String_Task_FlatMap_String_Task() {
            var flatMapRes = await GenericTask.SelectMany(async s => {
                await Task.Delay(100);
                return s + "World";
            });
            Assert.AreEqual("HelloWorld", flatMapRes);
        }

        [Test]
        public async Task String_Task_FlatMap_String_Task_With_ResultSelector() {
            var flatMapRes = await GenericTask
                .SelectMany(async s => {
                    await VoidTask;
                    return s + "World";
                }, (s, s1) => s + s1);
            Assert.AreEqual("HelloHelloWorld", flatMapRes);
        }

        [Test]
        public async Task String_Task_FlatMap_Void_Task() {
        }

        [Test]
        public async Task Void_Task_FlatMap_String_Task() {
        }

        [Test]
        public async Task Void_Task_FlatMap_Void_Task() {
        }
    }
}