using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Asynk.Library.Settings;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Asynk.Tests.Library.Settings
{
    /// <summary>
    /// Tests the application settings
    /// </summary>
    [TestFixture]
    public class SettingsTests
    {
        /// <summary>
        /// Setup
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            
        }

        /// <summary>
        /// Clean up
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
        
        }

        /// <summary>
        /// Tests the Queue settings
        /// </summary>
        [Test]
        public void QueueSettingTest()
        {
            Assert.That(AsynkConfig.QueueSettings.DefaultQueue.QueueType.GetInterface("IAsynkQueue") != null);
        }

        /// <summary>
        /// Tests the default values for the Misc Settings
        /// </summary>
        [Test]
        public void MiscSettingTest()
        {
            Assert.That(
                Regex.IsMatch("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", 
                AsynkConfig.Misc.IgnoreFilter));

            Assert.That(
                Regex.IsMatch("System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                AsynkConfig.Misc.IgnoreFilter));

            Assert.That(
                Regex.IsMatch("Microsoft.DirectX.Direct3DX, Version=1.0.2905.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35",
                AsynkConfig.Misc.IgnoreFilter));

            Assert.That(
                !Regex.IsMatch("Asynk.Library, Version=0.1.0.0, Culture=neutral, PublicKeyToken=791535ff581e7a7b",
                AsynkConfig.Misc.IgnoreFilter));
        }
    }
}
