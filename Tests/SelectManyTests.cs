using System.Threading.Tasks;
using MonTask;
using NUnit.Framework;

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
        public async Task String_Task_Flatmap_String_Task() {
            var flatMapRes = await GetString().SelectMany(async s => {
                await Task.Delay(100);
                return s + FlatMapText;
            });
            Assert.AreEqual(Text + FlatMapText, flatMapRes);
        }
    }
}