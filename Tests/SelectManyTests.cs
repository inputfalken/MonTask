﻿using System;
using System.Threading.Tasks;
using MonTask;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    internal class SelectManyTests {
        [TearDown]
        public void Reset() {
            _flatMapCounter = 0;
        }

        private const string Text = "Hello";

        private static Task<string> StringTask => Task.Run(async () => {
            await Task.Delay(100);
            return Text;
        });

        private int _flatMapCounter;

        private Task VoidTask => Task.Run(async () => {
            await Task.Delay(100);
            _flatMapCounter++;
        });

        [Test]
        public async Task String_Task_Catch_Exception() {
            var select = StringTask.SelectMany(s => throw new Exception("Exception"));
            try {
                await select;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

        [Test]
        public async Task String_Task_Catch_Exception_With_ResultSelector() {
            var select = StringTask.SelectMany(s => throw new Exception("Exception"), s => s.Length);
            try {
                await select;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

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
        public async Task String_Task_Run_Catch_Exception() {
            var select = StringTask.SelectMany<string, int>(s => throw new Exception("Exception"));
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

        [Test]
        public async Task String_Task_Run_Catch_Exception_With_ResultSelector() {
            var select = StringTask.SelectMany(s => throw new Exception("Exception"), s => s.Length);
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
            var select = VoidTask.SelectMany(() => throw new Exception("Exception"));
            try {
                await select;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

        [Test]
        public async Task Void_Task_Catch_Exception_With_ResultSelector() {
            var select = VoidTask.SelectMany(() => throw new Exception("Exception"), () => 5);
            try {
                await select;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
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

        [Test]
        public async Task Void_Task_Run_Catch_Exception() {
            var select = VoidTask.SelectMany(() => throw new Exception("Exception"));
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }

        [Test]
        public async Task Void_Task_Run_Catch_Exception_With_ResultSelector() {
            var select = VoidTask.SelectMany(() => throw new Exception("Exception"), () => 5);
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.AreEqual("Exception", e.Message);
            }
        }
    }
}