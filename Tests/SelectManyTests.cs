using System.Threading.Tasks;
using MonTask;
using NUnit.Framework;

// TODO make tests simpler
namespace Tests {
    [TestFixture]
    internal class SelectManyTests {
        private const string Text = "Hello";

        private static async Task<string> GetString() {
            await Task.Delay(100);
            return Text;
        }

        private const string FlatMapText = "World";

        [Test]
        public async Task String_Task_FlatMap_String_Task() {
            var flatMapRes = await GetString().SelectMany(async s => {
                await Task.Delay(100);
                return s + FlatMapText;
            });
            Assert.AreEqual(Text + FlatMapText, flatMapRes);
        }

        [Test]
        public async Task String_Task_FlatMap_String_Task_With_ResultSelector() {
            var flatMapRes = await GetString()
                .SelectMany(async s => {
                    await Task.Delay(100);
                    return s + FlatMapText;
                }, (s, s1) => s + s1);
            Assert.AreEqual(Text + Text + FlatMapText, flatMapRes);
        }

        [Test]
        public async Task String_Task_FlatMap_Void_Task() {
            var isFlatMapped = false;
            var flatmap = GetString()
                .SelectMany(s => Task.Delay(100))
                .ContinueWith(task => isFlatMapped = true);
            Assert.IsFalse(isFlatMapped);
            await flatmap;
            Assert.IsTrue(isFlatMapped);
        }

        [Test]
        public async Task Void_Task_FlatMap_String_Task() {
            var isFlatMapped = false;
            var flatmap = Task.Delay(100)
                .SelectMany(GetString)
                .ContinueWith(task => {
                    isFlatMapped = true;
                    task.Wait();
                    return task.Result;
                });
            Assert.IsFalse(isFlatMapped);
            Assert.AreEqual(Text, await flatmap);
            Assert.IsTrue(isFlatMapped);
        }

        [Test]
        public async Task Void_Task_FlatMap_Void_Task() {
            var isFlatmapped = false;
            var flatmap = Task.Delay(100)
                .SelectMany(() => Task.Delay(100))
                .ContinueWith(task => isFlatmapped = true);
            Assert.IsFalse(isFlatmapped);
            await flatmap;
            Assert.IsTrue(isFlatmapped);
        }
    }
}